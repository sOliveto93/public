using System.Text;
using api.Services;
using DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthJwt>();
builder.Services.AddScoped<DecodeJwtService>();
var secretKey = builder.Configuration["Jwt:SecretKey"];

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, 
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = signingKey,
        };
    });


//agregamos el contexto
builder.Services.AddDbContext<CrudContext>(options => {
    
    var connectionString = builder.Configuration.GetConnectionString("CrudConnection");
    
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 31)));
});


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
