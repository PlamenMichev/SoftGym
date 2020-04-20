namespace SoftGym.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Contracts;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.Trainers.Meals;

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

            if (inputModel.FoodPreferences != null)
            {
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
            }

            await this.mealsRepository.AddAsync(meal);
            await this.mealsRepository.SaveChangesAsync();

            return meal;
        }

        public async Task Delete(string mealId)
        {
            var entity = await this.mealsRepository
                .All()
                .FirstAsync(x => x.Id == mealId);

            this.mealsRepository.Delete(entity);
            await this.mealsRepository.SaveChangesAsync();
        }

        public async Task<Meal> EditMealAsync(EditMealInputModel inputModel)
        {
            var currentMeal = await this.mealsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == inputModel.Id);

            currentMeal.Name = inputModel.Name;
            if (inputModel.NewImageFile != null)
            {
                currentMeal.PictureUrl = await this.cloudinaryService.UploudAsync(inputModel.NewImageFile);
            }

            currentMeal.Type = inputModel.Type;
            currentMeal.CaloriesPer100Grams = inputModel.CaloriesPer100Grams;

            foreach (var foodPreference in this.mealsPreferencesRepository
            .All()
            .Where(x => x.MealId == currentMeal.Id))
            {
                this.mealsPreferencesRepository.Delete(foodPreference);
            }

            await this.mealsPreferencesRepository.SaveChangesAsync();

            if (inputModel.FoodPreferences != null)
            {
                foreach (var foodPreference in inputModel.FoodPreferences)
                {
                    FoodPreference currentPreference;
                    currentPreference = await this.foodPreferenceRepository
                        .All()
                        .FirstAsync(x => x.Preference == foodPreference);

                    MealPreference mealPreference = new MealPreference
                    {
                        Meal = currentMeal,
                        MealId = currentMeal.Id,
                        Preference = currentPreference,
                        PreferenceId = currentPreference.Id,
                    };

                    await this.mealsPreferencesRepository.AddAsync(mealPreference);
                    currentMeal.FoodPreferences.ToList().Add(mealPreference);
                    currentPreference.Meals.ToList().Add(mealPreference);
                }

                await this.mealsRepository.SaveChangesAsync();
            }

            return currentMeal;
        }

        public async Task<T> GetMealAsync<T>(string mealId)
        {
            return await this.mealsRepository
                .All()
                .Where(x => x.Id == mealId)
                .To<T>()
                .FirstAsync();
        }

        public async Task<IEnumerable<T>> GetMealsAsync<T>()
        {
            return await this.mealsRepository
                .All()
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetMealsCountAsync()
        {
            return await this.mealsRepository
                .All()
                .CountAsync();
        }
    }
}
