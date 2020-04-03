namespace SoftGym.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly IDeletableEntityRepository<ClientTrainer> clientTrainersRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            IDeletableEntityRepository<ClientTrainer> clientTrainersRepository)
        {
            this.userRepository = userRepository;
            this.clientTrainersRepository = clientTrainersRepository;
        }

        public async Task<ApplicationUser> ChangeEmailAsync(string userId, string newEmail)
        {
            var user = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == userId);

            user.Email = newEmail;
            await this.userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> ChangeFirstNameAsync(string id, string firstName)
        {
            var user = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            user.FirstName = firstName;
            await this.userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> ChangeLastNameAsync(string id, string lastName)
        {
            var user = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            user.LastName = lastName;
            await this.userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<ApplicationUser> ChangeProfilePhotoAsync(string userId, string newProfilePhotoUrl)
        {
            var user = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == userId);

            user.ProfilePictureUrl = newProfilePhotoUrl;
            await this.userRepository.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<string>> GetAllEmailsAsync()
        {
            return await this.userRepository
                .All()
                .Select(x => x.Email)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllUsersAsync<T>(string trainerId = null)
        {
            if (trainerId != null)
            {
                return await this.userRepository
                .All()
                .Where(x => x.Trainers.Any(x => x.TrainerId == trainerId))
                .To<T>()
                .ToListAsync();
            }

            return await this.userRepository
                .All()
                .To<T>()
                .ToListAsync();
        }

        public async Task<string> GetCardPictureUrlAsync(string id)
        {
            var user = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            return user.Card.PictureUrl;
        }

        public async Task<string> GetFirstNameAsync(string id)
        {
            var user = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == id);

            return user.FirstName;
        }

        public async Task<string> GetLastNameAsync(string id)
        {
            var user = await this.userRepository
                   .All()
                   .FirstOrDefaultAsync(x => x.Id == id);

            return user.LastName;
        }

        public async Task<string> GetProfilePictureUrlAsync(string id)
        {
            var user = await this.userRepository
                   .All()
                   .FirstOrDefaultAsync(x => x.Id == id);

            return user.ProfilePictureUrl;
        }

        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await this.userRepository
                .All()
                .FirstAsync(x => x.Id == id);
        }

        public async Task<int> GetUsersCountAsync(string trainerId = null)
        {
            if (trainerId == null)
            {
                return await this.userRepository
                    .All()
                    .CountAsync();
            }
            else
            {
                return await this.userRepository
                    .All()
                    .Where(x => x.Trainers.Any(y => y.TrainerId == trainerId))
                    .CountAsync();
            }
        }
    }
}
