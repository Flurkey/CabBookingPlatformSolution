var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS to allow calls from your frontend (port 7179)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("https://localhost:7179") // frontend origin
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware setup
app.UseHttpsRedirection();

app.UseCors("AllowFrontend"); // Enable CORS

app.UseAuthorization();

app.MapControllers();

app.Run();
