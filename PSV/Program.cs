using Microsoft.EntityFrameworkCore;
using PSV.Models;
using PSV.Services;
using PSV.Utils;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("msgtemplates.json", optional: false, reloadOnChange: true);

var configuration = configurationBuilder.Build();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddHostedService<TempFolderCleanerService>();

//Database
builder.Services.AddDbContext<Repository>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
});

//SMS
builder.Services.Configure<SmsServiceSettings>(
    configuration.GetSection("SmsServiceSettings")
);

builder.Services.AddScoped<ISmsService, SmsService>();
builder.Configuration.AddConfiguration(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();