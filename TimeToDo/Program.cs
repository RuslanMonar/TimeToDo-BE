using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TimeToDo.Application;
using TimeToDo.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var configurations = builder.Configuration;
// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddApplication(configurations);
services.AddInfrastructure(configurations);
services.AddCors(Opt =>
{
    Opt.AddPolicy("localhostOrigins", policy =>
    {
        policy.AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin();
    });
});

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configurations["JwtTokenIssuer"],
            ValidAudience = configurations["JwtTokenAudience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurations["JwtTokenKey"]))
        };
    });

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
