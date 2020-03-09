namespace SoftGym.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using SoftGym.Data.Models;
    using SoftGym.Web.ViewModels.Trainers.Meals;

    public interface IMealsService
    {
        public Task<Meal> AddMealAsync(AddMealInputModel inputModel);
    }
}
