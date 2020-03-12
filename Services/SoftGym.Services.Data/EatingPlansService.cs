namespace SoftGym.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Data.Contracts;
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
            double caloriesPerDay = 0;
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

            Meal[] breakfasts = await this.mealsRepository
                .All()
                .Where(x => x.Type == MealType.Breakfast)
                .ToArrayAsync();

            int[] pickedBreakfaststs = new int[3]
            {
                -1, -1, -1,
            };
            for (int i = 0; i < 3; i++)
            {
                var random = new Random();
                int breakfastIndex = random.Next(0, breakfasts.Length);
                while (pickedBreakfaststs.Contains(breakfastIndex) ||
                    inputModel?.FoodPreferences
                    .Any(x => breakfasts[breakfastIndex].FoodPreferences
                    .Any(y => y.Preference.Preference == x)) != false)
                {
                    breakfastIndex = random.Next(0, breakfasts.Length);
                }

                pickedBreakfaststs[i] = breakfastIndex;
                var currentBreakfast = breakfasts[breakfastIndex];
                var mealPlan = new MealPlan()
                {
                    EatingPlanId = eatingPlan.Id,
                    EatingPlan = eatingPlan,
                    MealId = currentBreakfast.Id,
                    Meal = currentBreakfast,
                    MealWeight = caloriesForMeal / (currentBreakfast.CaloriesPer100Grams / 100),
                    TotalCalories = (caloriesForMeal / (currentBreakfast.CaloriesPer100Grams / 100))
                    * (currentBreakfast.CaloriesPer100Grams / 100),
                };

                await this.mealsPlansRepository.AddAsync(mealPlan);
                eatingPlan.Meals.Add(mealPlan);
                currentBreakfast.Plans.Add(mealPlan);
            }

            Meal[] lunches = await this.mealsRepository
                .All()
                .Where(x => x.Type == MealType.Lunch)
                .ToArrayAsync();

            int[] pickedLunches = new int[3]
            {
                -1, -1, -1,
            };
            for (int i = 0; i < 3; i++)
            {
                var random = new Random();
                int lunchIndex = random.Next(0, lunches.Length);
                while (pickedLunches.Contains(lunchIndex) ||
                    inputModel?.FoodPreferences
                    .Any(x => lunches[lunchIndex].FoodPreferences
                    .Any(y => y.Preference.Preference == x)) != false)
                {
                    lunchIndex = random.Next(0, lunches.Length);
                }

                pickedLunches[i] = lunchIndex;
                var currentLunch = lunches[lunchIndex];
                var mealPlan = new MealPlan()
                {
                    EatingPlanId = eatingPlan.Id,
                    EatingPlan = eatingPlan,
                    MealId = currentLunch.Id,
                    Meal = currentLunch,
                    MealWeight = caloriesForMeal / (currentLunch.CaloriesPer100Grams / 100),
                    TotalCalories = (caloriesForMeal / (currentLunch.CaloriesPer100Grams / 100))
                    * (currentLunch.CaloriesPer100Grams / 100),
                };

                await this.mealsPlansRepository.AddAsync(mealPlan);
                eatingPlan.Meals.Add(mealPlan);
                currentLunch.Plans.Add(mealPlan);
            }

            Meal[] dinners = await this.mealsRepository
                .All()
                .Where(x => x.Type == MealType.Dinner)
                .ToArrayAsync();

            int[] pickedDinners = new int[3]
            {
                -1, -1, -1,
            };
            for (int i = 0; i < 3; i++)
            {
                var random = new Random();
                int dinnerIndex = random.Next(0, dinners.Length);
                while (pickedDinners.Contains(dinnerIndex) ||
                    inputModel?.FoodPreferences
                    .Any(x => dinners[dinnerIndex].FoodPreferences
                    .Any(y => y.Preference.Preference == x)) != false)
                {
                    dinnerIndex = random.Next(0, dinners.Length);
                }

                pickedDinners[i] = dinnerIndex;
                var currentDinner = dinners[dinnerIndex];
                var mealPlan = new MealPlan()
                {
                    EatingPlanId = eatingPlan.Id,
                    EatingPlan = eatingPlan,
                    MealId = currentDinner.Id,
                    Meal = currentDinner,
                    MealWeight = caloriesForMeal / (currentDinner.CaloriesPer100Grams / 100),
                    TotalCalories = (caloriesForMeal / (currentDinner.CaloriesPer100Grams / 100))
                    * (currentDinner.CaloriesPer100Grams / 100),
                };

                await this.mealsPlansRepository.AddAsync(mealPlan);
                eatingPlan.Meals.Add(mealPlan);
                currentDinner.Plans.Add(mealPlan);
            }

            Meal[] snacks = await this.mealsRepository
               .All()
               .Where(x => x.Type == MealType.Snack)
               .ToArrayAsync();

            int[] pickedSnacks = new int[6]
            {
                -1, -1, -1, -1, -1, -1,
            };
            for (int i = 0; i < 3; i++)
            {
                var random = new Random();
                int snackIndex = random.Next(0, snacks.Length);
                while (pickedSnacks.Contains(snackIndex) ||
                    inputModel?.FoodPreferences
                    .Any(x => snacks[snackIndex].FoodPreferences
                    .Any(y => y.Preference.Preference == x)) != false)
                {
                    snackIndex = random.Next(0, snacks.Length);
                }

                pickedSnacks[i] = snackIndex;
                var currentSnack = snacks[snackIndex];
                var mealPlan = new MealPlan()
                {
                    EatingPlanId = eatingPlan.Id,
                    EatingPlan = eatingPlan,
                    MealId = currentSnack.Id,
                    Meal = currentSnack,
                    MealWeight = (caloriesForMeal / 2) / (currentSnack.CaloriesPer100Grams / 100),
                    TotalCalories = ((caloriesForMeal / 2) / (currentSnack.CaloriesPer100Grams / 100))
                    * (currentSnack.CaloriesPer100Grams / 100),
                };

                await this.mealsPlansRepository.AddAsync(mealPlan);
                eatingPlan.Meals.Add(mealPlan);
                currentSnack.Plans.Add(mealPlan);
            }

            await this.mealsRepository.SaveChangesAsync();
            return eatingPlan;
        }
    }
}
