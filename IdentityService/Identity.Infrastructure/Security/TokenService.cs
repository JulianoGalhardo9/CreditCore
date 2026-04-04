using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Security;
public class TokenService : ITokenService
{
    public string GenerateToken(User user)
    {
        // 1. Criamos o "manipulador" que vai escrever e formatar o token.
        var tokenHandler = new JwtSecurityTokenHandler();

        // 2. Definimos a nossa chave secreta (Em produção, isso viria de um cofre de senhas).
        var key = Encoding.ASCII.GetBytes("ChaveSuperSecretaDoBancoSafraXPBTG2024!");

        // 3. Criamos as "Claims" (Afirmações). São as informações gravadas dentro do crachá.
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            // 4. Definimos o tempo de validade
            Expires = DateTime.UtcNow.AddHours(2),
            
            // 5. "Carimbamos" o token com a nossa chave usando o algoritmo HMAC SHA256.
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        // 6. A máquina processa todas essas regras e gera o objeto do token.
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // 7. Transforma o objeto do token em uma string final para ser enviada ao usuário.
        return tokenHandler.WriteToken(token);
    }
}