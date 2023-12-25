using System.Reflection;
using WebTournament.Application.AutoMapper;
using WebTournament.Infrastructure.IoC;
using WebTournament.Presentation.MVC.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

// ----- Database -----
builder.Services.AddDatabase(builder.Configuration);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddCustomServices(builder.Configuration);

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// ----- Role initializer -----
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

app.Run();


