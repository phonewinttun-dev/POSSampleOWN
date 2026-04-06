using Microsoft.EntityFrameworkCore;
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

    // Add Service Layer
    builder.Services.AddScoped<POSSampleOWN.Services.ICategoryService, POSSampleOWN.Services.CategoryService>();
    builder.Services.AddScoped<POSSampleOWN.Services.IProductService, POSSampleOWN.Services.ProductService>();
    builder.Services.AddScoped<POSSampleOWN.Services.ISearchService, POSSampleOWN.Services.SearchService>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add DbContext service
    builder.Services.AddDbContext<POSSampleOWN.Data.POSDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("POSConnectionString")));
    var connectionString = builder.Configuration.GetConnectionString("POSConnectionString");
    Console.WriteLine($"Database connection string: {connectionString}" );


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
}

catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}


