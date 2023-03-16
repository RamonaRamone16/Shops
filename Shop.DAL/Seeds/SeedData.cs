using Microsoft.AspNetCore.Identity;
using Shop.Models.Entities;
using Shop.Models.Enums;

namespace Shop.DAL.Seeds
{
    public static class ApplicationDbInitializer
    {
        public async static Task SeedDataAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager) 
        {
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = Enum.GetValues(typeof(Roles)).OfType<Roles>().ToList().Select(value => value.ToString());
            foreach (var role in roles)
            {
                var roleCheck = await roleManager.RoleExistsAsync(role);
                if (!roleCheck)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        public static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@gmail.com") == null)
            {
                var user = new User
                {
                    UserName = "admin",
                    Email = "admin@gmail.com"
                };

                IdentityResult result = await userManager.CreateAsync(user, "password");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }
        }
    }
}
