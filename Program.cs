using GameNest_BackEnd.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<GameNestDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Додаємо Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.MapControllers();

// Swagger тільки в режимі розробки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS поки не чіпаємо
// app.UseHttpsRedirection();

app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "ok",
        service = "GameNest Backend"
    });
});

app.Run();