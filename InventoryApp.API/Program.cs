using AutoMapper;
using InventoryApp.API.Extensions;
using InventoryApp.API.Middleware;
using InventoryApp.Infrastructure.Data;
using InventoryApp.Service.AutoMapperProfile;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();
try
{
    Log.Information("starting server.");
    var builder = WebApplication.CreateBuilder(args);

    //Add support to logging with SERILOG
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.WriteTo.Console();
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

    //builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
    //builder.Services.AddTransient(typeof(ProductService), typeof(ProductService));

    builder.Services.AddServices();

    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new AutoMapperProfile());
    });

    var mapper = config.CreateMapper();

    builder.Services.AddSingleton(mapper);

    builder.Services.AddValidator();

    // Add services to the container.
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    //Add support to logging request with SERILOG
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}