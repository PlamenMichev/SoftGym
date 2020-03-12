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
        private readonly string trainerRoleId;

        public TrainersService(
            IDeletableEntityRepository<ApplicationUser> trainersRepository,
            RoleManager<ApplicationRole> roleManager)
        {
            this.trainersRepository = trainersRepository;
            this.roleManager = roleManager;
            this.trainerRoleId = this.roleManager
                .Roles
                .First(x => x.Name.ToLower() == GlobalConstants.TrainerRoleName.ToLower())
                .Id;
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

        public async Task<IEnumerable<T>> GetAllTrainersAsync<T>()
        {
            return await this.trainersRepository
                .All()
                .Where(x => x.Roles.Any(y => y.RoleId == this.trainerRoleId))
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
    }
}
