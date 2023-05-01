param(
    [str]$server,
    [str]$version
)

$uri = "$server/version"
Write-Host "Getting version $uri"
Invoke-WebRequest $uri -UseBasicParsing | Foreach {
    $_.Content.Contains($version) | Foreach {
        if(-Not($_)) {
            Throw "Incorrect version."
        }
        else {
            Write-Host "Correct version: $version"
        }
    }
}