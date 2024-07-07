using HNG.Abstractions.Constants;
using HNG.Abstractions.Contracts;
using HNG.Abstractions.Models;
using HNG.Abstractions.Services.Business;
using HNG.Abstractions.Services.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HNG.Business
{
    public class AuthUserService : IAuthUserService
    {
        private readonly IUserDataService UserDataService;
        private readonly AppSettings AppSettings;

        public AuthUserService(IUserDataService userDataService,
            AppSettings appSettings)
        {
            UserDataService = userDataService;
            AppSettings = appSettings;
        }

        public async Task<UserTokenDTO> CreateToken(UserDTO user, string? token = null)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = token == null ? GetClaimsFromUser(user) : GetClaimsFromToken(token);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var authenticatingUser = await UserDataService.GetByEmailAddress(null, claims.First(g => g.Type == ClaimsTypeConst.Username).Value);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new UserTokenDTO()
            {
                AccessToken = accessToken,
                User = user
            };
        }

        private SigningCredentials GetSigningCredentials()
        {
#pragma warning disable CS8604 // Possible null reference argument.
            var key = Encoding.UTF8.GetBytes(AppSettings.Jwt.Key);
#pragma warning restore CS8604 // Possible null reference argument.
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaimsFromUser(UserDTO user)
        {
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId),
                        new Claim(ClaimsTypeConst.ClientId, AppSettings.Jwt.ClientId),
                        new Claim(ClaimsTypeConst.Username, user.Email),
                        new Claim(ClaimsTypeConst.Session, Guid.NewGuid().ToString())
                    };

            return claims;
        }

        private static List<Claim> GetClaimsFromToken(string token)
        {
            var claims = new List<Claim>();
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                // Access claims
                foreach (Claim claim in jsonToken.Claims)
                {
                    claims.Add(claim);
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }
            }
            else
            {
                throw new SecurityTokenException("Invalid token state");
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: AppSettings.Jwt.Issuer,
                audience: AppSettings.Jwt.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(AppSettings.Jwt.TokenExpirationInMinutes)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }

    }
}
