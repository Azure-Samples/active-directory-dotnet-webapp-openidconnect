using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;

namespace WebApp_OpenIDConnect_DotNet
{
    public partial class Startup
    {
        //
        // The Client ID is used by the application to uniquely identify itself to Azure AD.
        // The Metadata Address is used by the application to retrieve the signing keys used by Azure AD.
        // The Authority is the name of the Azure AD tenant in which this application is registered.
        // The Post Logout Redirect Uri is the URL where the user will be redirected after they sign out.
        //
        private const string clientId = "0e7b8474-90bf-41a5-a334-925723cff119";
        private const string metadataAddress = "https://login.windows.net/{0}/.well-known/openid-configuration";
        private const string authority = "skwantoso.com";
        private const string postLogoutRedirectUri = "https://localhost:44320/";

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    Client_Id = clientId,
                    MetadataAddress = String.Format(metadataAddress, authority),
                    Post_Logout_Redirect_Uri = postLogoutRedirectUri
                });
        }
    }
}