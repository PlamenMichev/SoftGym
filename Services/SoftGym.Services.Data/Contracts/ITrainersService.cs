namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;

    public interface ITrainersService
    {
        public Task<IEnumerable<T>> GetAllTrainersAsync<T>(string clientId = null);

        public Task<T> GetTrainerAsync<T>(string trainerId);

        public Task ChangeDescriptionAync(string id, string description);

        public Task<ClientTrainer> AddClientToTrainer(string clientId, string trainerId);

        public Task<ClientTrainer> RemoveClientFromTrainer(string clientId, string trainerId);
    }
}
