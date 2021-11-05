using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
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
            },
            new Client
            {
                ClientId = "react_client",
                ClientName = "React client",
                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,
                RequirePkce = true,

                RedirectUris = { "http://localhost:3000/signin-oidc" },
                PostLogoutRedirectUris = { "http://loclahost:3000/signout-oidc" },
                AllowedCorsOrigins = { "http://localhost:3000" },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "messenger_api",
                },
            }
        };
    }
}
