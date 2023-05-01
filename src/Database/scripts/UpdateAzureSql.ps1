#
# UpdateAzureSQL.ps1
#
$DatabaseServer = $OctopusParameters["DatabaseServer"]
$DatabaseName = $OctopusParameters["DatabaseName"]
$DatabaseAction = $OctopusParameters["DatabaseAction"]
$DatabaseUser = $OctopusParameters["DatabaseUser"]
$DatabasePassword = $OctopusParameters["DatabasePassword"]
Write-Output "Recursive directory listing for diagnostics"
Get-ChildItem -Recurse
Set-Location ..
Write-Host "Executing & .\scripts\AliaSQL.exe $DatabaseAction $databaseServer $databaseName .\scripts $databaseUser $databasePassword"
& .\scripts\AliaSQL.exe $DatabaseAction $DatabaseServer $DatabaseName .\scripts $DatabaseUser $DatabasePassword
if ($lastexitcode -ne 0) {
    throw ("AliaSQL had an error.")
}