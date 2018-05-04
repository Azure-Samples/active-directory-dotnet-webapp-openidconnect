//----------------------------------------------------------------------------------------------
//    Copyright 2014 Microsoft Corporation
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//----------------------------------------------------------------------------------------------

// The following using statements were added for this sample.
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;

using System.Threading.Tasks;

namespace WebApp_OpenIDConnect_DotNet
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            var settings = AuthConfig.GetSettings(ConfigurationManager.AppSettings);

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);
            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = settings.ClientId,
                    Authority = settings.Authority,
                    PostLogoutRedirectUri = settings.PostLogoutRedirectUri,
                    RedirectUri = settings.RedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications
                    {
                        AuthenticationFailed = context => 
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/Error?message=" + context.Exception.Message);
                            return Task.FromResult(0);
                        }
                    }
                });
        }
    }
}