$server = "$env:AppUrl"
$uri = "$server/version"
$buildnumber = "$env:Version"
Write-Host "Getting version $uri"
Write-Host "Version should be: $buildnumber"
Invoke-WebRequest $uri -UseBasicParsing | Foreach {
    $_.Content.Contains($buildnumber) | Foreach {
        if(-Not($_)) {
            Throw "Incorrect version."
        }
        else {
            Write-Host "Correct version: $buildnumber"
        }
    }
}