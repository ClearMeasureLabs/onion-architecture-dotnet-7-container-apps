param(
    $octopusURL = "",
    $octopusAPIKey = "",
    $projectName = "",
    $spaceName = "",
    $environment = $null,
    $varName = "",
    $gitRefOrBranchName = ""
)

# Defines header for API call
$header = @{ "X-Octopus-ApiKey" = $octopusAPIKey }

# Get space
$space = (Invoke-RestMethod -Method Get -Uri "$octopusURL/api/spaces/all" -Headers $header) | Where-Object {$_.Name -eq $spaceName}

# Get project
$project = (Invoke-RestMethod -Method Get -Uri "$octopusURL/api/$($space.Id)/projects/all" -Headers $header) | Where-Object {$_.Name -eq $projectName}

# Get project variables
$databaseVariables = Invoke-RestMethod -Method Get -Uri "$octopusURL/api/$($space.Id)/variables/$($project.VariableSetId)" -Headers $header

if($project.IsVersionControlled -eq $true) {
    if ([string]::IsNullOrWhiteSpace($gitRefOrBranchName)) {
        $gitRefOrBranchName = $project.PersistenceSettings.DefaultBranch
        Write-Output "Using $($gitRefOrBranchName) as the gitRef for this operation."
    }
    $versionControlledVariables = Invoke-RestMethod -Method Get -Uri "$octopusURL/api/$($space.Id)/projects/$($project.Id)/$($gitRefOrBranchName)/variables" -Headers $header
}

# Get environment values to scope to
$environmentObj = $databaseVariables.ScopeValues.Environments | Where { $_.Name -eq $environment } | Select -First 1

# Define values for variable
$variable = @{
    Name = $varName  # Replace with a variable name
    Value = $varValue # Replace with a value
    Type = "String"
    IsSensitive = $false
    Scope = @{ 
        Environment = @(
            $environmentObj.Id
            )
        }
}
# Assign the correct variables based on version-controlled project or not
$projectVariables = $databaseVariables

if($project.IsVersionControlled -eq $True -and $variable.IsSensitive -eq $False) {
    $projectVariables = $versionControlledVariables
}

$variablesWithSameName = $projectVariables.Variables | Where-Object {$_.Name -eq $variable.Name}

if ($environmentObj -eq $null){
    # The variable is not scoped to an environment
    $unscopedVariablesWithSameName = $variablesWithSameName | Where-Object { $_.Scope -like $null}
    return $unscopedVariablesWithSameName.Value
} 

if (@($variablesWithSameName.Scope.Environment) -contains $variable.Scope.Environment) {
    $variablesWithMatchingNameAndScope = $variablesWithSameName | Where-Object { $_.Scope.Environment -like $environmentObj.Id}
    return $variablesWithMatchingNameAndScope.Value
}

