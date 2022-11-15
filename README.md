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

