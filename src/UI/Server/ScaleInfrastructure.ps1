param(
    [int]$appReplicas = 1,
    [float]$cpu = 0.25,
    [float]$memGi = 0.5,
    [string]$serviceObjective = "Basic"
)

#$azPath = "C:\Program Files (x86)\Microsoft SDKs\Azure\CLI2\wbin"
###################################################################
#Octopus Deploy's default Hosted Windows install includes
#az cli, but the version doesn't include the containerapp
#extension or the upgrade extension.  That's the reason for
#all the az cli installation work here.  Starting with a
#fresh agent and fresh install gives us some flexibility
#in extensions.
#
#That also means that the agent doesn't inherit the
#credentials from Octopus Deploy, so we also have to
#set up az cli with the relevant information for az
#login.  Those details are stored as Project Variables
#for ease of reference and secrecy.
###################################################################

###################################################################
#Intalls az cli latest version from Microsoft. Essentially 
#a copy/paste from MS documentation.
$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi; Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'; rm .\AzureCLI.msi
###################################################################

###################################################################
#az cli installer doesn't consistently add the install
#directory to PATH.  This is here to make sure we can run
#from whatever default directory the agent lands on.
$Env:PATH += ";C:\Program Files (x86)\Microsoft SDKs\Azure\CLI2\wbin\"
Write-Host $env:PATH -Split ';'
###################################################################

###################################################################
#They heavy lifting.  With the cli installed and PATH set
#all that's left to do is configure it for the subscription.
& az extension add --name containerapp
& az login --service-principal -u $az_login_appid -p $az_login_appkey --tenant $az_login_tenant

###################################################################

###################################################################
#All of that to get to this.  This the actual containerapp
#update - set the number of replicas, and scale the database
& az containerapp update --name $container_app_name --resource-group $ResourceGroupName --max-replicas $appReplicas --cpu $cpu --memory "$($memGi)Gi"
& az sql db update --resource-group $ResourceGroupName --server $DatabaseServerName --name $DatabaseName --service-objective $serviceObjective
###################################################################