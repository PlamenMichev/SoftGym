namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITrainersService
    {
        public Task<IEnumerable<T>> GetAllTrainersAsync<T>();

        public Task<T> GetTrainerAsync<T>(string trainerId);

        public Task ChangeDescriptionAync(string id, string description);
    }
}
