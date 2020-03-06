namespace SoftGym.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;

    public class RolesService : IRolesService
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationRole> rolesRepository;

        public RolesService(
            IUsersService usersService,
            UserManager<ApplicationUser> userManager,
            IDeletableEntityRepository<ApplicationRole> rolesRepository)
        {
            this.usersService = usersService;
            this.userManager = userManager;
            this.rolesRepository = rolesRepository;
        }

        public async Task<ApplicationUser> AddRoleAsync(string userId, string roleName)
        {
            var currentUser = await this.usersService.GetUserByIdAsync(userId);
            await this.userManager.AddToRoleAsync(currentUser, roleName);

            return currentUser;
        }

        public async Task<ApplicationUser> RemoveRoleAsync(string userId, string roleName)
        {
            var currentUser = await this.usersService.GetUserByIdAsync(userId);
            await this.userManager.RemoveFromRoleAsync(currentUser, roleName);

            return currentUser;
        }
    }
}
