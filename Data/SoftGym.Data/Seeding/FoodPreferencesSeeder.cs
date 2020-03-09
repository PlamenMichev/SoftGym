namespace SoftGym.Data.Seeding
{
    using SoftGym.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class FoodPreferencesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.FoodPreferences.Any())
            {
                return;
            }

            var foodPreferences = new List<FoodPreference>();
            for (int i = 0; i < Enum.GetValues(typeof(Models.Enums.FoodPreference)).Length; i++)
            {
                foodPreferences.Add(new FoodPreference()
                {
                    Preference = (Models.Enums.FoodPreference)Enum.GetValues(typeof(Models.Enums.FoodPreference)).GetValue(i),
                });
            }

            dbContext.FoodPreferences.AddRange(foodPreferences);
            return;
        }
    }
}
