using IdentityServer3.Core;
using IdentityServer3.Core.Configuration;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using MyTestIdentityServer.IdentityServer;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MyTestIdentityServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = Constants.ClaimTypes.Subject;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();


            app.Map("/identity", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = "Test Embedded IdentityServer",
                    SigningCertificate = LoadCertificate(),

                    Factory = new IdentityServerServiceFactory()
                    .UseInMemoryUsers(Users.Get())
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get())
                });
            });

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookies"
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://localhost:44376/identity",
                ClientId = "TClient",
                Scope = "openid profile roles",
                RedirectUri = "https://localhost:44376/",
                ResponseType = "id_token",
                UseTokenLifetime = false,
                SignInAsAuthenticationType = "Cookies",

                /*
                 * OIDC notification can be used to do claim transformation and the claims will stored in cookie
                 * 
                 * 
                */
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = n =>
                    {
                        var id = n.AuthenticationTicket.Identity;

                        //The info we want to keep
                        var givenName = id.FindFirst(Constants.ClaimTypes.GivenName);
                        var familyName = id.FindFirst(Constants.ClaimTypes.FamilyName);
                        var sub = id.FindFirst(Constants.ClaimTypes.Subject);
                        var roles = id.FindAll(Constants.ClaimTypes.Role);

                        //Create new identity
                        var nid = new ClaimsIdentity(
                            id.AuthenticationType,
                            Constants.ClaimTypes.GivenName,
                            Constants.ClaimTypes.Role);


                        nid.AddClaim(givenName);
                        nid.AddClaim(familyName);
                        nid.AddClaim(sub);
                        nid.AddClaims(roles);

                        n.AuthenticationTicket = new AuthenticationTicket(nid, n.AuthenticationTicket.Properties);

                        return Task.FromResult(0);
                    }
                }



            });
        }

        X509Certificate2 LoadCertificate()
        {
            return new X509Certificate2(
                string.Format(@"{0}\IdentityServer\idsrv3test.pfx", AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
        }


    }
}