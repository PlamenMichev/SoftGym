namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;

    public interface IUsersService
    {
        public Task<string> GetFirstNameAsync(string id);

        public Task<string> GetLastNameAsync(string id);

        public Task<string> GetCardPictureUrlAsync(string id);

        public Task<string> GetProfilePictureUrlAsync(string id);

        public Task<ApplicationUser> ChangeFirstNameAsync(string id, string firstName);

        public Task<ApplicationUser> ChangeLastNameAsync(string id, string lastName);

        public Task<ApplicationUser> ChangeProfilePhotoAsync(string userId, string newProfilePhotoUrl);

        public Task<int> GetUsersCountAsync(string trainerId = null);

        public Task<ApplicationUser> ChangeEmailAsync(string userId, string newEmail);

        public Task<IEnumerable<string>> GetAllEmailsAsync();

        public Task<IEnumerable<T>> GetAllUsersAsync<T>();

        public Task<ApplicationUser> GetUserByIdAsync(string id);

        public Task<ClientTrainer> AddClientToTrainer(string clientId, string trainerId);

        public Task<ClientTrainer> RemoveClientFromTrainer(string clientId, string trainerId);
    }
}
