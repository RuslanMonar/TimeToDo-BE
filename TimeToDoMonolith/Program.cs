using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("TimeToDo", new OpenApiInfo
    {
        Title = "TimeToDo",
        Version = "v1",
        Description = "TimeToDo"
    });

    c.SwaggerDoc("Identity", new OpenApiInfo
    {
        Title = "Identity",
        Version = "v1",
        Description = "Identity"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/TimeToDo/swagger.json", "TimeToDo"); // Для TimeToDo
        c.SwaggerEndpoint("/swagger/Identity/swagger.json", "Identity"); // Для Identity
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
