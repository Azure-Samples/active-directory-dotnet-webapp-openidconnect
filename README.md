WebApp-OpenIDConnect-DotNet
===========================

This sample shows how to build a .Net MVC web application that uses OpenID Connect to sign-in users from a single Azure Active Directory tenant, using the ASP.Net OpenID Connect OWIN middleware.

## How To Run This Sample

Getting started is simple!  To run this sample you will need:
- Visual Studio 2013
- An Internet connection
- An Azure subscription

Every Azure subscription has an associated Azure Active Directory tenant.  If you don't already have an Azure subscription, you can get a free subscription by signing up at [http://wwww.windowsazure.com]{http://www.windowsazure.com}.  All of the Azure AD features used by this sample are available free of charge.

### Step 1:  Clone or download this repository

From your shell or command line:

'git clone git@github.com:WindowsAzureADSamples/WebApp-OpenIDConnect-DotNet.git'

### Step 2:  Create a user account in your Azure Active Directory tenant

If you already have a user account in your Azure Active Directory tenant, you can skip to the next step.  This sample will not work with a Microsoft account, so if you signed in to the Azure portal with a Microsoft account and have never created a user account in your directory before, you need to do that now.  If you create an account and want to use it to sign-in to the Azure portal, don't forget to add the user account as a co-administrator of your Azure subscription.

### Step 3:  Register the sample with your Azure Active Directory tenant

1. Sign in to the [Azure management porta]{https://manage.windowsazure.com}.
2. Click on Active Directory in the left hand nav.
3. Click the directory tenant where you wish to register the sample application.
4. Click the Applications tab.
5. In the drawer, click Add.
6. Click "Add an application my organization is developing".
7. Enter a friendly name for the application, select "Web Application and/or Web API", and click next.
8. For the sign-on URL, enter 'https://localhost:XXXX'.
9. For the App ID URI, enter 'https://<your_tenant_name>/WebApp-OpenIDConnect-DotNet'

All done!  Before moving on to the next step, you need to find the Client ID of your application.

1. Still in the Azure portal, click the Configure tab of your application.
2. Find the Client ID value and copy it to the clipboard.

### Step 4:  Configure the sample to use your Azure Active Directory tenant

1. Open the solution in Visual Studio 2013.
2. Open the 'web.config' file.
3. Find the app key called 'ida:Authority' and replace the value with your Azure AD tenant name.
4. Find the app key called 'ida:ClientId' and replace the value with the Client ID from the Azure portal.

### Step 5:  Run the sample

You know what to do!  Upon running the sample you will immediately be prompted to sign in.  Enter the name and password of a user account that is in your Azure AD tenant.

## About The Code

## How This Sample Was Created
