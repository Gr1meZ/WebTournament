using WebTournament.Application;
using WebTournament.Application.Configuration.AutoMapper;
using WebTournament.Infrastructure.IoC;
using WebTournament.Presentation.MVC.AutoMapper;
using WebTournament.Presentation.MVC.Filters;
using WebTournament.Presentation.MVC.ProgramExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());
// ----- Database -----
builder.Services.AddDatabase(builder.Configuration, builder.Environment.EnvironmentName);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddCustomServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(PresentationProfile), typeof(ApplicationProfile));
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies([typeof(Program).Assembly, typeof(ApplicationAssembleReference).Assembly]); });
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddCustomizedHealthCheck(builder.Configuration, builder.Environment);

var app = builder.Build();

// ----- Role initializer -----
await app.Services.AutoMigrateDatabaseAsync();
await app.Services.CreateRolesAsync();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.UseCustomizedHealthCheck(app.Environment);

app.Run();


