# Azure SQL DB #

## Here is what you will learn ##

- Create an Azure SQL DB
- Add Data to the Azure SQL DB
- Secure the Azure SQL DB
- Azure SQL DB elastic pools
- Use ARM Template for automated deployment

## Create an Azure SQL DB ##

You can either use the portal or the Azure Cloud Shell
1. Create a resource group
  - location: westeurope
  - Hit create
2. Create an Azure SQL Database
  - Resoucegroup: as created before
  - Databasename: MicrosoftEmployees
  - elastic Pools: no
  - Compute and Storage: choose a proper tarif
  - Create new server:
      - location: westeurope
      - allow azure service access: yes
  - Datasource: none
  - Sort: SQL_Latin1_General_CP1_CI_AS
  - Advances Data Security: not now
  - Hit create

## Add Data to SQL DB ##

Use the Azure Cloud Shell
1. Get to know the Database
  - az sql db list --resource-group [Name of your RG] | jq '[.[]|{name: .name}]'
  - az sql db show --resource-group [Name of your RG] --name [Name of your SQL DB] | jq '{name: .name, maxSizeBytes: .maxSizeBytes, status: .status}'
2. Connect to the DB
  - az sql db show-connection-string --resource-group [Name of your RG] --client sqlcmd --name [Name of your SQL DB]
  - sqlcmd -S tcp ...
3. Add a table
  - CREATE TABLE CEOs (EmployerID int, LastName varchar(255), FirstName varchar(255), Age int, StartYear int); GO
  - SELECT name FROM sys.tabled; GO
  - INSERT INTO CEOs (EmployerID, LastName, FirstName, Age, StartYear) VALUES (3141, 'Nadella', 'Satya', 51, 2014); GO
  - UPDATE CEOs SET Age=52 WHERE EmployerID=3141; GO

## Secure the Azure SQL DB ##

We are going to have to use either the Azure portal or PowerShell for parts of securing the Azure SQL DB, due to the Azure Data Explorers Endpoints
1. Configure retention policies
  - Get-AzSqlDatabase -ResourceGroupName [Name of your RG] -ServerName [Name of your SQL DB Server] | Get-AzSqlDatabaseLongTermRetentionPolicy
  - Get-AzSqlDatabaseBackupLongTermRetentionPolicy -ResourceGroupName [Name of your RG] -ServerName [Name of your SQL DB Server] -DatabaseName [Name of your SQL DB]
  - Set-AzSqlDatabaseBackupLongTermRetentionPolicy -ResourceGroupName [Name of your RG] -ServerName [Name of your SQL DB Server] -DatabaseName [Name of your SQL DB] -WeeklyRetention P8W -MonthlyRetentionP5M -YearlyRetention P5Y -WeekOfYear 1
2. Restore a Database (only in theory)
  - Restore-AzSqlDatabase  or  az sql db restore

1. Restrict Network access
  - Easy way: Portal
2. Restrict Database access
3. Encrypt Data
  - Mask data
4. Monitor the DB
  - activate Azrue SQL DB Monitoring
  - Advanced Data Security

## Azure SQL DB elastic pools ##

1. Create an elastic pool
  - az sql elastic-pool create --name [Name of you SQL EP] --resource-group [Name of your RG] --server [Name of your SQL DB Server] --edition GeneralPurpose --family Gen4 --capacity 2
2. Move the Azure SQL DB to the elastic pool
  - az sql db update --name [Name of your SQL DB] --resource-group [Name of your RG] --server [Name of your SQL DB Server] --elastic-pool [Name of you SQL EP]

## Use ARM Template for automated deployment ##

- TODO: Alex Doku
