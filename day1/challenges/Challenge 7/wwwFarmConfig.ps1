configuration wwwFarmConfig
{
    param
    (
        [Parameter(mandatory = $true)]
        [string]$webZipURI
    )
    # One can evaluate expressions to get the node list
    # E.g: $AllNodes.Where("Role -eq Web").NodeName
    Import-DSCResource -Module PSDesiredStateConfiguration

    node ("localhost")
    {
        # Call Resource Provider
        # E.g: WindowsFeature
        WindowsFeature WWW {
            Name   = "Web-Server"
            Ensure = "Present"
        }

        WindowsFeature ASPNet45 {
            Name   = "Web-Asp-Net45"
            Ensure = "Present"
        }

        WindowsFeature HTTPRedirection {
            Name   = "Web-Http-Redirect"
            Ensure = "Present"
        }

        WindowsFeature LogginTools {
            Name   = "Web-Log-Libraries"
            Ensure = "Present"
        }

        WindowsFeature RequestMonitor {
            Name   = "Web-Request-Monitor"
            Ensure = "Present"
        }

        WindowsFeature Tracing {
            Name   = "Web-Http-Tracing"
            Ensure = "Present"
        }

        WindowsFeature BasicAuthentication {
            Name   = "Web-Basic-Auth"
            Ensure = "Present"
        }

        WindowsFeature WindowsAuthentication {
            Name   = "Web-Windows-Auth"
            Ensure = "Present"
        }

        Script DownloadPackage {
            GetScript  = {
                return @{'Result' = '' }
            }
        
            SetScript  = 
            {
                $URI = $using:webZipURI
                if ((Test-Path 'c:\temp') -eq $false) { mkdir 'c:\temp' }
                Invoke-WebRequest -Uri "$URI" -OutFile "c:\temp\$(Split-Path "$URI" -Leaf)"
            }
        
            TestScript = 
            {
                $URI = $using:webZipURI
                Write-Verbose -Message "Testing DownloadPackage: $URI"
                Write-Verbose "$(Split-Path $URI -Leaf)"
                Test-Path "c:\temp\$(Split-Path "$URI" -Leaf)"
            }
        }
    
        Archive WebArchive {
            Destination = "c:\inetpub\wwwroot"
            Path        = "c:\temp\$(Split-Path -Path "$webZipURI" -Leaf)"
            DependsOn   = @("[Script]DownloadPackage", "[WindowsFeature]WWW")
        } 
        
        File RemoveWebZip {
            SourcePath      = "c:\temp\$(Split-Path -Path "$webZipURI" -Leaf)"
            DestinationPath = ""
            DependsOn       = "[Archive]WebArchive"
            Ensure          = Absent 
            Force           = $true
            Type            = "File" 
        }
    }
}