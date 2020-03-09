namespace SoftGym.Services.Data
{
    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Trainers.Meals;
    using System.Linq;
    using System.Threading.Tasks;

    public class MealsService : IMealsService
    {
        private readonly IDeletableEntityRepository<Meal> mealsRepository;
        private readonly IRepository<MealPreference> mealsPreferencesRepository;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IRepository<FoodPreference> foodPreferenceRepository;

        public MealsService(
            IDeletableEntityRepository<Meal> mealsRepository,
            IRepository<MealPreference> mealsPreferencesRepository,
            ICloudinaryService cloudinaryService,
            IRepository<FoodPreference> foodPreferenceRepository)
        {
            this.mealsRepository = mealsRepository;
            this.mealsPreferencesRepository = mealsPreferencesRepository;
            this.cloudinaryService = cloudinaryService;
            this.foodPreferenceRepository = foodPreferenceRepository;
        }

        public async Task<Meal> AddMealAsync(AddMealInputModel inputModel)
        {
            Meal meal = new Meal
            {
                Name = inputModel.Name,
                CaloriesPer100Grams = inputModel.CaloriesPer100Grams,
                PictureUrl = await this.cloudinaryService.UploudAsync(inputModel.PictureFile),
                Type = inputModel.Type,
            };

            foreach (var foodPreference in inputModel.FoodPreferences)
            {
                FoodPreference currentPreference;
                currentPreference = await this.foodPreferenceRepository
                    .All()
                    .FirstAsync(x => x.Preference == foodPreference);


                MealPreference mealPreference = new MealPreference
                {
                    Meal = meal,
                    MealId = meal.Id,
                    Preference = currentPreference,
                    PreferenceId = currentPreference.Id,
                };

                await this.mealsPreferencesRepository.AddAsync(mealPreference);
                meal.FoodPreferences.ToList().Add(mealPreference);
                currentPreference.Meals.ToList().Add(mealPreference);
            }

            await this.mealsRepository.AddAsync(meal);
            await this.mealsRepository.SaveChangesAsync();

            return meal;
        }
    }
}
