using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("messenger_api", "API for The Messenger"),
        };

        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "messenger_api_client",
                ClientName = "Messenger API Client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("long_long_messenger_api_client_secret".Sha256()),
                },
                AllowedScopes = { "messenger_api" },
            }
        };
    }
}
