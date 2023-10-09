using ESFE.Chatbot;
using ESFE.Chatbot.Models;
using ESFE.Chatbot.Repositories;
using ESFE.Chatbot.Services;
using ESFE.Chatbot.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ChatBotDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("ChatBotDbContext"));
});

builder.Services.AddScoped<IGenericRepository<ClientUser>, ClientUserRepository>();
builder.Services.AddScoped<IGenericRepository<FeedBack>, FeedBackRepository>();
builder.Services.AddScoped<IGenericRepository<TypeUser>, TypeUserRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClienteUserService, ClienteUserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
