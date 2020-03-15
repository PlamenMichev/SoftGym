namespace SoftGym.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.EatingPlans;
    using SoftGym.Web.ViewModels.EatingPlans.Enums;

    public class EatingPlansService : IEatingPlansService
    {
        private readonly IDeletableEntityRepository<EatingPlan> eatingPlansRepository;
        private readonly IDeletableEntityRepository<Meal> mealsRepository;
        private readonly IRepository<MealPlan> mealsPlansRepository;
        private readonly IUsersService usersService;

        public EatingPlansService(
            IDeletableEntityRepository<EatingPlan> eatingPlansRepository,
            IDeletableEntityRepository<Meal> mealsRepository,
            IRepository<MealPlan> mealsPlansRepository,
            IUsersService usersService)
        {
            this.eatingPlansRepository = eatingPlansRepository;
            this.mealsRepository = mealsRepository;
            this.mealsPlansRepository = mealsPlansRepository;
            this.usersService = usersService;
        }

        public async Task<EatingPlan> GenerateEatingPlanAsync(GenerateInputModel inputModel)
        {
            double caloriesPerDay;
            if (inputModel.Gender == "Male")
            {
                caloriesPerDay = (10 * inputModel.Weight)
                    + (6.25 * inputModel.Height)
                    - (5 * inputModel.Age)
                    + 5;
            }
            else
            {
                caloriesPerDay = (10 * inputModel.Weight)
                    + (6.25 * inputModel.Height)
                    - (5 * inputModel.Age)
                    - 161;
            }

            switch (inputModel.Activity)
            {
                case "light": caloriesPerDay = 1.30 * caloriesPerDay; break;
                case "medium": caloriesPerDay = 1.54 * caloriesPerDay; break;
                case "high": caloriesPerDay = 1.76 * caloriesPerDay; break;
            }

            if (inputModel.Goal == Goal.Gain)
            {
                caloriesPerDay = 1.15 * caloriesPerDay;
            }
            else if (inputModel.Goal == Goal.Lose)
            {
                caloriesPerDay = 0.79 * caloriesPerDay;
            }

            var eatingPlan = new EatingPlan()
            {
                UserId = inputModel.Id,
                User = await this.usersService.GetUserByIdAsync(inputModel.Id),
                ExpireDate = DateTime.UtcNow.AddDays(inputModel.DurationInDays),
                CaloriesPerDay = caloriesPerDay,
            };

            double caloriesForMeal = 0.25 * caloriesPerDay;

            await this.AddMeals(eatingPlan, inputModel, MealType.Breakfast, caloriesForMeal);
            await this.AddMeals(eatingPlan, inputModel, MealType.Lunch, caloriesForMeal);
            await this.AddMeals(eatingPlan, inputModel, MealType.Dinner, caloriesForMeal);
            await this.AddMeals(eatingPlan, inputModel, MealType.Snack, caloriesForMeal);

            await this.mealsRepository.SaveChangesAsync();
            return eatingPlan;
        }

        public async Task AddMeals(
            EatingPlan eatingPlan,
            GenerateInputModel inputModel,
            MealType mealType,
            double caloriesForMeal)
        {
            var meals = await this.mealsRepository
                .All()
                .Select(x => new
                {
                    x.Name,
                    x.Type,
                    FoodPreferences = x.FoodPreferences.Select(y => new
                    {
                        y.Preference,
                    }),
                    x.Id,
                    x.CaloriesPer100Grams,
                    x.Plans,
                })
                .Where(x => x.Type == mealType)
                .ToArrayAsync();

            if (inputModel.Goal == Goal.Gain)
            {
                meals = meals
                    .OrderByDescending(x => x.CaloriesPer100Grams)
                    .ToArray();
            }
            else if (inputModel.Goal == Goal.Lose)
            {
                meals = meals
                    .OrderBy(x => x.CaloriesPer100Grams)
                    .ToArray();
            }

            int[] pickedMeals;
            if (mealType == MealType.Snack)
            {
                pickedMeals = new int[6]
                {
                    -1, -1, -1, -1, -1, -1,
                };
                caloriesForMeal /= 2;
            }
            else
            {
                pickedMeals = new int[3]
                {
                    -1, -1, -1,
                };
            }

            for (int i = 0; i < pickedMeals.Length; i++)
            {
                var random = new Random();
                int mealIndex = 0;
                while (pickedMeals.Contains(mealIndex) ||
                    inputModel?.FoodPreferences
                    .Any(x => meals[mealIndex].FoodPreferences
                    .Any(y => y.Preference.Preference == x)) != false)
                {
                    for (int j = 1; j < meals.Length; j++)
                    {
                        if (pickedMeals.Contains(j) == false &&
                    inputModel?.FoodPreferences
                    .Any(x => meals[j].FoodPreferences
                    .Any(y => y.Preference.Preference == x)) == false)
                        {
                            mealIndex = j;
                            break;
                        }
                    }
                }

                pickedMeals[i] = mealIndex;
                var currentMeal = await this.mealsRepository
                    .All()
                    .FirstAsync(x => x.Id == meals[mealIndex].Id);
                var mealPlan = new MealPlan()
                {
                    EatingPlanId = eatingPlan.Id,
                    EatingPlan = eatingPlan,
                    MealId = currentMeal.Id,
                    Meal = currentMeal,
                    MealWeight = caloriesForMeal / (currentMeal.CaloriesPer100Grams / 100),
                    TotalCalories = (caloriesForMeal / (currentMeal.CaloriesPer100Grams / 100))
                    * (currentMeal.CaloriesPer100Grams / 100),
                };

                await this.mealsPlansRepository.AddAsync(mealPlan);
                eatingPlan.Meals.Add(mealPlan);
                currentMeal.Plans.Add(mealPlan);
            }
        }

        public async Task<ICollection<T>> GetAllPlansAsync<T>(string id = null)
        {
            if (id == null)
            {
                return await this.eatingPlansRepository
                    .All()
                    .To<T>()
                    .ToListAsync();
            }
            else
            {
                return await this.eatingPlansRepository
                    .All()
                    .Where(x => x.UserId == id)
                    .To<T>()
                    .ToListAsync();
            }
        }

        public async Task<T> GetPlanAsync<T>(string id)
        {
            return await this.eatingPlansRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstAsync();
        }

        public bool HasUserActivePlan(string userId)
        {
            var plansForUser = this.eatingPlansRepository
                .All()
                .Where(x => x.UserId == userId)
                .Select(x => new
                {
                    x.ExpireDate,
                })
                .ToList();

            return plansForUser
                .Any(x => x.ExpireDate.Subtract(DateTime.UtcNow).Hours > 0);
        }

        public async Task DeletePlanAsync(string planId)
        {
            var currentPlan = await this.eatingPlansRepository
                .All()
                .FirstAsync(x => x.Id == planId);

            this.eatingPlansRepository.Delete(currentPlan);
            await this.eatingPlansRepository.SaveChangesAsync();
        }
    }
}
