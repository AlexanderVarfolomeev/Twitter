using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;

namespace Twitter.DuendeServer.Configuration;

public static class DuendeConfig
{
    public static IEnumerable<ApiScope> Scopes = new[]
    {
        new ApiScope("twitter_api", "twitter api")
    };

    public static IEnumerable<IdentityResource> Resources = new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
    };

    public static IEnumerable<Client> Clients = new[]
    {
        new Client()
        {
            ClientId = "swagger",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes = {"twitter_api"}
        },
        new Client()
        {
            ClientId = "frontend",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowAccessTokensViaBrowser =true,
            AllowedGrantTypes = GrantTypes.Code,
            
            AllowOfflineAccess = true,
            AccessTokenType = AccessTokenType.Jwt,
            
            AllowedScopes = new List<string>()
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "twitter_api"
            }
        }
    };

    public static List<TestUser> Users = new List<TestUser>()
    {
        new TestUser()
        {
            Username = "Alise",
            Password = "Alice",
            SubjectId = "1",
            Claims =  {
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
            }
        }
    };


}