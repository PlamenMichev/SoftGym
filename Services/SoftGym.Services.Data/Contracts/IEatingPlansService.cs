namespace SoftGym.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using SoftGym.Data.Models;
    using SoftGym.Web.ViewModels.EatingPlans;

    public interface IEatingPlansService
    {
        public Task<EatingPlan> GenerateEatingPlanAsync(GenerateInputModel inputModel);
    }
}
