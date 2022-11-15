# Introduction

This repository provides a quick demo of App Registration (multi-tenant) leveraging scopes and app role.

The application is simple, a REST API in C# hosted in App Service saving value in an Azure Redis Cache and protected by Azure Active Directory.

# Installation

The first thing to do is to Fork this GitHub repository.  

Now, you need to have a service principal to connect to Azure and execute the GitHub workflows.

To do so, refer to this [documentation](https://github.com/marketplace/actions/azure-login#configure-a-service-principal-with-a-secret) and save the generated values.

Next, you will need to create a token that will be used to write secret in your GitHub repository.  Follow this [documentation](https://github.com/marketplace/actions/create-github-secret-action) to do so, be careful the permission will be different if you use a public or private repository.  **IMPORTANT**, the name of the token need to be **PA_DEMO_APP_REGISTRATION**.

Once is done you will need to create 3 [GitHub Actions Secrets](https://docs.github.com/en/rest/actions/secrets) in your repository.

This is the 3 secrets that you need to create.

| Secret Name | Description |
| ------------| ------------|
| AZURE_CREDENTIALS | The saved value from the service principal created before, needed to login to Azure.  Be sure the service principal have contributor on the subscription.
| AZURE_SUBSCRIPTION | The Azure subscription ID where the resource will be created
| PA_DEMO_APP_REGISTRATION | The value from the personal token created before, this is needed to write the name of the WebApp created in the workflow infra.yml

## Create the Azure Resource

Now, go to the **Actions** tab in the GitHub repository and click on the Create Azure Resources, click on the left button **Run workflow** and click on the green button.  This will create all the Azure Resources needed.

# Create App Registration in Active Directory

Now, before deploying the application, we need to create the Application in Active Directory.

## Creating the Contoso Weather Api

Now go to your Azure Active Directory and in the left blade menu click App registrations.

Click on the top button **New registration**.

From there enter those values

![alt text](https://github.com/hugogirard/demoAppRegistration/blob/main/images/Contoso%20Weather%20Api.png?raw=true)

Now, you need to define the scope of the API, click on the created App and go to **Expose an API** menu in the left blade.

The first thing to do will be to have an Application ID URI, for this click on the **Set API**  hyperlink.

![alt text](https://github.com/hugogirard/demoAppRegistration/blob/main/images/SetApiUri.png?raw=true)

Now you will create two Scopes, click the **Add a scope** button and enter those values.

![alt text](https://github.com/hugogirard/demoAppRegistration/blob/main/images/Create%20Read.City.Weather%20Scope.png?raw=true)

Once is done, create the next scope with those values

![alt text](https://github.com/hugogirard/demoAppRegistration/blob/main/images/Read.Weather.From.Mars.png?raw=true)

Now we will create two App Roles, go to the **App roles** menu in the left blade.

You need to create the first role with the name **SecretAgent**

![alt text](https://github.com/hugogirard/demoAppRegistration/blob/main/images/AppRole.png?raw=true)

Create another role with the Display name **Guest** and keep the other values the same.

## Enterprise applications

Now, you need to go to the **Enterprise applications** in the left menu blade of Azure Active Directory.

This represent the instance of your application Contoso Weather Api in your Azure Active Directory.

By default, all users in your AAD can call your application.  In this scenario we want to restrict this.  To do so, click on **Contoso Weather Api** in your **Enterprise applications**.

Now, go to the Properties tab and be sure Assignment required is set to Yes.

![alt text](https://github.com/hugogirard/demoAppRegistration/blob/main/images/AssignmentRequired.png?raw=true)

Now, you need to assign two users to the Enterprise application, one with the SecretAgent role and the other with the Guest role.

Go to the **Users and groups** menu in the left blade and Add two users.

Once is done, you should have something like this.

