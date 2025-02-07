namespace StartupWing_BlazorServer.Components.Services;
using System.IdentityModel.Tokens.Jwt;

public class TokenService
{
    public string DecodeToken(string? token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodedToken = handler.ReadJwtToken(token);
        var payload = decodedToken.Claims;
        foreach (var p in payload)
        {
            if (p.Type == "sub")
            {
                return p.Value;
            }
        }
        return "";
    }
}