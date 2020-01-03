# Azure Web Apps #

## Here is what you will learn ##

- Create an AppService Plan / Azure Web App
- Create and deploy an ASP.NET Core Web App to Azure
- Create and configure WebApp slots
- Use slots to deploy new versions of your web application with near-zero downtime

## Create an Azure Web App ##

Create a webapp:

- Create a resource group
  - westeurope
- Add web app
- Publish: Code
- Runtime: .NET Core 3.0
- Operation System: Windows
- Region: Westeurope
- SKU and Size: S1
  - Get familiar with other SKUs
- Enable AppInsights
- Hit "Create"

## Create a sample application ##

Install dotnet core SDK: https://dotnet.microsoft.com/download
Check installation:

```shell
$ dotnet
```

If everything is okay:

```shell
$ dotnet new mvc -o myFirstCoreApp
```

Open VS Code: code .
Get familiar with the environment
Have a look at the controller HomeController
Set a breakpoint (F9) on method public IActionResult Index() in Controllers/HomeController.cs

Press F5 - if VS Code asks you about the environment, choose .NET Core

Open th Debug Tools 


When the breakpoint gets hit, get familiar with the tools of the debugger.
Open Views/Home/Index.cshtml and change the welcome text to "Welcome to your first WebApp".
Run it again locally and check, if the changes appear.

Now let's deploy the webapp to Azure.
Add Azure App Service Extension: https://marketplace.visualstudio.com/items?itemName=ms-azuretools.vscode-azureappservice
Find your webapp in the extension and right-click --> Deploy to Web App…
After a few seconds the browser will show you your first web app running in Azure


PORTAL

Open your web app in the portal
Got to "Deployment Slots" and create a new slot called "Staging" (Clone settings from your production slot)
When finished, go back to VS Code

LOCAL/VS CODE

Open Views/Home/Index.cshtml again and change the welcome text to "Welcome to your first WebApp - this time with slots!".
Find your webapp in the Azure AppService extension, drill down to slots and right-click --> Deploy to Slot…

PORTAL

Click on the newly created slot "Staging" and copy the URL in the overview blade.
Open your browser, point it to the URL and check, if the headline contains the new text.
Also check the production slot (URL without "-staging")

Now if everything works as expected, go back to "Deployment Slots" and click on "Swap" (selecting the staging slot as source). 
Click "Swap" at the bottom of the screen.

Now check, that the production slot serves the new version of the website.
