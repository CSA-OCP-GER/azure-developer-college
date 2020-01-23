# Azure SQL DB #


## Here is what you will learn ##

- Create an Azure SQL DB
- Add Data to the Azure SQL DB
- Secure the Azure SQL DB
- Azure SQL DB elastic pools
- Use ARM Template for automated deployment
Next to creating a single SQL DB we could also create a managed instance and an instance pools
If of interest we could show how to set up a DB with SQL Server Management Studio.

## Create an Azure SQL DB ##

We are going to use the Azure CLI as well as the Azure Portal for this exercise.
1. Create a resource group

```az group create --name [Name of your RG] --location westeurope```

2. Create an Azure SQL Server

```az sql server create --name [Name of your SQL Server] --resource-group [Name of your RG] --location westeurope --admin-user [Name of your Server Admin] --admin-password [Your Admin Password]```

3. Create an Azure SQL DB

```az sql db create --name MicrosoftEmployees --resource-group [Name of your RG] --location westeurope --edition GeneralPurpose --family Gen4 --capacity 1 --zone-redundant false```

4. There are several options to make the Azure SQL DB accessible. Per default the access is not permitted via the Azure SQL Server. To allow you to interact with your DB you can create a Firewall-Rule to allow your IP on the Server and hence the DB

```az sql server firewall-rule create --name [Name of your Firewall Rule] --resource-group [Name of your RG] --server [Name of your server] --start-ip-address [your IP address] --end-ip-address [your IP address]```

4. Optional: Add scalability options

```az sql db list-usage --name MicrosoftEmployees --resource-group [Name of your RG] --server [Name of your SQL Server]```
```az sql db create --name MicrosoftEmployees --resource-group [Name of your RG] --location westeurope --edition GeneralPurpose --family Gen4 --capacity 2 --zone-redundant false```
  

## Add Data to SQL DB ##

A Table will be created and Data entered

Use the Azure Cloud Shell

1. Get to know your environment

  ```az sql server list --resource-group [Name of your RG]```
  ```az sql db list --resource-group [Name of your RG] --server [Name of your Server]```
  
If you run the command like this you are getting a lot of information to make sense of. You can restrict this by using a query

  ```az sql db list --resource-group [Name of your RG] --query '[].{Name:name}'```
  
  ```az sql db show --resource-group [Name of your RG] --name MicrosoftEmployees --query '{name: .name, maxSizeBytes: .maxSizeBytes, status: .status}'```
  
2. Connect to the DB

  ```az sql db show-connection-string --name MicrosoftEmployees --server [Name of your Server] --client sqlcmd```

Copy the sqlcmd command and enter your admin name and password. The command should look something like this:
  
  ```sqlcmd -S tcp:[Name of your Server].database.windows.net,1433 -d MicrosoftEmployees -U [Name of your Server Admin] -P [Your Admin Password] -N -l 30```
  
After running this you should see a ```1>```. Now you can run SQL Queries. If you are unfamiliar with their Syntax feel free to take some time getting used to it.

3. Add a table.

  ```CREATE TABLE CEOs (EmployerID int, LastName varchar(255), FirstName varchar(255), Age int, StartYear int); GO```

4. Add Data to your table

  ```INSERT INTO CEOs (EmployerID, LastName, FirstName, Age, StartYear) VALUES (42, 'Nadella', 'Satya', 51, 2014); GO```

5. Update the Age of Satya Nadella in the Table

  ```UPDATE CEOs SET Age=52 WHERE EmployerID=42; GO```
  
6. Query the data

   ```SELECT * FROM CEOs;```
  
6. Add the other CEOs Microsoft has had to the list as well (the ID is fictional). To ```exit``` enter exit.


## Secure the Azure SQL DB ##

There are many tasks surrounding the securing of an Azure SQL DB and and Azure SQL Server.

### Network Security for Azure SQL DB ###

To help protect the data, firewalls prevent network access to the database server until access is explicitly granted based on IP address or Azure Virtual network traffic origin. Earlier we opened up the SQL Server for our own IP address by creating a firewall rule. Now we are having a look at Virtual network firewall rules.

1. Create the Virtual Network

```az network vnet create --name [Name of your VNet] --resource-group [Name of your RG] --location westeurope --address-prefixes [xxx.xxx.xxx.xxx/xx]```

2. Create the service endpoint and take a look at what was created

