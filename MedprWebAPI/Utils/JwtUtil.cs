using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedprModels.Responses;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AspNetSample.WebAPI.Utils;

public class JwtUtilSha256 : IJwtUtil
{
    private readonly IConfiguration _configuration;

    public JwtUtilSha256(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public TokenResponse GenerateToken(UserModelResponse model)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:JwtSecret"]));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var nowUtc = DateTime.UtcNow;
        var exp = nowUtc.AddMinutes(double.Parse(_configuration["Token:ExpiryMinutes"]))
            .ToUniversalTime();

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, model.Login),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")), //jwt uniq id from spec
            new Claim(ClaimTypes.NameIdentifier, model.Id.ToString("D")),
            new Claim(ClaimTypes.Role, model.Role),
        };

        var jwtToken = new JwtSecurityToken(
            _configuration["Token:Issuer"],
            _configuration["Token:Issuer"],
            claims,
            expires: exp,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return new TokenResponse()
        {
            AccessToken = accessToken,
            Login = model.Login,
            Role = model.Role,
            TokenExpiration = jwtToken.ValidTo
        };
    }
}