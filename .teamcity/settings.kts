import jetbrains.buildServer.configs.kotlin.*
import jetbrains.buildServer.configs.kotlin.buildFeatures.XmlReport
import jetbrains.buildServer.configs.kotlin.buildFeatures.approval
import jetbrains.buildServer.configs.kotlin.buildFeatures.dockerSupport
import jetbrains.buildServer.configs.kotlin.buildFeatures.perfmon
import jetbrains.buildServer.configs.kotlin.buildFeatures.xmlReport
import jetbrains.buildServer.configs.kotlin.buildSteps.DotnetVsTestStep
import jetbrains.buildServer.configs.kotlin.buildSteps.dockerCommand
import jetbrains.buildServer.configs.kotlin.buildSteps.dotnetVsTest
import jetbrains.buildServer.configs.kotlin.buildSteps.nuGetPublish
import jetbrains.buildServer.configs.kotlin.buildSteps.powerShell
import jetbrains.buildServer.configs.kotlin.projectFeatures.buildReportTab
import jetbrains.buildServer.configs.kotlin.projectFeatures.dockerRegistry
import jetbrains.buildServer.configs.kotlin.projectFeatures.nuGetFeed
import jetbrains.buildServer.configs.kotlin.triggers.vcs

/*
The settings script is an entry point for defining a TeamCity
project hierarchy. The script should contain a single call to the
project() function with a Project instance or an init function as
an argument.

VcsRoots, BuildTypes, Templates, and subprojects can be
registered inside the project using the vcsRoot(), buildType(),
template(), and subProject() methods respectively.

To debug settings scripts in command-line, run the

    mvnDebug org.jetbrains.teamcity:teamcity-configs-maven-plugin:generate

command and attach your debugger to the port 8000.

To debug in IntelliJ Idea, open the 'Maven Projects' tool window (View
-> Tool Windows -> Maven Projects), find the generate task node
(Plugins -> teamcity-configs -> teamcity-configs:generate), the
'Debug' option is available in the context menu for the task.
*/

version = "2023.05"

project {

    buildType(Build_2)
    buildType(Tdd)
    buildType(Uat)
    buildType(IntegrationBuild)
    buildType(Build)
    buildType(Prod)
    buildType(DeleteTdd)

    params {
        param("env.containerAppURL", "")
        param("OctoSpace", "Spaces-195")
        param("env.BuildConfiguration", "Release")
        param("TDD-Resource-Group", "onion-architecture-dotnet-7-containers-TDD")
        param("TDD-App-Name", "tdd-ui")
        param("OctoProject", "teamcity-dotnet-7-container-apps")
        param("OctoSpaceName", "Onion DevOps")
        param("env.BUILD_BUILDNUMBER", "%build.number%")
        param("AzAppId", "767d5e60-4d25-4794-9a4d-f714fab829e0")
        param("env.Version", "%build.number%")
        password("AzPassword", "credentialsJSON:b66a8739-aa0b-4987-a245-07c6907bdd01", label = "AzPassword")
        param("OctoURL", "https://clearmeasure.octopus.app/")
        password("OctoApiKey", "credentialsJSON:959b363e-7a9f-4706-86fa-532f285020e7", label = "OctoApiKey")
        password("AzTenant", "credentialsJSON:d16337c7-5751-4ecd-9110-f82755b0ebca", label = "AzTenant")
    }

    features {
        buildReportTab {
            id = "PROJECT_EXT_2"
            title = "Code Coverage"
            startPage = "coverage.zip!index.html"
        }
        dockerRegistry {
            id = "PROJECT_EXT_3"
            name = "Onion-Arch ACR"
            url = "onionarchitecturedotnet7containers.azurecr.io"
            userName = "767d5e60-4d25-4794-9a4d-f714fab829e0"
            password = "credentialsJSON:b66a8739-aa0b-4987-a245-07c6907bdd01"
        }
        nuGetFeed {
            id = "repository-nuget-Onion_Architecture_Container_Apps"
            name = "Onion_Architecture_Container_Apps"
            description = ""
        }
    }
    buildTypesOrder = arrayListOf(IntegrationBuild, Build, Tdd, DeleteTdd, Uat, Prod)
}