```az network vnet subnet create --name [Name of your Subnet] --resource-group [Name of your RG] --vnet-name [Name of your VNet] --address-prefix [xxx.xxx.xxx.xxx/xx] --service-endpoints Microsoft.SQL```

```az network vnet subnet show --name [Name of your Subnet] --resource-group [Name of your RG] --vnet-name [Name of your VNet]```

3. Create a VNet rule on the server to secure it to the subnet Note

```az sql server vnet-rule create --name [Name of your VNet Rule] --resource-group [Name of your RG] --vnet-name [Name of your VNet] --subnet [Name of your Subnet] --server [Name of your SQL Server]```

Note: Controlling access with firewall rules does not apply to a managed instance.

### Access Management for Azure SQL DB ###

Under Access Management we are taking a look at authentication and authorization. Authentication is the process of proving the user is who they claim to be. Authorization refers to the permissions assigned to a user within an Azure SQL Database, and determines what the user is allowed to do.

Azure SQL Database supports two types of authentication: SQL authentication, as has been set up with the server admin and password while creating the Azure SQL Server. And Azure Active Directory authentication.

1. Let's have a look at the SQL authentication. As server admin you can create additional SQL logins and users - which enables other users to connect to the SQL Database. For this one open the Azure portal.

```sqlcmd -S tcp:[Name of your Server].database.windows.net,1433 -d MicrosoftEmployees -U [Name of your Server Admin] -P [Your Admin Password] -N -l 30```

```CREATE USER Marvin WITH PASSWORD = '42_as_ANSWER!'; GO```

2. Now create a new Table with this user.

```exit```

```sqlcmd -S tcp:[Name of your Server].database.windows.net,1433 -d MicrosoftEmployees -U Marvin -P 42_as_ANSWER! -N -l 30```

```CREATE TABLE Bees (EmployerID int, LastName varchar(255), FirstName varchar(255), Role varchar (255), StartYear int); GO```

Add some Data to the table and query them.

When doing this remember that there is still a firewall rule in place - the new User currently has to operate under the same IP address you previously added to the SQL server firewall rule.

Note: The Azure Active Directory authentication is a far more suitable and contemporary solution. Learn more about it at our Friday sessions.

Authorization refers to the permissions assigned to a user. Permissions are controlled by adding user accounts to database roles and assigning database-level permissions to those roles or by granting the user certain object-level permissions. As always there is more than one way to implement this.

1. There are also database roles for SQL Server and database. You will find fixed roles as well as custom roles. Let's have a look at the fixed roles. To add and remove users to or from a database role, use the ADD MEMBER and DROP MEMBER options of the ALTER ROLE statement.

    ```sqlcmd -S tcp:[Name of your Server].database.windows.net,1433 -d MicrosoftEmployees -U [Name of your Server Admin] -P [Your Admin Password] -N -l 30```

   ```ALTER ROLE  db_backupoperator  
      {  
        ADD MEMBER database_principal  
        |  DROP MEMBER database_principal  
        |  WITH NAME = new_name  
      }  
      [;]  ```

2. Custom roles can be created by granting access to specific Objects and Users. In this example, we will block access from a specific value for the previously added user Marvin.

   ```sqlcmd -S tcp:[Name of your Server].database.windows.net,1433 -d MicrosoftEmployees -U [Name of your Server Admin] -P [Your Admin Password] -N -l 30```
   
   ```REVOKE SELECT ON OBJECT::CEOs.EmployerID TO Marvin; GO```
   
   ```exit```
   
   ```sqlcmd -S tcp:[Name of your Server].database.windows.net,1433 -d MicrosoftEmployees -U Marvin -P 42_as_ANSWER! -N -l 30```
   
Now try to access the variable. 
As Admin you can grant access again like this:

   ```GRANT SELECT ON OBJECT::CEOs.EmployerID TO Marvin; GO```

### Threat protection ###

SQL Database secures data by providing auditing and threat detection capabilities.

1. As part of that Advanced Threat Protection is analyzing your SQL Server logs to detect unusual behavior and potentially harmful attempts to access or exploit databases. Alerts are created for suspicious activities such as SQL injection, potential data infiltration, and brute force attacks or for anomalies in access patterns to catch privilege escalations and breached credentials use. Alerts are viewed from the Azure Security Center, where the details of the suspicious activities are provided and recommendations for further investigation given along with actions to mitigate the threat. We are going to enable this feature.

   ```az sql db threat-policy update --resource-group [Name of your RG] --server [Name of your Server] --name MicrosoftEmployees --email-account-admins Enabled --email-addresses [any E-Mail Address]```

