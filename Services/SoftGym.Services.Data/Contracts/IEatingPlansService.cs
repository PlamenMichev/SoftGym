namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;
    using SoftGym.Web.ViewModels.EatingPlans;

    public interface IEatingPlansService
    {
        public Task<EatingPlan> GenerateEatingPlanAsync(GenerateInputModel inputModel);

        public Task<ICollection<T>> GetAllPlansAsync<T>(string id = null);

        public Task<T> GetPlanAsync<T>(string id);

        public bool HasUserActivePlan(string userId);

        public Task DeletePlanAsync(string planId);
    }
}