object Build : BuildType({
    name = "DockerBuildAndPush"

    buildNumberPattern = "${IntegrationBuild.depParamRefs.buildNumber}"

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        powerShell {
            name = "Install UI nupkg"
            scriptMode = script {
                content = """
                    ${'$'}nupkgPath = "build/ChurchBulletin.UI.%build.number%.nupkg"
                    ${'$'}destinationPath = "built"
                    
                    Add-Type -AssemblyName System.IO.Compression.FileSystem
                    
                    [System.IO.Compression.ZipFile]::ExtractToDirectory(${'$'}nupkgPath, ${'$'}destinationPath)
                """.trimIndent()
            }
        }
        dockerCommand {
            name = "Docker Build"
            commandType = build {
                source = file {
                    path = "Dockerfile"
                }
                contextDir = "."
                namesAndTags = "onionarchitecturedotnet7containers.azurecr.io/churchbulletin.ui:%build.number%"
            }
        }
        dockerCommand {
            name = "Docker Push"
            commandType = push {
                namesAndTags = "onionarchitecturedotnet7containers.azurecr.io/churchbulletin.ui:%build.number%"
            }
        }
    }

    features {
        perfmon {
        }
        dockerSupport {
            loginToRegistry = on {
                dockerRegistryId = "PROJECT_EXT_3"
            }
        }
    }

    dependencies {
        dependency(IntegrationBuild) {
            snapshot {
            }

            artifacts {
                cleanDestination = true
                artifactRules = "+:**=>build"
            }
        }
    }
})

object Build_2 : BuildType({
    name = "Deploy Application"

    type = BuildTypeSettings.Type.COMPOSITE
    buildNumberPattern = "${IntegrationBuild.depParamRefs.buildNumber}"

    vcs {
        root(DslContext.settingsRoot)

        showDependenciesChanges = true
    }

    triggers {
        vcs {
        }
    }

    features {
        perfmon {
        }
    }

    dependencies {
        snapshot(DeleteTdd) {
        }
        snapshot(Prod) {
        }
    }
})

object DeleteTdd : BuildType({
    name = "Delete TDD"

    buildNumberPattern = "${IntegrationBuild.depParamRefs.buildNumber}"

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        powerShell {
            name = "Delete TDD Resources"
            scriptMode = script {
                content = """
                    Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi
                    Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'
                    Remove-Item .\AzureCLI.msi
                    ${'$'}env:PATH += ";C:\Program Files (x86)\Microsoft SDKs\Azure\CLI2\wbin"
                    
                    
                    az config set extension.use_dynamic_install=yes_without_prompt
                    # Log in to Azure
                    az login --service-principal --username %AzAppId% --password %AzPassword% --tenant %AzTenant%
                    
                    az group delete -n %TDD-Resource-Group%-%build.number% --yes
                """.trimIndent()
            }
        }
    }

    features {
        perfmon {
        }
    }

    dependencies {
        snapshot(Tdd) {
        }
    }

    requirements {
        matches("teamcity.agent.jvm.os.family", "Windows")
    }
})

object IntegrationBuild : BuildType({
    name = "Integration Build"

    allowExternalStatus = true
    artifactRules = """
        build/*.nupkg
        build/reports => coverage.zip
    """.trimIndent()
    buildNumberPattern = "3.0.%build.counter%"

    params {
        param("dotnet.cli.test.reporting", "off")
    }

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        powerShell {
            name = "Enable LocalDB"
            formatStderrAsError = true
            scriptMode = script {
                content = """
                    # Download the SqlLocalDB.msi installer from the Microsoft website
                    ${'$'}installerPath = "${'$'}env:TEMP\SqlLocalDB.msi"
                    Invoke-WebRequest "https://download.microsoft.com/download/7/c/1/7c14e92e-bdcb-4f89-b7cf-93543e7112d1/SqlLocalDB.msi" -OutFile ${'$'}installerPath
                    
                    # Install SqlLocalDB
                    Start-Process -FilePath msiexec -ArgumentList "/i `"${'$'}installerPath`" /qn IACCEPTSQLLOCALDBLICENSETERMS=YES" -Wait
                    
                    # Remove the installer file
                    Remove-Item ${'$'}installerPath
                    
                    # Reload env vars
                    ${'$'}env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
                    
                    Write-Host "Starting LocalDB"
                    sqllocaldb start mssqllocaldb
                """.trimIndent()
            }
        }
        powerShell {
            name = "Build.ps1"
            scriptMode = script {
                content = """
                    dotnet tool install Octopus.DotNet.Cli --global
                    
                    # Reload env vars
                    ${'$'}env:Path = [System.Environment]::GetEnvironmentVariable("Path","Machine") + ";" + [System.Environment]::GetEnvironmentVariable("Path","User")
                    
                    # Run build script
                    . .\build.ps1 ; CIBuild
                    
                    
                    dotnet tool install --global dotnet-reportgenerator-globaltool
                    
                    ${'$'}coverageFile = "build\test\**\In\**\coverage.cobertura.xml"
                    ${'$'}outputDir = "build\reports"
                    reportgenerator "-reports:${'$'}coverageFile" "-targetdir:${'$'}outputDir"
                """.trimIndent()
            }
        }
        nuGetPublish {
            name = "Publish Packages"
            toolPath = "%teamcity.tool.NuGet.CommandLine.6.1.0%"
            packages = "**/*.nupkg"
            serverUrl = "%teamcity.nuget.feed.httpAuth.OnionArchitectureDotnet7ContainerApps.Onion_Architecture_Container_Apps.v3%"
            apiKey = "%teamcity.nuget.feed.api.key%"
        }
    }

    features {
        perfmon {
        }
        xmlReport {
            reportType = XmlReport.XmlReportType.TRX
            rules = "build/test/**/*.trx"
        }
    }

    requirements {
        matches("teamcity.agent.jvm.os.family", "Windows")
    }
})

