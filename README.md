# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Onion Architecture .NET 7 Container Apps Getting Started
- [Github Setup:](#github-setup)
- [Azure](#azure)
  - [Create an Azure Container Registry](#create-an-azure-container-registry)
  - [Connect Azure to Octopus Deploy](#connect-azure-to-octopus-deploy)
- [Octopus Deploy Environment Setup:](#octopus-deploy-environment-setup)
- [Octopus Deploy Project Setup:](#octopus-deploy-project-setup)
  - [Connect Octopus to GitHub](#connect-octopus-to-github)
  - [Create a new Version Controlled Project:](#create-a-new-version-controlled-project)
  - [Create and Update Project Variables](#create-and-update-project-variables)
- [Azure DevOps Setup:](#azure-devops-setup)
  - [Create Service Connections](#create-service-connections)
  - [Create an artifact feed](#create-an-artifact-feed)
  - [Authorize the Pipeline to push packages to the feed](#authorize-the-pipeline-to-push-packages-to-the-feed)
  - [Add the Azure DevOps Feed to Octopus](#add-the-azure-devops-feed-to-octopus)
  - [Create the Library Variable Group](#create-the-library-variable-group)
  - [Grant the pipeline access to the variable group](#grant-the-pipeline-access-to-the-variable-group)
  - [Create a Pipeline](#create-a-pipeline)

Requirements:

- Octopus Deploy
- Azure
- Azure DevOps
- Github

# Github:

Fork the [onion-architecture-dotnet-7-container-apps](https://github.com/ClearMeasureLabs/onion-architecture-dotnet-7-container-apps) repo

# Azure

## Create an Azure Container Registry

[https://learn.microsoft.com/en-us/azure/container-registry/container-registry-get-started-portal?tabs=azure-cli](https://learn.microsoft.com/en-us/azure/container-registry/container-registry-get-started-portal?tabs=azure-cli)

Name the resource group something identifiable and different than the application environments. onion-architecture-container-apps-acr for example.
The resource groups for the application environments will be created and destroyed programatically, the container registry should be kept separate.

When creating the container registry, a Basic SKU is sufficient. Name the container registry something identifiable. onion-architecture-container-apps for example.

## Connect Azure to Octopus Deploy

### Create an Azure App Registration for Octopus Deploy

1. In Azure AD select App Registrations

![](RackMultipart20230525-1-75kkpw_html_4fa33ca2171871f7.png)

### Select New Registration

- Name the Registration
- A Redirect URI is not needed
- Select Register

![](RackMultipart20230525-1-75kkpw_html_c49ab560a2f3e2be.png)

### Create a Client Secret

        1. Select Certificates and Secrets
        2. New Client Secret
        3. Provide a description and select add

![](RackMultipart20230525-1-75kkpw_html_bdbd1dd158d12fcf.png)

1. Save the client secret Value. It will be used in Octopus.

### Set Octopus Deploy as an Azure Contributor

1. In your Azure subscription, navigate to Access Control (IAM), and add a role assignment
2. Select Privileged administrator roles, then Contributor

![](RackMultipart20230525-1-75kkpw_html_153f64007edf41d3.png)

1. In the Members tab use the + Select Members page to select the App Registration that was created
2. Press Review + assign

![](RackMultipart20230525-1-75kkpw_html_e9dfe4c8cfa6d51c.png)

1. Press Review + assign again to save

### Create an Azure Account in Octopus Deploy

1. In Octopus Deploy navigate to Infrastructure -\> Accounts
2. Add an Azure Subscription Account

        1. Name the account Azure-Onion-Containers
        2. Fill in the Subscription ID
          i. This can be found in the Subscription Overview page in the Azure web portal
        3. Leave the Authentication Method as 'Use a Service Principal'
        4. Fill in the Tenant ID
          i. This can be found in the Overview page of the App Registration in the Azure web portal
        5. Fill in the Application ID
          i. This is the Application (client) ID from the App registration that was just created. This can be found in the Overview page in the Azure web portal
        6. Fill in the Application Password / Key
          i. This is the client secret value that was created previously

![](RackMultipart20230525-1-75kkpw_html_66233ef27e72cde9.png)

# Octopus Deploy Environment Setup:

In Octopus Deploy create 3 environments.

- TDD
- UAT
- Prod

No Deployment targets need to be created.

Create a Lifecycle that uses those three environments promoting from TDD -\> UAT -\> Prod

# Octopus Deploy Project Setup:

## Connect Octopus to GitHub

### In GitHub

Create a Personal Access Token (with repo access only.

Save the token for use in Octopus

### In Octopus

Create Git Credentials using the GitHub Personal Access Token

## Create a new Version Controlled Project:

1. Create a new project, ensure the "Use version control for this project" box is checked
2. Use the Lifecycle that was just created

![](RackMultipart20230525-1-75kkpw_html_61a2b9f506dd0082.png)

1. Click Save AND CONFIGURE VCS

      1. Skip the "How do you intend to use this project" popup
2. Set the Git Repository URL to the URL of the forked repo
3. Use the Library Git Credentials that were created earlier
4. Change the default branch to 'master'

![](RackMultipart20230525-1-75kkpw_html_f03267bb7448293d.png)

1. Click CONFIGURE and push the initial commit to convert the project

## Create and Update Project Variables

In the Octopus Project navigate to Variables -\> Project

- Create a variable named **DatabasePassword** Set the values to Sensitive and enter passwords for TDD, UAT, and Prod environments
- Update **registry\_login\_server** to the login server of the Azure Container Registry that was created
  - This can be found in the Overview page of the container registry in the Azure Web Portal
- Update **EnsureEnvironmentsExist** to True for Prod/UAT to ensure that all resources will be created the first time.

# Azure DevOps Setup:

Create a new project

Install the Octopus Deploy Integration ([https://marketplace.visualstudio.com/items?itemName=octopusdeploy.octopus-deploy-build-release-tasks](https://marketplace.visualstudio.com/items?itemName=octopusdeploy.octopus-deploy-build-release-tasks))

## Create Service Connections

To create a service connection

        1. Go to Project Settings in the bottom left
        2. Under the Pipelines heading, select Service Connections
        3. Select Create Service Connection

1. Create an Azure Resource Manager Service Connection

    1. Select Azure Resource Manager as the new service connection type

![](RackMultipart20230525-1-75kkpw_html_44255d02a8569e61.png)

    1. Use the recommended authentication method (Service Principal (automatic))
    2. Select your Azure Subscriptoin
    3. Leave the Resource Group section blank
    4. Name the Service Connection: dotnet-7-containerapp
    5. Check 'Grant access permission to all pipelines'

![](RackMultipart20230525-1-75kkpw_html_a359f2ee0fddb7a6.png)

    1. Save the service connection

1. Create an Octopus Deploy Service connection

    1. In Octopus Deploy create an API key ([https://octopus.com/docs/octopus-rest-api/how-to-create-an-api-key](https://octopus.com/docs/octopus-rest-api/how-to-create-an-api-key))
    2. In Azure DevOps select New Service Connection, choose Octopus Deploy as the type

![](RackMultipart20230525-1-75kkpw_html_9ad97c8353747449.png)

    1. Fill in the URL of your Octopus instance, the API key that was created, and name the service connection: OctoServiceConnection
    2. Check 'Grant access permission to all pipelines'

![](RackMultipart20230525-1-75kkpw_html_7f880a8afd52e5cc.png)

    1. Save the service connection

1. Create an Azure Container Registry Service Connection

    1. Select New Service Connection, choose Docker Registry as the type

![](RackMultipart20230525-1-75kkpw_html_d60a98d1f8592edc.png)

    1. Configure the registry
      1. Choose Azure Container Registy as the type
      2. Choose Service Principal as the Authentication Type
      3. Select your Azure Subscription
      4. Select the container registry that was created
      5. Name the service connection: OnionArchACRServiceConnection
      6. Select 'Grant access permission to all pipelines'

![](RackMultipart20230525-1-75kkpw_html_d2507be4fc1629c9.png)

    1. Save the Service Connection

## Create an artifact feed

1. In the Azure DevOps project: Go to Artifacts, then select **+ Create Feed**

![](RackMultipart20230525-1-75kkpw_html_f4cde8d4ab2680d7.png)

1. Name the feed something relevant, scope it to the current project, select create

![](RackMultipart20230525-1-75kkpw_html_9b152b2f3b6c09f0.png)

## Authorize the Pipeline to push packages to the feed

1. Set the Project Build Service ** ** identity to be a  **Contributor**  on your feed ([https://learn.microsoft.com/en-us/azure/devops/artifacts/feeds/feed-permissions?view=azure-devops#configure-feed-settings](https://learn.microsoft.com/en-us/azure/devops/artifacts/feeds/feed-permissions?view=azure-devops%23configure-feed-settings))

![](RackMultipart20230525-1-75kkpw_html_733945d8db000ad7.png)

## Add the Azure DevOps Feed to Octopus

### Create a Personal Access Token

1. Create an Azure DevOps Personal Access Token [https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=Windows](https://learn.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=Windows)
2. Give the token the Packaging Read scope
3. Save the token for use in Octopus

### In Azure DevOps

1. Navigate to the Artifacts page.
2. Select the 'Connect to Feed' button
3. Select Nuget.exe as the feed type
4. In the Project Setup section, copy the URL from the value field

![](RackMultipart20230525-1-75kkpw_html_46956bee13f928dd.png)

### In Octopus Deploy

1. Navigate to Library -\> External Feeds and select ADD FEED
2. Set the Feed type to NuGet Feed
3. Name the feed Onion-Arch-DotNet-7
4. Paste in the URL that was copied from Azure DevOps
5. Provide something in the Feed username field. It can be anything other than an empty string. It's not actually used.
6. Provide the personal access token from Azure DevOps as the Feed Password

![](RackMultipart20230525-1-75kkpw_html_e7e48d369a59dbce.png)

## Create the Library Variable Group

1. In the Azure DevOps project: Go to Pipelines -\> Library

![](RackMultipart20230525-1-75kkpw_html_8ea8a2ff18aeccf1.png)

1. Create a variable group named **Integration-Build**

    1. Create a variable called **FeedName**. The value will be \<Azure DevOps project name\>/\<Azure DevOps feed name\>
    2. Create a variable called **OctoProjectGroup** with the value being the Project Group that houses your Octopus Project.
    3. Create a variable called **OctoProjectName** with the value being the name of your Octopus Project.
    4. Create a variable called **OctoSpace** with the value being the name of your Octopus Space.

![](RackMultipart20230525-1-75kkpw_html_c5a5a1cd410e039c.png)

1. Save the variable group

## Grant the pipeline access to the variable group

1. From the variable group page select the Pipeline permissions tab at the top
2. Select the hamburger menu, and select Open Access

![](RackMultipart20230525-1-75kkpw_html_41e53b35d4da5882.png)

1. Select Open access to allow all pipelines in the project to use the variable group

![](RackMultipart20230525-1-75kkpw_html_2d281f2c0ed7b7fb.png)

## Create a Pipeline

1. Go to Pipelines -\> Pipelines

![](RackMultipart20230525-1-75kkpw_html_1cccab02207c6d87.png)

1. Select Create Pipeline
2. Select Github as the location for your code
  1. Accept and allow Github and Azure DevOps to connect

![](RackMultipart20230525-1-75kkpw_html_e982720ffd029def.png)

1. Select the forked repo when asked to select a repository
  1. Select Approve & Install to allow Azure Pipelines to connect to GitHub
2. When reviewing the pipeline YAML select **Run** to create and run the Pipeline for the first time

![](RackMultipart20230525-1-75kkpw_html_638bf1850ba7cede.png)

The pipeline will build the application, create all of the resources in the TDD environment, deploy the app to TDD, test the app, then destroy the TDD resources. Then the Azure resources in UAT will be created, and the app will be deployed to TDD. Ultimately Prod resources will be created, and the app will be deployed to Prod

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)