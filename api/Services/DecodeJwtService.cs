using System.IdentityModel.Tokens.Jwt;
using api.Dto;
using Microsoft.AspNetCore.Mvc;

namespace api.Services;

public class DecodeJwtService
{
    public async Task<IActionResult> decode(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return new UnauthorizedObjectResult(new { message = "Token no proporcionado" });

        }
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;
            var credential = jwtToken.Claims.FirstOrDefault(c => c.Type == "credential")?.Value;

            if (role == null || credential == null)
            {
                return new UnauthorizedObjectResult(new { message = "Faltan claims en el token" });
            }

            if (role.Equals("True"))
            {
                var dto = new UserDto()
                {
                    role = role,
                    credential = credential,
                };

                return new OkObjectResult(new { message = "Token deserealizado correctamente", dto });
            }
//agregar otras reglas a conveniencia
            return new UnauthorizedObjectResult(new { message = "No autorizado" });
        }
        catch (Exception ex)
        {
            return new UnauthorizedObjectResult(new { message = "Error al procesar el token", error = ex.Message });
        }
    }

}

