namespace SoftGym.Data.Seeding
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using SoftGym.Data.Models;
    using SoftGym.Data.Seeding.Dtos;

    public class MealsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Meals.Any())
            {
                var meals = JsonConvert.DeserializeObject<MealDto[]>(File.ReadAllText(@"../../Data/SoftGym.Data/Seeding/Data/Meals.json"));

                foreach (var meal in meals)
                {
                    var newMeal = new Meal()
                    {
                        Name = meal.Name,
                        PictureUrl = meal.PictureUrl,
                        Type = meal.Type,
                        CaloriesPer100Grams = meal.CaloriesPer100Grams,
                    };

                    foreach (var mealPreference in meal.MealsPreferences)
                    {
                        var newMealPreference = new MealPreference()
                        {
                            MealId = newMeal.Id,
                            PreferenceId = mealPreference.PreferenceId,
                        };

                        await dbContext.MealsPreferences.AddAsync(newMealPreference);
                    }

                    await dbContext.Meals.AddAsync(newMeal);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
