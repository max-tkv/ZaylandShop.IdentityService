using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace ZaylandShop.IdentityService.Web.Configuration;

public static class IdentityServiceConfiguration
{
    static string[] allowedScopes =
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        IdentityServerConstants.StandardScopes.Email,
        "ZaylandShopWebAPI"
    };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("ZaylandShopWebAPI", "Zayland Shop Web API"),
            new("email")
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("ZaylandShopWebAPI", "Zayland Shop Web API", new [] { JwtClaimTypes.Name })
            {
                Scopes = { "ZaylandShopWebAPI" }
            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "zayland-shop-web-api",
                ClientName = "Zayland Shop Web",
                ClientUri = "https://localhost:5001", // todo

                AllowedGrantTypes = GrantTypes.Code,
                RequireClientSecret = false,

                RedirectUris =
                {
                    "https://localhost:5003" // todo
                },
                AllowedCorsOrigins =
                {
                    "https://localhost:5003" // todo
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:5003" // todo
                },
                AllowedScopes = allowedScopes
            }
        };
}