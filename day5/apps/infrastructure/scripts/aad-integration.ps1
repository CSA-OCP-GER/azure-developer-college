param(
    [Parameter(Mandatory = $true)]
    [string]$ApiAppName,
    [Parameter(Mandatory = $true)]
    [string]$ApiAppUri,
    [Parameter(Mandatory = $true)]
    [string]$UiAppName,
    [Parameter(Mandatory = $true)]
    [string]$UiAppReplyUrl)
    
    # read the exposed permission first
    $exposedPermissions = Get-Content -Raw -Path ./oauth2-permissions.json |
        ConvertFrom-Json
    # create the ApiApp
    $apiApp = New-AzureADApplication -DisplayName $ApiAppName -IdentifierUris $ApiAppUri -Oauth2Permissions $exposedPermissions
    New-AzureADServicePrincipal -AppId $apiApp.AppId

