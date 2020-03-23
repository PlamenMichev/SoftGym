namespace SoftGym.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SoftGym.Common;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;

    public class TrainersService : ITrainersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> trainersRepository;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly IDeletableEntityRepository<ClientTrainer> clientTrainersRepository;
        private readonly string trainerRoleId;

        public TrainersService(
            IDeletableEntityRepository<ApplicationUser> trainersRepository,
            RoleManager<ApplicationRole> roleManager,
            IDeletableEntityRepository<ClientTrainer> clientTrainersRepository)
        {
            this.trainersRepository = trainersRepository;
            this.roleManager = roleManager;
            this.clientTrainersRepository = clientTrainersRepository;
            this.trainerRoleId = this.roleManager
                .Roles
                .First(x => x.Name.ToLower() == GlobalConstants.TrainerRoleName.ToLower())
                .Id;
        }

        public async Task<ClientTrainer> AddClientToTrainer(string clientId, string trainerId)
        {
            var client = await this.trainersRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == clientId);

            var trainer = await this.trainersRepository
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

        public async Task ChangeDescriptionAync(string id, string description)
        {
            var user = await this.trainersRepository
                .All()
                .Where(x => x.Id == id)
                .FirstAsync();

            user.Description = description;
            await this.trainersRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllTrainersAsync<T>(string clientId = null)
        {
            if (clientId == null)
            {
                return await this.trainersRepository
                    .All()
                    .Where(x => x.Roles.Any(y => y.RoleId == this.trainerRoleId))
                    .To<T>()
                    .ToListAsync();
            }

            return await this.trainersRepository
                .All()
                .Where(x => x.Clients.Any(x => x.ClientId == clientId))
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetTrainerAsync<T>(string trainerId)
        {
            return await this.trainersRepository
                .All()
                .Where(x => x.Id == trainerId)
                .To<T>()
                .FirstAsync();
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
