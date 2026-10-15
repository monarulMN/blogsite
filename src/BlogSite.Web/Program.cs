using BlogSite.Infrastructure;
using BlogSite.Infrastructure.Data;
using BlogSite.Infrastructure.Extensions;
using Cortex.Mediator.DependencyInjection;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Serilog;
using Slugify;
using System.Text.RegularExpressions;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/web-log-.log", rollingInterval: RollingInterval.Day)
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? 
        throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    var migrationsAssembly = typeof(ApplicationDbContext).Assembly;

    #region Dependency Injection
    builder.Services.AddInfrastructureDependency();
    #endregion

    #region Serilog Configuration
    builder.Host.UseSerilog((context, loggerConfiguration) => loggerConfiguration
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(context.Configuration));
    #endregion

    #region Cortex Mediator Configuration
    builder.Services.AddCortexMediator(
        new[] {typeof(Program)},
        options => options.AddDefaultBehaviors());
    #endregion

    #region Mapster Configuration
    builder.Services.AddMapster();

    //custom configuration
    //var config = TypeAdapterConfig.GlobalSettings;
    //config.Scan(typeof(MapsterConfiguration).Assembly);
    //builder.Services.AddSingleton(config);
    //builder.Services.AddScoped<IMapper, ServiceMapper>();
    #endregion

    #region SlugHelper Configuration
    builder.Services.AddSingleton(sp =>
    {
        var config = new SlugHelperConfiguration
        {
            DeniedCharactersRegex = new Regex("[^a-zA-Z0-9-]"),
            StringReplacements = { { " ", "-" } },
            ForceLowerCase = true,
            CollapseDashes = true,
            TrimWhitespace = true
        };

        return new SlugHelper(config);
    });

    builder.Services.AddScoped<ISlugHelper, SlugHelper>();
    #endregion

    #region DbContext Configuration
    builder.Services.AddDbContext(connectionString, migrationsAssembly);
    #endregion

    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    #region Identity Configuration
    builder.Services.AddIdentity();
    #endregion

    builder.Services.AddControllersWithViews();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();
    app.UseStaticFiles();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    //app.MapRazorPages()
    //   .WithStaticAssets();

    Log.Information("Application started successfully.");

    app.Run();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
