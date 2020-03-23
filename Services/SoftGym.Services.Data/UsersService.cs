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

        public async Task<ClientTrainer> AddClientToTrainer(string clientId, string trainerId)
        {
            var client = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == clientId);

            var trainer = await this.userRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == trainerId);

            if (this.clientTrainersRepository.AllWithDeleted().Any(x => x.ClientId == clientId && x.TrainerId == trainerId))
            {
                var clientTrainer = await this.clientTrainersRepository
                    .AllWithDeleted()
                    .FirstAsync(x => x.ClientId == clientId && x.TrainerId == trainerId);
                clientTrainer.IsDeleted = false;
                await this.clientTrainersRepository.SaveChangesAsync();

                return clientTrainer;
            }

            var newClientTrainer = new ClientTrainer()
            {
                Client = client,
                ClientId = clientId,
                Trainer = trainer,
                TrainerId = trainerId,
            };

            client.Trainers.Add(newClientTrainer);
            trainer.Clients.Add(newClientTrainer);

            await this.clientTrainersRepository.AddAsync(newClientTrainer);
            await this.clientTrainersRepository.SaveChangesAsync();

            return newClientTrainer;
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

        public async Task<IEnumerable<T>> GetAllUsersAsync<T>()
        {
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

        public async Task<ClientTrainer> RemoveClientFromTrainer(string clientId, string trainerId)
        {
            var clientTrainer = await this.clientTrainersRepository
                .All()
                .FirstAsync(x => x.ClientId == clientId && x.TrainerId == trainerId);

            this.clientTrainersRepository.Delete(clientTrainer);
            await this.clientTrainersRepository.SaveChangesAsync();

            return clientTrainer;
        }
    }
}
