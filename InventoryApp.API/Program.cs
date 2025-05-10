using AutoMapper;
using FluentValidation;
using InventoryApp.API.Extensions;
using InventoryApp.API.Middleware;
using InventoryApp.Infrastructure.Data;
using InventoryApp.Service.AutoMapperProfile;
using InventoryApp.Service.Validators;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo
    .Console()
    .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("starting server.");

    var builder = WebApplication.CreateBuilder(args);

    //Add support to logging with SERILOG
    builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        loggerConfiguration.WriteTo.Console();
        loggerConfiguration.WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day);
        loggerConfiguration.ReadFrom.Configuration(context.Configuration);
    });

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString));

    builder.Services.AddServices();

    var config = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile(new AutoMapperProfile());
    });

    var mapper = config.CreateMapper();

    builder.Services.AddSingleton(mapper);

    //builder.Services.AddValidator();
    builder.Services.AddValidatorsFromAssemblyContaining<CategoryDtoValidator>();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    // Add services to the container.
    builder.Services.AddControllers();

    var app = builder.Build();

    app.UseMiddleware<ExceptionMiddleware>();

    //Add support to logging request with SERILOG
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication();

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