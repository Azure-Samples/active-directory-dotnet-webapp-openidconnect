---
services: active-directory
platforms: dotnet
author: jmprieur
level: 200
client: ASP.NET 4.x Web App
service: 
endpoint: AAD V1
---
# Integrate Azure AD into a web application using OpenID Connect

![Build badge](https://identitydivision.visualstudio.com/_apis/public/build/definitions/a7934fdd-dcde-4492-a406-7fad6ac00e17/534/badge)
## About this sample

### Overview

This sample shows how to build a .Net MVC web application that uses OpenID Connect to sign in users from a single Azure Active Directory tenant, using the ASP.Net OpenID Connect OWIN middleware.

For more information about how the protocols work in this scenario and other scenarios, see [Authentication Scenarios for Azure AD](http://go.microsoft.com/fwlink/?LinkId=394414).

![Overview](./ReadmeFiles/topology.png)

### Scenario

Run the application, and press the **sign-in** button. you are signed-in, and have to consent for the application to access your profile.
When you have signed-in, the **Sign-out** button appears, which you can press to sign-out from the application

## How To Run This Sample

>[!Note] If you want to run this sample on **Azure Government**, navigate to the "Azure Government Deviations" section at the bottom of this page.

To run this sample, you'll need:

- [Visual Studio 2017](https://aka.ms/vsdownload)
- An Internet connection
- An Azure Active Directory (Azure AD) tenant. For more information on how to get an Azure AD tenant, see [How to get an Azure AD tenant](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/)
- A user account in your Azure AD tenant. This sample will not work with a Microsoft account (formerly Windows Live account). Therefore, if you signed in to the [Azure portal](https://portal.azure.com) with a Microsoft account and have never created a user account in your directory before, you need to do that now.

### Step 1:  Clone or download this repository

From your shell or command line:

`git clone https://github.com/Azure-Samples/active-directory-dotnet-webapp-openidconnect.git`
> Given that the name of the sample is pretty long, and so are the name of the referenced NuGet pacakges, you might want to clone it in a folder close to the root of your hard drive, to avoid file size limitations on Windows.

### Step 2:  Register the sample with your Azure Active Directory tenant

To register the project, you can:

- either follow the steps in the paragraphs below ([Step 2](#step-2--register-the-sample-with-your-azure-active-directory-tenant) and [Step 3](#step-3--configure-the-sample-to-use-your-azure-ad-tenant))
- or use PowerShell scripts that:
  - **automatically** create for you the Azure AD applications and related objects (passwords, permissions, dependencies)
  - modify the Visual Studio projects' configuration files.

If you want to use this automation, read the instructions in [App Creation Scripts](./AppCreationScripts/AppCreationScripts.md)

#### First step: choose the Azure AD tenant where you want to create your applications

As a first step you'll need to:

1. Sign in to the [Azure portal](https://portal.azure.com).
2. On the top bar, click on your account and under the **Directory** list, choose the Active Directory tenant where you wish to register your application.
3. Click on **More Services** in the left hand nav, and choose **Azure Active Directory**.
4. Click on **App registrations** and choose **Add**.
5. Enter a friendly name for the application, for example 'WebApp-OpenIDConnect-DotNet' and select 'Web Application and/or Web API' as the Application Type. For the sign-on URL, enter the base URL for the sample - i.e 'https://myappname.azurewebsites.net/' Click on **Create** to create the application.
6. While still in the Azure portal, make sure you select the display setting **All apps** (not "My apps") to be able to see the application you've just created. Choose your application, click on **Settings** and choose **Properties**.
7. Find the Application ID value and copy it to the clipboard.
8. In the same page, change the `logoutUrl` property to `https://yourappname.azurewebsites.net/Account/EndSession`.  This is the default single sign out URL for this sample.
9. For the App ID URI, enter `https://<your_tenant_name>/WebApp-OpenIDConnect-DotNet`, replacing `<your_tenant_name>` with the name of your Azure AD tenant. 

### Step 3:  Configure the sample to use your Azure Active Directory tenant

1. Open the solution in Visual Studio 2013.
2. Open the `web.config` file.
3. Find the app key `ida:Tenant` and replace the value with your AAD tenant name.
4. Find the app key `ida:ClientId` and replace the value with the Application ID from the Azure portal.
5. If you changed the base URL of the sample, find the app key `ida:PostLogoutRedirectUri` and replace the value with the new base URL of the sample.
=======
1. On the top bar, click on your account and under the **Directory** list, choose the Active Directory tenant where you wish to register your application.
1. Click on **All services** in the left-hand nav, and choose **Azure Active Directory**.

> In the next steps, you might need the tenant name (or directory name) or the tenant ID (or directory ID). These are presented in the **Properties**
of the Azure Active Directory window respectively as *Name* and *Directory ID*

#### Register the app (WebApp-OpenIDConnect-DotNet)

1. In the  **Azure Active Directory** pane, click on **App registrations** and choose **New application registration**.
1. Enter a friendly name for the application, for example 'WebApp-OpenIDConnect-DotNet' and select 'Web app / API' as the *Application Type*.
1. For the *sign-on URL*, enter the base URL for the sample. By default, this sample uses `https://localhost:44320/`.
1. Click **Create** to create the application.
1. In the succeeding page, Find the *Application ID* value and record it for later. You'll need it to configure the Visual Studio configuration file for this project.
1. Then click on **Settings**, and choose **Properties**.
1. For the App ID URI, replace the guid in the generated URI 'https://\<your_tenant_name\>/\<guid\>', with the name of your service, for example, 'https://\<your_tenant_name\>/WebApp-OpenIDConnect-DotNet' (replacing `<your_tenant_name>` with the name of your Azure AD tenant)
1. From the **Settings** | **Reply URLs** page for your application, update the Reply URL for the application to be `https://localhost:44320/`
1. Configure Permissions for your application. To that extent, in the Settings menu, choose the 'Required permissions' section and then,
   click on **Add**, then **Select an API**, and type `Windows Azure Active Directory` in the textbox. Then, click on  **Select Permissions** and select **User.Read**.

### Step 3:  Configure the sample to use your Azure AD tenant

In the steps below, "ClientID" is the same as "Application ID" or "AppId".

1. Open the solution in Visual Studio to configure the projects
1. Open the `WebApp-OpenIDConnect-DotNet\Web.Config` file
1. Find the app key `ida:Tenant` and replace the existing value with your Azure AD tenant name.
1. Find the app key `ida:ClientId` and replace the existing value with the application ID (clientId) of the `WebApp-OpenIDConnect-DotNet` application copied from the Azure portal.
1. If you changed the base URL of the sample, find the app key `ida:PostLogoutRedirectUri` and replace the value with the new base URL of the sample.

### Step 4:  Run the sample

Clean the solution, rebuild the solution, and run it.

Click the sign-in link on the homepage of the application to sign in.  On the Azure AD sign-in page, enter the name and password of a user account that is in your Azure AD tenant.

## About the code

This sample shows how to use the OpenID Connect ASP.Net OWIN middleware to sign in users from a single Azure AD tenant.  The middleware is initialized in the `Startup.Auth.cs` file, by passing it the Client ID of the application and the URL of the Azure AD tenant where the application is registered.  The middleware then takes care of:

- Downloading the Azure AD metadata, finding the signing keys, and finding the issuer name for the tenant.
- Processing OpenID Connect sign-in responses by validating the signature and issuer in an incoming JWT, extracting the user's claims, and putting them on ClaimsPrincipal.Current.
- Integrating with the session cookie ASP.Net OWIN middleware to establish a session for the user.

You can trigger the middleware to send an OpenID Connect sign-in request by decorating a class or method with the `[Authorize]` attribute, or by issuing a challenge,

```CSharp
HttpContext.GetOwinContext().Authentication.Challenge(
  new AuthenticationProperties { RedirectUri = "/" },
  OpenIdConnectAuthenticationDefaults.AuthenticationType);
```

Similarly you can send a sign out request,

```CSharp
HttpContext.GetOwinContext().Authentication.SignOut(
  OpenIdConnectAuthenticationDefaults.AuthenticationType,
  CookieAuthenticationDefaults.AuthenticationType);
```

When a user is signed out, they will be redirected to the `Post_Logout_Redirect_Uri` specified when the OpenID Connect middleware is initialized.

The OpenID Connect & Cookie OWIN middleware in this project is created as a part of the open-source [Katana project](http://katanaproject.codeplex.com).  You can read more about OWIN [here](http://owin.org).

## How to recreate this sample

1. In Visual Studio, create a new ASP.Net MVC web application with Authentication set to No Authentication.
2. Set SSL Enabled to be True.  Note the SSL URL.
3. In the project properties, Web properties, set the Project Url to be the SSL URL.
4. Add the following ASP.Net OWIN middleware NuGets: Microsoft.IdentityModel.Protocol.Extensions, System.IdentityModel.Tokens.Jwt, Microsoft.Owin.Security.OpenIdConnect, Microsoft.Owin.Security.Cookies, Microsoft.Owin.Host.SystemWeb.
5. In the `App_Start` folder, create a class `Startup.Auth.cs`.  You will need to remove `.App_Start` from the namespace name.  Replace the code for the `Startup` class with the code from the same file of the sample app.  Be sure to take the whole class definition!  The definition changes from `public class Startup` to `public partial class Startup`.
6. In `Startup.Auth.cs` resolve missing references by adding `using` statements for `Owin`, `Microsoft.Owin.Security`, `Microsoft.Owin.Security.Cookies`, `Microsoft.Owin.Security.OpenIdConnect`, `System.Configuration`, and `System.Globalization`.
7. Right-click on the project, select Add, select "OWIN Startup class", and name the class "Startup".  If "OWIN Startup Class" doesn't appear in the menu, instead select "Class", and in the search box enter "OWIN".  "OWIN Startup class" will appear as a selection; select it, and name the class `Startup.cs`.
8. In `Startup.cs`, replace the code for the `Startup` class with the code from the same file of the sample app.  Again, note the definition changes from `public class Startup` to `public partial class Startup`.
9. In the `Views` --> `Shared` folder, create a new partial view `_LoginPartial.cshtml`.  Replace the contents of the file with the contents of the file of the same name from the sample.
10. In the `Views` --> `Shared` folder, replace the contents of `_Layout.cshtml` with the contents of the file of the same name from the sample.  Effectively, all this will do is add a single line, `@Html.Partial("_LoginPartial")`, that lights up the previously added `_LoginPartial` view.
11. Create a new empty controller called `AccountController`.  Replace the implementation with the contents of the file of the same name from the sample.
12. If you want the user to be required to sign in before they can see any page of the app, then in the `HomeController`, decorate the `HomeController` class with the `[Authorize]` attribute.  If you leave this out, the user will be able to see the home page of the app without having to sign in first, and can click the sign-in link on that page to get signed in.
13. Almost done!  Follow the steps in "Running This Sample" to register the application in your AAD tenant.
14. In `web.config`, in `<appSettings>`, create keys for `ida:ClientId`, `ida:AADInstance`, `ida:Tenant`, and `ida:PostLogoutRedirectUri` and set the values accordingly.  For the global Azure AD, the value of `ida:AADInstance` is `https://login.microsoftonline.com/{0}`.

## Azure Government Deviations

In order to run this sample on Azure Government, you can follow through the steps above with a few variations:

- Step 2:
- You must register this sample for your AAD Tenant in Azure Government by following Step 2 above in the [Azure Government portal](https://portal.azure.us).

- Step 3:
  - Before configuring the sample, you must make sure your [Visual Studio is connected to Azure Government](https://docs.microsoft.com/azure/azure-government/documentation-government-get-started-connect-with-vs)
  - Navigate to the Web.config file. Replace the `ida:AADInstance` property in the Azure AD section with `https://login.microsoftonline.us/`.

Once those changes have been accounted for, you should be able to run this sample on Azure Government

## Deploying the sample to Azure

### Create and publish the `WebApp-OpenIDConnect-DotNet` to an Azure Web Site

1. Sign in to the [Azure portal](https://portal.azure.com).
2. Click **Create a resource** in the top left-hand corner, select **Web + Mobile** --> **Web App**, select the hosting plan and region, and give your web site a name, for example, `WebApp-OpenIDConnect-DotNet-contoso.azurewebsites.net`.  Click Create Web Site.
3. Once the web site is created, click on it to manage it.  For this set of steps, download the publish profile by clicking **Get publish profile** and save it.  Other deployment mechanisms, such as from source control, can also be used.
4. Switch to Visual Studio and go to the TodoListService project.  Right click on the project in the Solution Explorer and select **Publish**.  Click **Import Profile** on the bottom bar, and import the publish profile that you downloaded earlier.
5. Click on **Settings** and in the `Connection tab`, update the Destination URL so that it is https, for example [https://WebApp-OpenIDConnect-DotNet-contoso.azurewebsites.net](https://WebApp-OpenIDConnect-DotNet-contoso.azurewebsites.net). Click Next.
6. On the Settings tab, make sure `Enable Organizational Authentication` is NOT selected.  Click **Save**. Click on **Publish** on the main screen.
7. Visual Studio will publish the project and automatically open a browser to the URL of the project.  If you see the default web page of the project, the publication was successful.

### Update the Active Directory tenant application registration for `WebApp-OpenIDConnect-DotNet`

1. Navigate to the [Azure portal](https://portal.azure.com).
2. On the top bar, click on your account and under the **Directory** list, choose the Active Directory tenant containing the `WebApp-OpenIDConnect-DotNet` application.
3. On the applications tab, select the `WebApp-OpenIDConnect-DotNet` application.
4. From the Settings -> Reply URLs menu, update the Sign-On URL, and Reply URL fields to the address of your service, for example [https://WebApp-OpenIDConnect-DotNet-contoso.azurewebsites.net](https://WebApp-OpenIDConnect-DotNet-contoso.azurewebsites.net). Save the configuration.

## Community Help and Support

Use [Stack Overflow](https://stackoverflow.com/questions/tagged/azure-active-directory) to get support from the community.
Ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before.
Make sure that your questions or comments are tagged with [`azure-active-directory`].

If you find a bug in the sample, please raise the issue on [GitHub Issues](../../issues).

To provide a recommendation, visit the following [User Voice page](https://feedback.azure.com/forums/169401-azure-active-directory).

## Contributing

If you'd like to contribute to this sample, see [CONTRIBUTING.MD](/CONTRIBUTING.md).

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information, see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## More information

For more information about how to secure an ASP.NET web app using Azure Active Directory, please see [Developing ASP.NET Apps with Azure Active Directory](https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/developing-aspnet-apps-with-windows-azure-active-directory#create-an-aspnet-application)

If you are interested in the same, but for **.NET Core**, see [Azure Active Directory with ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/azure-active-directory/?view=aspnetcore-2.1)

For more information about how OAuth 2.0 protocols work in this scenario and other scenarios, see [Authentication Scenarios for Azure AD](http://go.microsoft.com/fwlink/?LinkId=394414).
