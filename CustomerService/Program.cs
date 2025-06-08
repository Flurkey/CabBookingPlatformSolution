var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        policy =>
        {
            policy.WithOrigins("https://localhost:7179")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddControllers();

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS
app.UseCors("AllowWebApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
