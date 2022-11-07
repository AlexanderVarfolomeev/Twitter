using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using Shared.Security;

namespace Twitter.DuendeServer.Configuration;

public static class DuendeConfig
{
    public static IEnumerable<ApiScope> Scopes = new[]
    {
        new ApiScope(AppScopes.TwitterRead, "Access to TwitterApi - read data."),
        new ApiScope(AppScopes.TwitterWrite, "Access to TwitterApi - write data.")
    };

    public static IEnumerable<IdentityResource> Resources = new IdentityResource[]
    {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile()
    };

    public static IEnumerable<Client> Clients = new[]
    {
        new Client
        {
            ClientId = "swagger",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            AllowedScopes =
            {
                AppScopes.TwitterRead
            }
        },
        new Client
        {
            ClientId = "wpf",
            ClientSecrets = {new Secret("secret".Sha256())},
            AllowAccessTokensViaBrowser = true,
            AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

            AllowOfflineAccess = true,
            AccessTokenType = AccessTokenType.Jwt,

            AccessTokenLifetime = 3600 * 12, // 12 hours

            RefreshTokenUsage = TokenUsage.OneTimeOnly,
            RefreshTokenExpiration = TokenExpiration.Sliding,
            AbsoluteRefreshTokenLifetime = 2592000, // 30 days
            SlidingRefreshTokenLifetime = 1296000, // 15 days

            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                AppScopes.TwitterRead,
                AppScopes.TwitterWrite
            }
        }
    };

    public static List<TestUser> Users = new()
    {
        new TestUser
        {
            Username = "alice",
            Password = "alice",
            SubjectId = "1",
            Claims =
            {
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean)
            }
        }
    };
}