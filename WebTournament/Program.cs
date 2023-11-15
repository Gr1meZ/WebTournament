using DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

var loggerFactory = LoggerFactory.Create(loggingBuilder => loggingBuilder.AddConsole());
loggerFactory.CreateLogger<Program>();

await app.Services.AutoMigrateDatabase();
await app.Services.CreateRoles();

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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
