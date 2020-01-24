configuration wwwFarmConfig
{
    # One can evaluate expressions to get the node list
    # E.g: $AllNodes.Where("Role -eq Web").NodeName
    node ("localhost")
    {
        # Call Resource Provider
        # E.g: WindowsFeature, File
        WindowsFeature WWW {
            Ensure = "Present"
            Name   = "Feature Name"
        }

        File DownloadPackage {            	
            Ensure          = "Present"              	
            Type            = "File"             	
            SourcePath      = "https://storageaccount.blob.core.windows.net/mycontainer/web.zip"            	
            DestinationPath = "C:\temp"   
  
        }
        Archive WebArchive {
            Destination = "c:\inetpub\wwwroot"
            Path        = "c:\temp\web.zip"
            DependsOn   = @("[File]DownloadPackage", "[WindowsFeature]WWW")
         
        }
    }
}




