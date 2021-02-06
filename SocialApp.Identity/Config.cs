using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialApp.Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),

                //custom claims
                //new IdentityResource
                //{
                //    Name = "scope.api",
                //    UserClaims = {"api.role"}
                //}
            };

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource("SocialApi"),
                new ApiResource("SocialIdentityApi")
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {               
                new Client
                {
                    RequireConsent = false,
                    AllowAccessTokensViaBrowser = true,
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256())},
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes =
                    {
                        "SocialApi",
                        "SocialIdentityApi",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    RedirectUris =
                    {
                        "https://localhost:5001/signin-oidc"
                    },

                    PostLogoutRedirectUris = {""},
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AlwaysSendClientClaims = true
                }
            }; 
    }
}
