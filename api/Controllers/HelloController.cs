using api.Dto;
using api.Services;
using DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HelloController : ControllerBase
{

    private readonly UserService _userService;
    private readonly DecodeJwtService _decodeJwtService;
    private readonly AuthJwt _authjwt;
    public HelloController(UserService userService,AuthJwt authJwt,DecodeJwtService decodeJwtService)
    {
        _userService = userService;
        _authjwt = authJwt;
        _decodeJwtService = decodeJwtService;
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Auth([FromBody] LoginRequest loginRequest)
    {
        var result=await _authjwt.Authenticate(loginRequest.email, loginRequest.password);
        
        return result;
    }

    [HttpGet("decoded-token")]
    [Authorize]
    public async Task<IActionResult> DecodedToken()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

        var decodeResult=await _decodeJwtService.decode(token);
        
        /* esto es solo para ver la respuesta
         if (decodeResult is OkObjectResult okResult)
        {
            var result = okResult.Value.ToString();
            Console.WriteLine($"Decoded Token Response: {result}");
        }*/
       
        return decodeResult;

    }
    
    [HttpGet("getUser/{id}")]
    public IActionResult GetUserById(int id){
        var user= _userService.GetUserById(id);
        if (user == null)
        {
            return NotFound(new { message = "Usuario no encontrado" });
        }

        return Ok(user);
    }
    
    [HttpGet("allUsers")]
    public List<User> GetAllUsers()
    {
        var users = _userService.GetAllUsers();
        Console.WriteLine($"Usuarios obtenidos: {users.ToList()}");
        return users;
        
    }

}