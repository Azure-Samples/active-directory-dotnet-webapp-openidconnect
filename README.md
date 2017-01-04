---
services: active-directory
platforms: dotnet
author: dstrockis
---

# Integrate Azure AD into a web application using OpenID Connect

This sample shows how to build a .Net MVC web application that uses OpenID Connect to sign-in users from a single Azure Active Directory tenant, using the ASP.Net OpenID Connect OWIN middleware.

For more information about how the protocols work in this scenario and other scenarios, see [Authentication Scenarios for Azure AD](http://go.microsoft.com/fwlink/?LinkId=394414).

> Looking for previous versions of this code sample? Check out the tags on the [releases](../../releases) GitHub page.

## How To Run This Sample

Getting started is simple!  To run this sample you will need:
- Visual Studio 2013
- An Internet connection
- An Azure Active Directory (Azure AD) tenant. For more information on how to get an Azure AD tenant, please see [How to get an Azure AD tenant](https://azure.microsoft.com/en-us/documentation/articles/active-directory-howto-tenant/) 
- A user account in your Azure AD tenant. This sample will not work with a Microsoft account, so if you signed in to the Azure portal with a Microsoft account and have never created a user account in your directory before, you need to do that now.

### Step 1:  Clone or download this repository

From your shell or command line:

`git clone https://github.com/Azure-Samples/active-directory-dotnet-webapp-openidconnect.git`

### Step 2:  Register the sample with your Azure Active Directory tenant

1. Sign in to the [Azure portal](https://portal.azure.com).
2. On the top bar, click on your account and under the **Directory** list, choose the Active Directory tenant where you wish to register your application.
3. Click on **More Services** in the left hand nav, and choose **Azure Active Directory**.
4. Click on **App registrations** and choose **Add**.
5. Enter a friendly name for the application, for example 'WebApp-OpenIDConnect-DotNet' and select 'Web Application and/or Web API' as the Application Type. For the sign-on URL, enter the base URL for the sample, which is by default `https://localhost:44320/`. For the App ID URI, enter `https://<your_tenant_name>/WebApp-OpenIDConnect-DotNet`, replacing `<your_tenant_name>` with the name of your Azure AD tenant. Click on **Create** to create the application.
6. While still in the Azure portal, choose your application, click on **Settings** and choose **Properties**.
7. Find the Application ID value and copy it to the clipboard.
8. In the same page, change the `logoutUrl` property to `https://localhost:44320/Account/EndSession`.  This is the default single sign out URL for this sample.

### Step 3:  Configure the sample to use your Azure Active Directory tenant

1. Open the solution in Visual Studio 2013.
2. Open the `web.config` file.
3. Find the app key `ida:Tenant` and replace the value with your AAD tenant name.
4. Find the app key `ida:ClientId` and replace the value with the Application ID from the Azure portal.
5. If you changed the base URL of the sample, find the app key `ida:PostLogoutRedirectUri` and replace the value with the new base URL of the sample.

### Step 4:  Run the sample

Clean the solution, rebuild the solution, and run it.

Click the sign-in link on the homepage of the application to sign-in.  On the Azure AD sign-in page, enter the name and password of a user account that is in your Azure AD tenant.

## About The Code

This sample shows how to use the OpenID Connect ASP.Net OWIN middleware to sign-in users from a single Azure AD tenant.  The middleware is initialized in the `Startup.Auth.cs` file, by passing it the Client ID of the application and the URL of the Azure AD tenant where the application is registered.  The middleware then takes care of:
- Downloading the Azure AD metadata, finding the signing keys, and finding the issuer name for the tenant.
- Processing OpenID Connect sign-in responses by validating the signature and issuer in an incoming JWT, extracting the user's claims, and putting them on ClaimsPrincipal.Current.
- Integrating with the session cookie ASP.Net OWIN middleware to establish a session for the user. 

You can trigger the middleware to send an OpenID Connect sign-in request by decorating a class or method with the `[Authorize]` attribute, or by issuing a challenge,
```C#
HttpContext.GetOwinContext().Authentication.Challenge(
	new AuthenticationProperties { RedirectUri = "/" },
	OpenIdConnectAuthenticationDefaults.AuthenticationType);
```
Similarly you can send a signout request,
```C#
HttpContext.GetOwinContext().Authentication.SignOut(
	OpenIdConnectAuthenticationDefaults.AuthenticationType,
	CookieAuthenticationDefaults.AuthenticationType);
```
When a user is signed out, they will be redirected to the `Post_Logout_Redirect_Uri` specified when the OpenID Connect middleware is initialized.

The OpenID Connect & Cookie OWIN middleware in this project is created as a part of the open source [Katana project](http://katanaproject.codeplex.com).  You can read more about OWIN [here](http://owin.org).

## How To Recreate This Sample

1. In Visual Studio 2013, create a new ASP.Net MVC web application with Authentication set to No Authentication.
2. Set SSL Enabled to be True.  Note the SSL URL.
3. In the project properties, Web properties, set the Project Url to be the SSL URL.
4. Add the following ASP.Net OWIN middleware NuGets: Microsoft.IdentityModel.Protocol.Extensions, System.IdentityModel.Tokens.Jwt, Microsoft.Owin.Security.OpenIdConnect, Microsoft.Owin.Security.Cookies, Microsoft.Owin.Host.SystemWeb.
5. In the `App_Start` folder, create a class `Startup.Auth.cs`.  You will need to remove `.App_Start` from the namespace name.  Replace the code for the `Startup` class with the code from the same file of the sample app.  Be sure to take the whole class definition!  The definition changes from `public class Startup` to `public partial class Startup`.
6. In `Startup.Auth.cs` resolve missing references by adding `using` statements for `Owin`, `Microsoft.Owin.Security`, `Microsoft.Owin.Security.Cookies`, `Microsoft.Owin.Security.OpenIdConnect`, `System.Configuration`, and `System.Globalization`.
7. Right-click on the project, select Add, select "OWIN Startup class", and name the class "Startup".  If "OWIN Startup Class" doesn't appear in the menu, instead select "Class", and in the search box enter "OWIN".  "OWIN Startup class" will appear as a selection; select it, and name the class `Startup.cs`.
8. In `Startup.cs`, replace the code for the `Startup` class with the code from the same file of the sample app.  Again, note the definition changes from `public class Startup` to `public partial class Startup`.
9. In the `Views` --> `Shared` folder, create a new partial view `_LoginPartial.cshtml`.  Replace the contents of the file with the contents of the file of same name from the sample.
10. In the `Views` --> `Shared` folder, replace the contents of `_Layout.cshtml` with the contents of the file of same name from the sample.  Effectively, all this will do is add a single line, `@Html.Partial("_LoginPartial")`, that lights up the previously added `_LoginPartial` view.
11. Create a new empty controller called `AccountController`.  Replace the implementation with the contents of the file of same name from the sample.
12. If you want the user to be required to sign-in before they can see any page of the app, then in the `HomeController`, decorate the `HomeController` class with the `[Authorize]` attribute.  If you leave this out, the user will be able to see the home page of the app without having to sign-in first, and can click the sign-in link on that page to get signed in.
13. Almost done!  Follow the steps in "Running This Sample" to register the application in your AAD tenant.
14. In `web.config`, in `<appSettings>`, create keys for `ida:ClientId`, `ida:AADInstance`, `ida:Tenant`, and `ida:PostLogoutRedirectUri` and set the values accordingly.  For the public Azure AD, the value of `ida:AADInstance` is `https://login.windows.net/{0}`.
