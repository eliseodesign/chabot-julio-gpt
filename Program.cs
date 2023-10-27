using System.Text;
using ESFE.Chatbot;
using ESFE.Chatbot.Models;
using ESFE.Chatbot.Repositories;
using ESFE.Chatbot.Services;
using ESFE.Chatbot.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ChatBotDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatBotDbContext"));
});

builder.Services.AddScoped<IGenericRepository<ClientUser>, ClientUserRepository>();
builder.Services.AddScoped<IGenericRepository<FeedBack>, FeedBackRepository>();
builder.Services.AddScoped<IGenericRepository<TypeUser>, TypeUserRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClienteUserService, ClienteUserService>();

builder.Services.AddScoped<IAuthService, AuthService>();

// start JWT config
var key = builder.Configuration.GetValue<string>("JwtSettings:key");
var keyBytes = Encoding.ASCII.GetBytes(key);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
// end JWT config

// builder.Services.AddTransient<RequestLimit>(); // middleware de limites de mensajes
builder.Services.AddMemoryCache(); // Configurar la memoria caché

builder.WebHost.UseUrls("http://*:5000");

// Agregar configuración CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder
            .AllowAnyOrigin() // Esto permite cualquier origen
            .AllowAnyHeader()
            .AllowAnyMethod());
});


// start swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatBot Auth API", Version = "v1" });

    // Agrega una descripción para el esquema de seguridad (Bearer token)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Bearer token",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    // Agrega un requisito de seguridad para todas las operaciones
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });
});

// end swagger

var app = builder.Build();
// app.Listen("http://*:80");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Agregar el middleware CORS para permitir todas las solicitudes
app.UseCors("AllowAnyOrigin");

app.UseAuthentication(); // jwt auth
// app.UseMiddleware<RequestLimit>();
app.UseAuthorization();

app.MapControllers();

app.Run();
