namespace SoftGym.Services.Data.Contracts
{
    using SoftGym.Data.Models;
    using System.Threading.Tasks;

    public interface IUsersService
    {
        public Task<string> GetFirstNameAsync(string id);

        public Task<string> GetLastNameAsync(string id);

        public Task<string> GetCardPictureUrlAsync(string id);

        public Task<string> GetProfilePictureUrlAsync(string id);

        public Task<ApplicationUser> ChangeFirstNameAsync(string id, string firstName);

        public Task<ApplicationUser> ChangeLastNameAsync(string id, string lastName);

        public Task<ApplicationUser> ChangeProfilePhotoAsync(string userId, string newProfilePhotoUrl);
    }
}
