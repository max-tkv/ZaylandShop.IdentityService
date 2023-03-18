namespace ZaylandShop.IdentityService.Options;

public class JwtOption
{
    public string Issuer { get; set; }
    
    public string Audience { get; set; }
    
    public string Secret { get; set; }
    
    public string ExpirationMinutes { get; set; }
}