namespace SoftGym.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SoftGym.Common;
    using SoftGym.Data.Models;

    public class UsersToRolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(userManager, GlobalConstants.AdministratorRoleName, "admin@admin.com");
            await SeedRoleAsync(userManager, GlobalConstants.TrainerRoleName, "trainer@trainer.com");
        }

        private static async Task SeedRoleAsync(UserManager<ApplicationUser> userManager, string roleName, string userEmail)
        {
            var user = await userManager.Users.FirstAsync(x => x.Email == userEmail);
            if (user.Roles.Any() == false)
            {
                await userManager.AddToRoleAsync(user, roleName);
            }
        }
    }
}
