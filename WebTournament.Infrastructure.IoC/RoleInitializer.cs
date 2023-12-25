using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Infrastructure.Identity.Models;

namespace WebTournament.Infrastructure.IoC
{
    public static class RoleInitializer
    {
        public static async Task CreateRolesAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new ApplicationRole(roleName));
                }
            }

            //Here you could create a super user who will maintain the web app
            var powerUser = new ApplicationUser
            {

                UserName = "admin@mail.ru",
                Email = "admin@mail.ru",
                EmailConfirmed = true

            };
            const string userPwd = "Admin1*";
            var user = await userManager.FindByEmailAsync("admin@mail.ru");

            if (user == null)
            {
                var createPowerUser = await userManager.CreateAsync(powerUser, userPwd);
                if (createPowerUser.Succeeded)
                {

                    await userManager.AddToRoleAsync(powerUser, "Admin");

                }
            }
        }
    }
}
