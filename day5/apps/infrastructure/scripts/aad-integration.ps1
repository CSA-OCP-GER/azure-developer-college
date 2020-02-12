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
    $exposedPermissionsFromJson = Get-Content -Raw -Path ./oauth2-permissions.json `
        | ConvertFrom-Json

    Write-Host $exposedPermissionsFromJson

    $exposedPermissions = New-Object System.Collections.Generic.List[Microsoft.Open.AzureAD.Model.OAuth2Permission]

    $exposedPermissionsFromJson |
        ForEach-Object -Process {
            $permission = New-Object Microsoft.Open.AzureAD.Model.OAuth2Permission
            $permission.Id = [System.Guid]::Parse($_.id)
            $permission.Origin = $_.origin
            $permission.IsEnabled = $true
            $permission.Type = $_.type
            $permission.AdminConsentDisplayName = $_.adminConsentDisplayName
            $permission.AdminConsentDescription = $_.adminConsentDescription
            $permission.UserConsentDescription = $_.userConsentDescription
            $permission.UserConsentDisplayName = $_.userConsentDisplayName
            $permission.Value = $_.value

            $exposedPermissions.Add($permission)
        }
    # create the ApiApp
    $apiApp = New-AzureADApplication -DisplayName $ApiAppName -IdentifierUris $ApiAppUri -Oauth2Permissions $exposedPermissions -AvailableToOtherTenants $false 
    New-AzureADServicePrincipal -AppId $apiApp.AppId

