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

You can either use the portal or the Azure Cloud Shell
1. Create a resource group

```az group create --name [Name of your RG] --location westeurope```

2. Create an Azure SQL Server

```az sql server create --name [Name of your SQL Server] --resource-group [Name of your RG] --location westeurope --admin-user [Name of your Server Admin] --admin-password [Your Admin Password]```

3. Create an Azure SQL DB

```az sql db create --name MicrosoftEmployees --resource-group [Name of your RG] --location westeurope --edition GeneralPurpose --family Gen4 --capacity 1 --zone-redundant false```

4. Optional: Add scalability options

```az sql db list-usage --name MicrosoftEmployees --resource-group [Name of your RG] --server [Name of your SQL Server]```
```az sql db create --name MicrosoftEmployees --resource-group [Name of your RG] --location westeurope --edition GeneralPurpose --family Gen4 --capacity 2 --zone-redundant false```
  

## Add Data to SQL DB ##

Add Azure Data Studio training and/or SSMS queriing

Use the Azure Cloud Shell
1. Get to know the Database

  ```az sql db list --resource-group [Name of your RG] | jq '[.[]|{name: .name}]'```
  ```az sql db show --resource-group [Name of your RG] --name MicrosoftEmployees | jq '{name: .name, maxSizeBytes: .maxSizeBytes, status: .status}'```
  
2. Connect to the DB

  ```az sql db show-connection-string --resource-group [Name of your RG] --client sqlcmd --name MicrosoftEmployees```
  ```sqlcmd -S tcp ...```

3. Add a table

  ```CREATE TABLE CEOs (EmployerID int, LastName varchar(255), FirstName varchar(255), Age int, StartYear int); GO```
  ```SELECT name FROM sys.tabled; GO```
  ```INSERT INTO CEOs (EmployerID, LastName, FirstName, Age, StartYear) VALUES (3141, 'Nadella', 'Satya', 51, 2014); GO```
  ```UPDATE CEOs SET Age=52 WHERE EmployerID=3141; GO```


## Secure the Azure SQL DB ##

We are going to have to use either the Azure portal or PowerShell for parts of securing the Azure SQL DB, due to the Azure Data Explorers Endpoints

1. SQL Database backup/Configure retention policies

  ```Get-AzSqlDatabase -ResourceGroupName [Name of your RG] -ServerName [Name of your SQL DB Server] | Get-AzSqlDatabaseLongTermRetentionPolicy```
  ```Get-AzSqlDatabaseBackupLongTermRetentionPolicy -ResourceGroupName [Name of your RG] -ServerName [Name of your SQL Server] -DatabaseName MicrosoftEmployees```
  ```Set-AzSqlDatabaseBackupLongTermRetentionPolicy -ResourceGroupName [Name of your RG] -ServerName [Name of your SQL Server] -DatabaseName MicrosoftEmployees -WeeklyRetention P8W -MonthlyRetention P5M -YearlyRetention P5Y -WeekOfYear 1```
  
2. Restore a Database (only in theory)

  ```Restore-AzSqlDatabase -PointInTime [DateTime] -ResourceId [String] -ServerName [String] -TargetDatabaseName [String]```  or  ```az sql db restore```
  ```Get-AzSqlDeletedDatabaseBackup -DeletionDate [DateTime] -ResourceGroupName [String] -ServerName [String] -DatabaseName [String]```
    
  
1. High-availability
  - Zone-redundancy configuration - change tier, Accelerated Database Recovery

1. Restrict Network access with firewall-rules

```az aql server firewall-rule list --server [Name of your SQL Server] --resource-group [Name of your RG]```
```az sql server firewall-rule create --name [Name of a firewall-rule] --server [Name of your SQL Server] --resource-group [Name of your RG] --location westeurope --start-ip-address [choose IP address] --end-ip-address [choose IP address]```

2. Restrict Database access (SSMS)

SQL authentication:
```CREATE USER ApplicationUser WITH PASSWORD = ['another password'];```
```ALTER ROLE db_datareader ADD MEMBER ApplicationUser; ALTER ROLE db_datawriter ADD MEMBER ApplicationUser;```

AD authentication:
```CREATE USER [Azure AD principal name] FROM EXTERNAL PROVIDERS;```

3. Encrypt Data
  - Mask data
  
  ```Set-AzSqlDatabaseTransparentDataEncryption -ServerName [Name of your SQL Server] -DatabaseName MicrosoftEmployees -ResourceGroupName [Name of your RG]```
  
4. Monitor the DB
  - activate Azrue SQL DB Monitoring
  - Advanced Data Security


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
