$ErrorActionPreference = "Stop";

# Define working variables
$octopusURL = Get-AutomationVariable -Name 'octopusURL'
$octopusAPIKey = Get-AutomationVariable -Name 'apiKey'
$header = @{ "X-Octopus-ApiKey" = $octopusAPIKey }
$spaceName = Get-AutomationVariable -Name 'octoSpace'
$projectName = Get-AutomationVariable -Name 'octoProject'
$runbookName = Get-AutomationVariable -Name 'octoRunbook'
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

