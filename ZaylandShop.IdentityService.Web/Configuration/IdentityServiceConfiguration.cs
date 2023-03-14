using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Newtonsoft.Json;

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
            new ApiScope("myapi.read", "Read access to My API"),
            new ApiScope("myapi.write", "Write access to My API")
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
            new ApiResource("myapi", "My API")
            {
                Scopes = { "myapi.read", "myapi.write" }
            }
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "myclient",
                ClientSecrets = { new Secret("myclientsecret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "myapi.read", "myapi.write" }
            }
            // new Client
            // {
            //     ClientId = "zayland-shop-web-api",
            //     ClientName = "Zayland Shop Web",
            //     ClientUri = "https://localhost:5001", // todo
            //
            //     AllowedGrantTypes = GrantTypes.Code,
            //     RequireClientSecret = false,
            //
            //     RedirectUris =
            //     {
            //         "https://localhost:5003" // todo
            //     },
            //     AllowedCorsOrigins =
            //     {
            //         "https://localhost:5003" // todo
            //     },
            //     PostLogoutRedirectUris =
            //     {
            //         "https://localhost:5003" // todo
            //     },
            //     AllowedScopes = allowedScopes
            // }
        };
}