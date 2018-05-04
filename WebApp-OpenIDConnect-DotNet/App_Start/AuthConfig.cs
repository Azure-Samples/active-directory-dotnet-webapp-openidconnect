using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApp_OpenIDConnect_DotNet
{
    public class AuthConfig
    {
        private string clientId;
        private string aadInstance;
        private string tenant;
        private string appBaseUri;

        public static class SettingName
        {
            public const string ClientId = "ida:ClientId";
            public const string AadInstance = "ida:AADInstance";
            public const string Tenant = "ida:Tenant";
            public const string AppBaseUri = "ida:AppBaseUri";
        }

        private AuthConfig(NameValueCollection appSettings)
        {
            if (appSettings == null)
            {
                throw new ArgumentNullException($"{appSettings}", "ConfigurationManager.AppSettings should be passed to AuthConfig");
            }

            clientId = appSettings[SettingName.ClientId];
            aadInstance = appSettings[SettingName.AadInstance];
            tenant = appSettings[SettingName.Tenant];
            appBaseUri = appSettings[SettingName.AppBaseUri];

            GuardAgainstBadAppSettingValues();
        }

        public static AuthSettings GetSettings(NameValueCollection appSettings)
        {
            var config = new AuthConfig(appSettings);
            return new AuthSettings(config.clientId, config.aadInstance, config.tenant, config.appBaseUri);
        }

        private void GuardAgainstBadAppSettingValues()
        {
            var messages = new StringBuilder();

            CheckClientId(messages);
            CheckAadInstance(messages);
            CheckTenant(messages);
            CheckBaseUri(messages);

            if (messages.Length > 0)
            {
                throw new InvalidOperationException(messages.ToString());
            }
        }

        private void CheckClientId(StringBuilder messages)
        {
            CheckForNullOrEmptyValue(clientId, SettingName.ClientId, messages);

            Guid clientIdValue;
            if (!Guid.TryParse(clientId, out clientIdValue))
            {
                messages.AppendLine($"{SettingName.ClientId} value is not a Guid. The value should be the Guid Id of the Application created in the Azure AD tenant.");
            }
        }

        private void CheckAadInstance(StringBuilder messages)
        {
            CheckForNullOrEmptyValue(aadInstance, SettingName.AadInstance, messages);
            CheckUri(aadInstance, SettingName.AadInstance, messages);
        }
        private void CheckTenant(StringBuilder messages)
        {
            CheckForNullOrEmptyValue(tenant, SettingName.Tenant, messages);

            if (!Regex.IsMatch(tenant, "^[^/@]*\\.onmicrosoft\\.com(/.*)?$"))
            {
                messages.AppendLine($"{SettingName.Tenant} must have the name of the tenant followed by .onmicrosoft.com, e.g. contoso.onmicrosoft.com");
            }
        }

        private void CheckBaseUri(StringBuilder messages)
        {
            CheckForNullOrEmptyValue(appBaseUri, SettingName.AppBaseUri, messages);
            CheckUri(appBaseUri, SettingName.AppBaseUri, messages);
        }

        private void CheckForNullOrEmptyValue(string value, string settingName, StringBuilder messages)
        {
            if (string.IsNullOrEmpty(value))
            {
                messages.AppendLine($"{settingName} value is empty or null.");
            }
        }
        private void CheckUri(string value, string settingName, StringBuilder messages)
        {
            Uri uri;
            if (!Uri.TryCreate(value, UriKind.Absolute, out uri))
            {
                messages.AppendLine($"{settingName} value '{value}' is not a valid Uri. A valid example for {settingName} value would be 'https://localhost/'");
            }
        }
    }

    public struct AuthSettings
    {
        public string ClientId { get; }
        public string AadInstance { get; }
        public string Tenant { get; }
        public string RedirectUri { get; }
        public string PostLogoutRedirectUri { get; }
        public string Authority { get; }

        public AuthSettings(string clientId, string aadInstance, string tenant, string baseUri)
        {
            ClientId = clientId;
            AadInstance = aadInstance;
            Tenant = tenant;
            RedirectUri = baseUri;
            PostLogoutRedirectUri = baseUri;
            Authority = GetAuthority(aadInstance, tenant);
        }

        private static string GetAuthority(string aadInstance, string tenant)
        {
            var uri = new UriBuilder(aadInstance);
            uri.Path = tenant;

            return uri.ToString();
        }
    }
}