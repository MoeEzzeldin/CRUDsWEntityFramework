using Microsoft.EntityFrameworkCore;
using EntityFramework.Data;
using System.Reflection;
using Serilog;
using Serilog.Events;
using EntityFramework.Repos.IRepo;
using EntityFramework.Repos.Repo;


Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
               .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
               .Enrich.FromLogContext()
               .WriteTo.Console()
               .WriteTo.File("logs/Items-.txt", rollingInterval: RollingInterval.Day)
               .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
// Configure Serilog for logging
builder.Host.UseSerilog();


// Add services to the container.
builder.Services.AddControllersWithViews();
// Add AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
// Register the DbContext with dependency injection
builder.Services.AddDbContext<MyAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<IItemRepository, ItemRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
