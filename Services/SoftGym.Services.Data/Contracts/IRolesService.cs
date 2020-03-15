namespace SoftGym.Services.Data.Contracts
{
    using SoftGym.Data.Models;

    using System.Threading.Tasks;

    public interface IRolesService
    {
        public Task<ApplicationUser> AddRoleAsync(string userId, string roleName);

        public Task<ApplicationUser> RemoveRoleAsync(string userId, string roleName);
    }
}
