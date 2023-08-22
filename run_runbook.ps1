$ErrorActionPreference = "Stop";

# Define working variables
$octopusURL = "https://clearmeasure.octopus.app/"
$octopusAPIKey = "API-key"
$header = @{ "X-Octopus-ApiKey" = $octopusAPIKey }
$spaceName = "Onion DevOps"
$projectName = "onion-architecture-dotnet-7-container-apps"
$runbookName = "Unhealthy app alert"
$environmentNames = @("TDD")

# Get space
$space = (Invoke-RestMethod -Method Get -Uri "$octopusURL/api/spaces/all" -Headers $header) | Where-Object {$_.Name -eq $spaceName}
Write-Host "Using Space named $($space.Name) with id $($space.Id)"

# Create the release body
$createRunbookRunCommandV1 = @{
	SpaceId          = $space.Id
    SpaceIdOrName    = $spaceName
    ProjectName      = $projectName
    RunbookName      = $runbookName
    EnvironmentNames = $environmentNames
} | ConvertTo-Json

# Run runbook
Invoke-RestMethod -Method POST -Uri "$OctopusURL/api/$($Space.Id)/runbook-runs/create/v1" -Body $createRunbookRunCommandV1 -Headers $header