object Prod : BuildType({
    name = "Prod"

    enablePersonalBuilds = false
    type = BuildTypeSettings.Type.DEPLOYMENT
    buildNumberPattern = "${IntegrationBuild.depParamRefs.buildNumber}"
    maxRunningBuilds = 1

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        step {
            name = "Deploy To Prod"
            type = "octopus.deploy.release"
            param("octopus_space_name", "%OctoSpaceName%")
            param("octopus_waitfordeployments", "true")
            param("octopus_version", "3.0+")
            param("octopus_host", "%OctoURL%")
            param("octopus_project_name", "%OctoProject%")
            param("octopus_deploymenttimeout", "00:30:00")
            param("octopus_deployto", "Prod")
            param("secure:octopus_apikey", "%OctoApiKey%")
            param("octopus_releasenumber", "%build.number%")
        }
    }

    features {
        perfmon {
        }
        approval {
            approvalRules = "group:ALL_USERS_GROUP:1"
        }
    }

    dependencies {
        snapshot(Uat) {
        }
    }
})

object Tdd : BuildType({
    name = "TDD"

    enablePersonalBuilds = false
    type = BuildTypeSettings.Type.DEPLOYMENT
    buildNumberPattern = "${IntegrationBuild.depParamRefs.buildNumber}"
    maxRunningBuilds = 1

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        step {
            name = "Create and Deploy Release"
            type = "octopus.create.release"
            param("secure:octopus_apikey", "%OctoApiKey%")
            param("octopus_releasenumber", "%build.number%")
            param("octopus_additionalcommandlinearguments", "--variable=ResourceGroupName:%TDD-Resource-Group%-%build.number% --variable=container_app_name:%TDD-App-Name%")
            param("octopus_space_name", "%OctoSpaceName%")
            param("octopus_waitfordeployments", "true")
            param("octopus_version", "3.0+")
            param("octopus_host", "%OctoURL%")
            param("octopus_project_name", "%OctoProject%")
            param("octopus_deploymenttimeout", "00:30:00")
            param("octopus_deployto", "TDD")
            param("octopus_git_ref", "%teamcity.build.branch%")
        }
        powerShell {
            name = "Get Container App URL"
            scriptMode = script {
                content = """
                    Invoke-WebRequest -Uri https://aka.ms/installazurecliwindows -OutFile .\AzureCLI.msi
                    Start-Process msiexec.exe -Wait -ArgumentList '/I AzureCLI.msi /quiet'
                    Remove-Item .\AzureCLI.msi
                    ${'$'}env:PATH += ";C:\Program Files (x86)\Microsoft SDKs\Azure\CLI2\wbin"
                    
                    
                    az config set extension.use_dynamic_install=yes_without_prompt
                    # Log in to Azure
                    az login --service-principal --username %AzAppId% --password %AzPassword% --tenant %AzTenant%
                    ${'$'}containerAppURL = az containerapp show --resource-group %TDD-Resource-Group%-%build.number% --name %TDD-App-Name% --query properties.configuration.ingress.fqdn
                    ${'$'}containerAppURL = ${'$'}containerAppURL -replace '"', ''
                    Write-Host "url retrieved from AZ: ${'$'}containerAppURL"
                    [System.Environment]::SetEnvironmentVariable("containerAppURL", ${'$'}containerAppURL, "User")
                    Write-Host "ContainerAppURL after retrieval: ${'$'}env:containerAppURL"
                """.trimIndent()
            }
        }
        powerShell {
            name = "Download Acceptance Test Package"
            scriptMode = script {
                content = """
                    ${'$'}nupkgPath = "build/ChurchBulletin.AcceptanceTests.%build.number%.nupkg"
                    ${'$'}destinationPath = "."
                    
                    Add-Type -AssemblyName System.IO.Compression.FileSystem
                    
                    [System.IO.Compression.ZipFile]::ExtractToDirectory(${'$'}nupkgPath, ${'$'}destinationPath)
                    ${'$'}currentPath = (Get-Location).Path
                    # Set the download URL for the Chrome driver
                    ${'$'}chromeDriverUrl = "http://chromedriver.storage.googleapis.com/114.0.5735.90/chromedriver_win32.zip"
                    ${'$'}chromeDriverPath = "./chromedriver.zip"
                    
                    # Download the Chrome driver
                    Invoke-WebRequest -Uri ${'$'}chromeDriverUrl -OutFile ${'$'}chromeDriverPath
                    
                    ${'$'}chromedriverdestinationPath = "C:\SeleniumWebDrivers\ChromeDriver"
                    
                    Expand-Archive -Path ${'$'}chromeDriverPath -DestinationPath ${'$'}chromedriverdestinationPath
                    
                    # Add the Chrome driver to the PATH environment variable
                    ${'$'}env:PATH += ";${'$'}chromedriverdestinationPath"
                    
                    ${'$'}LocalTempDir = ${'$'}env:TEMP; 
                    ${'$'}ChromeInstaller = "ChromeInstaller.exe"; 
                    ${'$'}ChromeInstallerFile = "${'$'}LocalTempDir\${'$'}ChromeInstaller"; 
                    ${'$'}WebClient = New-Object System.Net.WebClient; 
                    ${'$'}WebClient.DownloadFile("https://download.filepuma.com/files/web-browsers/google-chrome-64bit-/Google_Chrome_(64bit)_v114.0.5735.199.exe", ${'$'}ChromeInstallerFile); 
                    Start-Process -FilePath ${'$'}ChromeInstallerFile -Args "/silent /install" -Verb RunAs -Wait; 
                    Remove-Item ${'$'}ChromeInstallerFile
                """.trimIndent()
            }
        }
        dotnetVsTest {
            name = "Run Acceptance Tests"
            assemblies = "*AcceptanceTests.dll"
            version = DotnetVsTestStep.VSTestVersion.CrossPlatform
            platform = DotnetVsTestStep.Platform.Auto
            param("dotNetCoverage.dotCover.home.path", "%teamcity.tool.JetBrains.dotCover.CommandLineTools.DEFAULT%")
        }
    }

    features {
        perfmon {
        }
    }

    dependencies {
        snapshot(Build) {
        }
        artifacts(IntegrationBuild) {
            artifactRules = "+:**=>build"
        }
    }

    requirements {
        matches("teamcity.agent.jvm.os.family", "Windows")
    }
})

object Uat : BuildType({
    name = "UAT"

    enablePersonalBuilds = false
    type = BuildTypeSettings.Type.DEPLOYMENT
    buildNumberPattern = "${IntegrationBuild.depParamRefs.buildNumber}"
    maxRunningBuilds = 1

    vcs {
        root(DslContext.settingsRoot)
    }

    steps {
        step {
            name = "Deploy To UAT"
            type = "octopus.deploy.release"
            param("octopus_space_name", "%OctoSpaceName%")
            param("octopus_waitfordeployments", "true")
            param("octopus_version", "3.0+")
            param("octopus_host", "%OctoURL%")
            param("octopus_project_name", "%OctoProject%")
            param("octopus_deploymenttimeout", "00:30:00")
            param("octopus_deployto", "UAT")
            param("secure:octopus_apikey", "%OctoApiKey%")
            param("octopus_releasenumber", "%build.number%")
        }
    }

    features {
        perfmon {
        }
        approval {
            approvalRules = "group:ALL_USERS_GROUP:1"
        }
    }

    dependencies {
        snapshot(Tdd) {
        }
    }
})
