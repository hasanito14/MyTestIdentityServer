using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityServer3.Core.Models;

namespace MyTestIdentityServer.IdentityServer
{
    public static class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    Enabled = true,
                    ClientName = "Test Client",
                    ClientId = "TClient",
                    Flow = Flows.Implicit,

                    RedirectUris = new List<string>
                    { "https://localhost:44376/" },

                    AllowAccessToAllScopes = true
                }

            };
        }
    }
}