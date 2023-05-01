$server = "$env:AppUrl"
$uri = "$server/version"
$buildnumber = "$env:buildnumber"
Write-Host "Getting version $uri"
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