2. SQL Database auditing tracks database activities and helps to maintain compliance with security standards by recording database events to an audit log in a customer-owned Azure storage account. Auditing allows users to monitor ongoing database activities, as well as analyze and investigate historical activity to identify potential threats or suspected abuse and security violations.
First we need to create a storage account to save the audit logs to.

```az storage account create --name [Name of your Storage Account] --resource-group [Name of your RG] --location westeurope```

```az sql db audit-policy update --storage-account [Name of your Storage Account] --server [Name of your Server] --resource-group [Name of your RG] --name [Name of your DB]```

This is a complex topic. To grasp it fully visit the Azure portal. Go to your Azure SQL DB. Under Security move to the Auditing tab. Now enable Log Analytics and Event Hub.

### Information protection and encryption ###

There are different Methods to secure the information stored in a SQL Database. We are going to look at the standards. For advanced security you can look up the Azure Key Vault and Always Encrypted.

1. SQL Database secures customer data by encrypting data in motion with Transport Layer Security.

   Sql Server enforces encryption (SSL/TLS) at all times for all connections. This ensures all data is encrypted "in transit" between the client and server irrespective of the setting of Encrypt or TrustServerCertificate in the connection string.

   As a best practice, recommend that in your application's connection string you specify an encrypted connection and not trust the server certificate. This forces your application to verify the server certificate and thus prevents your application from being vulnerable to man in the middle type attacks.

   For example when using the ADO.NET driver this is accomplished via Encrypt=True and TrustServerCertificate=False. If you obtain your connection string from the Azure portal, it will have the correct settings and should look like this:

   ```Server=tcp:[Name of your SQL Server].database.windows.net,1433;Initial Catalog=[Name of your DB];Persist Security Info=False;User ID=[Name of your Admin];Password=[Your Admin Password];MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;```
   
2. Transparent Data Encryption (TDE) for Azure SQL Database adds a layer of security to help protect data at rest from unauthorized or offline access to raw files or backups. Common scenarios include datacenter theft or unsecured disposal of hardware or media such as disk drives and backup tapes. TDE encrypts the entire database using an AES encryption algorithm, which doesn’t require application developers to make any changes to existing applications.

   In Azure, all newly created SQL databases are encrypted by default and the database encryption key is protected by a built-in server certificate. Certificate maintenance and rotation are managed by the service and requires no input from the user. Customers who prefer to take control of the encryption keys can manage the keys in Azure Key Vault.
   
   ```az sql db tde set --status Enabled --database MicrosoftEmployees --resource-group [Name of your RG] --server [Name of your SQL Server]```

1. SQL Database backup/Configure retention policies

  
2. Restore a Database

    
1. High-availability
  - Zone-redundancy configuration - change tier, Accelerated Database Recovery


AD authentication:
```CREATE USER [Azure AD principal name] FROM EXTERNAL PROVIDERS;```

3. Encrypt Data
  - Mask data
  
  ```Set-AzSqlDatabaseTransparentDataEncryption -ServerName [Name of your SQL Server] -DatabaseName MicrosoftEmployees -ResourceGroupName [Name of your RG]```
  ```Get-AzSqlDatabaseTransparentDataEncryptionActivity -ServerName [Name of your SQL Server] -DatabaseName MicrosoftEmployees -ResourceGroupName [Name of your RG]```
  
## Connect the Azure SQL DB to a Web Application ##

1. Get Web App basics

2. Create Web App

3. Modify Web App?

## Azure SQL DB elastic pools ##

1. Create an elastic pool
  
  ```az sql elastic-pool create --name [Name of you SQL EP] --resource-group [Name of your RG] --server [Name of your SQL Server] --edition GeneralPurpose --family Gen4 --capacity 2```

2. Move the Azure SQL DB to the elastic pool

  ```az sql db update --name MicrosoftEmployees --resource-group [Name of your RG] --server [Name of your SQL Server] --elastic-pool [Name of you SQL EP]```


## Use ARM Template for automated deployment ##

https://github.com/Azure/azure-quickstart-templates/tree/master/201-sql-database-transparent-encryption-create

https://docs.microsoft.com/de-de/azure/sql-database/sql-database-resource-manager-samples?tabs=single-database


## Clean up ##

Delete Resource Group

```az group delete -name [Name of your RG]```
