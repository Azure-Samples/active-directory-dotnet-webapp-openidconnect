using System;
using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;

namespace WebApp_OpenIDConnect_DotNet.Models
{
    public class MyOpenIDConnectAuthenticationHandler : OpenIdConnectAuthenticationHandler
    {
        private const string NonceProperty = "nonceProperty";
        private const string NoncePrefix =  OpenIdConnectAuthenticationDefaults.CookiePrefix + "nonce.";

        private readonly ILogger _logger;

        public MyOpenIDConnectAuthenticationHandler(ILogger logger)
            : base(logger)
        {
            _logger = logger;
        }

        protected override void AddNonceToMessage(OpenIdConnectMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            AuthenticationProperties properties = new AuthenticationProperties();
            string nonce = Options.ProtocolValidator.GenerateNonce();
            properties.Dictionary.Add(NonceProperty, nonce);
            message.Nonce = nonce;

            //computing the hash of nonce and appending it to the cookie name
            string nonceKey = GetNonceKey(nonce);
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsSecure,
            };

            string nonceId = Convert.ToBase64String(Encoding.UTF8.GetBytes((Options.StateDataFormat.Protect(properties))));
            Response.Cookies.Append(nonceKey, nonceId, cookieOptions);
        }

        protected override string RetrieveNonce(OpenIdConnectMessage message)
        {
            if (message.IdToken == null)
            {
                return null;
            }

            JwtSecurityToken token = new JwtSecurityToken(message.IdToken);
            if (token == null)
            {
                return null;
            }

            //computing the hash of nonce and appending it to the cookie name
            string nonceKey = GetNonceKey(token.Payload.Nonce);
            string nonceCookie = Request.Cookies[nonceKey];
            if (string.IsNullOrWhiteSpace(nonceCookie))
            {
                _logger.WriteWarning("The nonce cookie was not found.");
                return null;
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsSecure
            };

            Response.Cookies.Delete(nonceKey, cookieOptions);

            string nonce = null;
            AuthenticationProperties nonceProperties = Options.StateDataFormat.Unprotect(Encoding.UTF8.GetString(Convert.FromBase64String(nonceCookie)));
            if (nonceProperties != null)
            {
                nonceProperties.Dictionary.TryGetValue(NonceProperty, out nonce);
            }
            else
            {
                _logger.WriteWarning("Failed to un-protect the nonce cookie.");
            }

            return nonce;
        }

        private string GetNonceKey(string nonce)
        {
            using(HashAlgorithm hash = SHA256.Create())
            {
                return NoncePrefix + Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(nonce)));
            }
        }
    }
}