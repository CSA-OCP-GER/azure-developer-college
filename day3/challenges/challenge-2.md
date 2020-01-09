# Azure SQL DB #

## Here is what you will learn ##

- Create an Azure SQL DB
- Add Data to the Azure SQL DB
- Secure the Azure SQL DB
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
  - CREATE TABLE CEOs (StartYear int, LastName varchar(255), FirstName varchar(255), Age int); GO
  - SELECT name FROM sys.tabled; GO
  - INSERT INTO CEOs (StartYear, LastName, FirstName, Age) VALUES (2014, 'Nadella', 'Satya', 52); GO

## Create a Table with SQL Management Studio ##

- TODO: Alex Doku


## Use ARM Template for automated deployment ##

- TODO: Alex Doku
