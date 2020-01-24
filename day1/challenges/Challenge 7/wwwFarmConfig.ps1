configuration wwwFarmConfig
{
    # One can evaluate expressions to get the node list
    # E.g: $AllNodes.Where("Role -eq Web").NodeName
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

        File DownloadPackage {            	
            Ensure          = "Present"              	
            Type            = "File"             	
            SourcePath      = "https://github.com/CSA-OCP-GER/azure-developer-college/raw/features/day1handson/day1/challenges/Challenge%207/web.zip"            	
            DestinationPath = "C:\temp"   
  
        }
        Archive WebArchive {
            Destination = "c:\inetpub\wwwroot"
            Path        = "c:\temp\web.zip"
            DependsOn   = @("[File]DownloadPackage", "[WindowsFeature]WWW")
         
        }
    }
}




