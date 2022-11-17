#
# UpdateAzureSQL.ps1
#
$DatabaseServer = $env:DatabaseServer
$DatabaseName = $env:DatabaseName
$DatabaseAction = $env:DatabaseAction
$DatabaseUser = $env:DatabaseUser
$DatabasePassword = $env:DatabasePassword
Write-Output "Recursive directory listing for diagnostics"
Get-ChildItem -Recurse
Write-Host "Executing & .\scripts\AliaSQL.exe $DatabaseAction $databaseServer $databaseName .\scripts $databaseUser $databasePassword"
& .\scripts\AliaSQL.exe $DatabaseAction $DatabaseServer $DatabaseName .\scripts $DatabaseUser $DatabasePassword
if ($lastexitcode -ne 0) {
    throw ("AliaSQL had an error.")
}