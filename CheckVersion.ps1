$server = "$env:AppUrl"
$uri = "$server/version"
Write-Host "Getting version $uri"
Invoke-WebRequest $uri -UseBasicParsing | Foreach {
    $_.Content.Contains($(Build.BuildNumber)) | Foreach {
        if(-Not($_)) {
            Throw "Incorrect version."
        }
        else {
            Write-Host "Correct version: $(Build.BuildNumber)"
        }
    }
}