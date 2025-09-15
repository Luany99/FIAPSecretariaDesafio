using FIAPSecretariaDesafio.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FIAPSecretariaDesafio.Application.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUsuarioService _userService;

        public AuthenticateService(IUsuarioService userService)
        {
            _userService = userService;
        }

        public string Authenticate(string userName, string password)
        {

            var user = _userService.GetUserByName(userName).Result;

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Usuário ou senha inválidos");
            }


            var keyBytes = Encoding.ASCII.GetBytes("K0zLl1IM8Z8nECy5Zt3J+0/vXG4Q8qD5aZ2bL6XwVxA=");

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                 new Claim(ClaimTypes.NameIdentifier, userName)
                 }),
                NotBefore = DateTime.Now,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                                      new SymmetricSecurityKey(keyBytes),
                                      SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public static bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

    }
}
