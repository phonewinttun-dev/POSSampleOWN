using Microsoft.EntityFrameworkCore;
using POSSampleOWN.domain;
using POSSampleOWN.domain.Features;
using Scalar.AspNetCore;
using Serilog;


try
{
    var builder = WebApplication.CreateBuilder(args);
    
    Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/mini_pos_log.txt", rollingInterval: RollingInterval.Hour)
    .CreateLogger();

    //Add Serilog
    builder.Services.AddSerilog();

    // Add services to the container.
    builder.Services.AddControllers();

    // Add Dependency Injection
    builder.AddDomain();
    
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapSwagger("/openapi/{documentName}.json");
        app.MapScalarApiReference();
    }

    // Add CORS Policy
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
    });

    

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}

catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}


