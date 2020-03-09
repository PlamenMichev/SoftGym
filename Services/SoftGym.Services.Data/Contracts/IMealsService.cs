namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;
    using SoftGym.Web.ViewModels.Trainers.Meals;

    public interface IMealsService
    {
        public Task<Meal> AddMealAsync(AddMealInputModel inputModel);

        public Task<IEnumerable<T>> GetMealsAsync<T>();

        public Task<T> GetMealAsync<T>(string mealId);

        public Task Delete(string mealId);

        Task<Meal> EditMealAsync(EditMealInputModel inputModel);
    }
}
