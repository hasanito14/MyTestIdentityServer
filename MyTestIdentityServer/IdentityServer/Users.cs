using IdentityServer3.Core;
using IdentityServer3.Core.Services.InMemory;
using System.Collections.Generic;
using System.Security.Claims;

namespace MyTestIdentityServer.IdentityServer
{
    public static class Users
    {
        public static List<InMemoryUser> Get()
        {
            return new List<InMemoryUser>
            {
                new InMemoryUser
                {
                    Username = "TestUser",
                    Password = "ChangeMe1!",
                    Subject = "1",

                    Claims = new[]
                    {
                        new Claim(Constants.ClaimTypes.GivenName, "Test"),
                        new Claim(Constants.ClaimTypes.FamilyName, "User"),
                        new Claim(Constants.ClaimTypes.Role, "Test1"),
                        new Claim(Constants.ClaimTypes.Role, "Test2")
                    }
                }
            };
        }
    }
}