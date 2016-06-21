using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;

namespace WebApp_OpenIDConnect_DotNet.Utils
{
    public class SingleSignOutMiddleware : OwinMiddleware
    {
        public static string DefaultCookieName = "AadSingleSignOut";
        public static string SignOutOccurred = "True";

        private SingleSignOutOptions _options;

        public SingleSignOutMiddleware(OwinMiddleware next, SingleSignOutOptions options) : base(next)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            _options = options;
            _options.CookieName = options.CookieName ?? DefaultCookieName;
        }

        public async override Task Invoke(IOwinContext context)
        {
            string cookie = context.Request.Cookies[_options.CookieName];
            if (cookie == null)
            {
                await Next.Invoke(context);
            }
            else
            {
                context.Response.Cookies.Delete(_options.CookieName);
                context.Response.Redirect(_options.SignedOutUrl);
            }
        }
    }

    public sealed class SingleSignOutOptions
    {
        public string SignedOutUrl { get; set; }
        public string CookieName { get; set; }
    }
}