namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;
    using SoftGym.Web.ViewModels.WorkoutPlans;

    public interface IWorkoutPlansService
    {
        public Task<WorkoutPlan> GenerateWorkoutPlanAsync(GenerateWorkoutPlanInputModel inputModel);

        public Task<IEnumerable<T>> GetWorkoutPlansAsync<T>(string id = null);

        public Task<T> GetWorkoutPlanAsync<T>(string planId);

        public Task<WorkoutPlan> DeleteAsync(string planId);

        public Task<bool> HasUserActivePlan(string userId);
    }
}
