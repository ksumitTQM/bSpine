using System.Globalization;
using bSpine.Data;
using bSpine.Services;
using Microsoft.EntityFrameworkCore;

// Use a fixed culture so currency and date formatting is consistent for the demo.
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;
CultureInfo.CurrentCulture = culture;
CultureInfo.CurrentUICulture = culture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// In-memory database: data is re-seeded every time the application starts.
builder.Services.AddDbContext<BSpineDbContext>(options =>
    options.UseInMemoryDatabase("bSpineDemo"));

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<ICaseService, CaseService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    SeedData.Initialize(scope.ServiceProvider);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
