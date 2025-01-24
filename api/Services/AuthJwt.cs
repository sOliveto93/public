using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class AuthJwt
{
    private readonly IConfiguration _configuration;
    private readonly UserService _userService;

    public AuthJwt(IConfiguration configuration,UserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    public async Task<IActionResult> Authenticate(string email,string password)
    {
        var user=await _userService.GetUserByEmail(email);
        if (user == null)
        {
            return new NotFoundObjectResult(new { message = "no encontrado" });
        }
        
//añadir hashing
        if (user.Password == password)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var biteKey = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
            var tokenDes = new SecurityTokenDescriptor
            {
                Subject   = new ClaimsIdentity(new Claim[]
                {
                    new Claim("name",user.UserFirstName),
                    new Claim("role", user.Role.ToString()),
                    new Claim("credential",user.Credential.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(biteKey), 
                    SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDes);
            
            return new OkObjectResult(new {token = tokenHandler.WriteToken(token)});
        }
        return new UnauthorizedObjectResult(new { message = "contraseña incorrecta." });
    }
}