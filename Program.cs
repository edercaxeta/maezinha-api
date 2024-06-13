using Maezinha.Data;
using Maezinha.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:5500", "http://127.0.0.1:5500")
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configura o DbContext
builder.Services.AddDbContext<DataContext>(options => options.UseInMemoryDatabase("Test"));

builder.Services.AddScoped<DataContext, DataContext>();

// Configura a autenticação JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "suaIssuer",
            ValidAudience = "suaAudience",
            IssuerSigningKey = GerarChaveSecreta() // Utilize um método para gerar a chave
        };
    });

var app = builder.Build();

// Configura o pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Adiciona middleware de autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowLocalhost");

app.MapControllers();

// Rota padrão para o login
app.MapPost("/api/categoria/login", async (DataContext context, LoginModel model) =>
{
    var user = await context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email && u.Senha == model.Senha);

    if (user == null)
    {
        return Results.Unauthorized();
    }

    // Gere um token JWT
    var tokenHandler = new JwtSecurityTokenHandler();
    var key = GerarChaveSecreta(); // Utilize o mesmo método para gerar a chave
    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, user.Email),
            // Adicione mais claims conforme necessário
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
    };
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var tokenString = tokenHandler.WriteToken(token);

    return Results.Ok(new { message = "Login bem-sucedido", user = user, token = tokenString });
});

app.Run();

// Método para gerar a chave secreta aleatória
SymmetricSecurityKey GerarChaveSecreta()
{
    var random = new byte[64]; // Ajuste o tamanho conforme necessário
    using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
    rng.GetBytes(random);
    return new SymmetricSecurityKey(random);
}
