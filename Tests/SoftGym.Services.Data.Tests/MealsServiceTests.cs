namespace SoftGym.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SoftGym.Data;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Data.Repositories;
    using SoftGym.Services.Contracts;
    using SoftGym.Web.ViewModels.Trainers.Meals;
    using Xunit;

    public class MealsServiceTests
    {
        public MealsServiceTests()
        {
            new MapperInitializationProfile();
        }

        [Theory]
        [MemberData(nameof(testDateTime))]
        public async Task AddMealAsyncShouldAddMealProperly(
            string name,
            double caloriesPer100Grams,
            MealType mealType,
            IEnumerable<SoftGym.Data.Models.Enums.FoodPreference> foodPreferencesData)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var mealsRepository = new EfDeletableEntityRepository<Meal>(db);
            var mealsPreferencesRepository = new EfRepository<MealPreference>(db);
            var foodPreferencesRepository = new EfRepository<SoftGym.Data.Models.FoodPreference>(db);

            var foodPreferences = new List<SoftGym.Data.Models.FoodPreference>();
            for (int i = 0; i < Enum.GetValues(typeof(SoftGym.Data.Models.Enums.FoodPreference)).Length; i++)
            {
                foodPreferences.Add(new SoftGym.Data.Models.FoodPreference()
                {
                    Preference = (SoftGym.Data.Models.Enums.FoodPreference)Enum.GetValues(typeof(SoftGym.Data.Models.Enums.FoodPreference)).GetValue(i),
                });

                await foodPreferencesRepository.AddAsync(foodPreferences[i]);
            }

            await foodPreferencesRepository.SaveChangesAsync();
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new MealsService(
                mealsRepository,
                mealsPreferencesRepository,
                cloudinaryService.Object,
                foodPreferencesRepository);

            var inputModel = new AddMealInputModel()
            {
                Name = name,
                CaloriesPer100Grams = caloriesPer100Grams,
                PictureFile = fileMock.Object,
                Type = mealType,
                FoodPreferences = foodPreferencesData,
            };

            var result = await service.AddMealAsync(inputModel);

            Assert.NotNull(result);
        }

        public static object[][] testDateTime =
        {
            new object[] { "Test name", 150, MealType.Breakfast, new List<SoftGym.Data.Models.Enums.FoodPreference>() { SoftGym.Data.Models.Enums.FoodPreference.Egg } },
            new object[] { "Very long name for meal", 40, MealType.Lunch, new List<SoftGym.Data.Models.Enums.FoodPreference>() { SoftGym.Data.Models.Enums.FoodPreference.Egg, SoftGym.Data.Models.Enums.FoodPreference.Milk, SoftGym.Data.Models.Enums.FoodPreference.Vegetarian } },
            new object[] { "sho", 600, MealType.Dinner, new List<SoftGym.Data.Models.Enums.FoodPreference>() { SoftGym.Data.Models.Enums.FoodPreference.Egg } },
            new object[] { "Panckakes", 251, MealType.Snack, new List<SoftGym.Data.Models.Enums.FoodPreference>() },
            new object[] { "A lot of words for name", 142, MealType.Breakfast, new List<SoftGym.Data.Models.Enums.FoodPreference>() },
            new object[] { "shorter name", 231, MealType.Snack, new List<SoftGym.Data.Models.Enums.FoodPreference>() { SoftGym.Data.Models.Enums.FoodPreference.Egg } },
            new object[] { "Bil", 421, MealType.Dinner, new List<SoftGym.Data.Models.Enums.FoodPreference>() },
        };
    }
}
