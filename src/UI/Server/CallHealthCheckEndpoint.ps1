param(
    [string]$server
)
Start-Sleep -Seconds 90
$uri = "$server/_healthcheck"
Write-Host "Smoke testing $uri"
Invoke-WebRequest $uri -UseBasicParsing | Foreach {
    $_.Content.Contains("Healthy") | Foreach {
        if(-Not($_)) {
            Throw "Web smoke test failed"
        }
        else {
            Write-Host "Web smoke test passed."
        }
    }
}