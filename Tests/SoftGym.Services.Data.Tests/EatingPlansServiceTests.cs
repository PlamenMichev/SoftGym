namespace SoftGym.Services.Data.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SoftGym.Data;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Data.Repositories;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.EatingPlans;
    using SoftGym.Web.ViewModels.EatingPlans.Enums;
    using Xunit;

    public class EatingPlansServiceTests
    {
        private Mock<IDeletableEntityRepository<EatingPlan>> eatingPlansRepository;
        private Mock<IRepository<SoftGym.Data.Models.FoodPreference>> foodPreferenceRepository;
        private Mock<IRepository<MealPlan>> mealsPlansRepository;
        private Mock<IUsersService> usersService;
        private Mock<NotificationsService> notificationsService;
        private Mock<IDeletableEntityRepository<Meal>> mealsRepository;
        private Mock<IDeletableEntityRepository<ApplicationUser>> usersRepository =
            new Mock<IDeletableEntityRepository<ApplicationUser>>();

        private Mock<IDeletableEntityRepository<ClientTrainer>> clientsTrainersRepository =
            new Mock<IDeletableEntityRepository<ClientTrainer>>();

        public EatingPlansServiceTests()
        {
            this.eatingPlansRepository = new Mock<IDeletableEntityRepository<EatingPlan>>();
            this.foodPreferenceRepository = new Mock<IRepository<SoftGym.Data.Models.FoodPreference>>();
            this.mealsPlansRepository = new Mock<IRepository<MealPlan>>();
            this.notificationsService = new Mock<NotificationsService>(
                new Mock<IDeletableEntityRepository<Notification>>().Object,
                new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null).Object);
            this.mealsRepository = new Mock<IDeletableEntityRepository<Meal>>();
            this.usersService =
                new Mock<IUsersService>();
        }

        public async Task<EatingPlansService> Before()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CardsDb").Options;
            var db = new ApplicationDbContext(options);
            var mealsRepositoryInMemory = new EfDeletableEntityRepository<Meal>(db);
            var foodPreferencesRepositoryInMemory = new EfRepository<SoftGym.Data.Models.FoodPreference>(db);
            var mealPreferenceReposityoryInMemory = new EfRepository<MealPreference>(db);
            var eatingPlansRepositoryInMemory = new EfDeletableEntityRepository<EatingPlan>(db);

            var service = new EatingPlansService(
                eatingPlansRepositoryInMemory,
                mealsRepositoryInMemory,
                this.mealsPlansRepository.Object,
                this.usersService.Object,
                this.notificationsService.Object);

            for (int i = 0; i < 8; i++)
            {
                if (i <= 4)
                {
                    await mealsRepositoryInMemory.AddAsync(new Meal()
                    {
                        Name = "Supa",
                        CaloriesPer100Grams = 100,
                        PictureUrl = "some url",
                        Type = MealType.Breakfast,
                    });
                }
                else
                {
                    var meal = new Meal()
                    {
                        Name = "Supa",
                        CaloriesPer100Grams = 100,
                        PictureUrl = "some url",
                        Type = MealType.Breakfast,
                    };

                    var foodPreference = new SoftGym.Data.Models.FoodPreference()
                    {
                        Preference = SoftGym.Data.Models.Enums.FoodPreference.Egg,
                    };

                    var mealPreference = new MealPreference()
                    {
                        MealId = meal.Id,
                        Meal = meal,
                        PreferenceId = foodPreference.Id,
                        Preference = foodPreference,
                    };

                    await foodPreferencesRepositoryInMemory.AddAsync(foodPreference);
                    await mealPreferenceReposityoryInMemory.AddAsync(mealPreference);

                    meal.FoodPreferences.ToList().Add(new SoftGym.Data.Models.MealPreference()
                    {
                        MealId = meal.Id,
                        Preference = foodPreference,
                    });
                    await mealsRepositoryInMemory.AddAsync(meal);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (i <= 4)
                {
                    await mealsRepositoryInMemory.AddAsync(new Meal()
                    {
                        Name = "Supa",
                        CaloriesPer100Grams = 100,
                        PictureUrl = "some url",
                        Type = MealType.Lunch,
                    });
                }
                else
                {
                    var meal = new Meal()
                    {
                        Name = "Supa",
                        CaloriesPer100Grams = 100,
                        PictureUrl = "some url",
                        Type = MealType.Lunch,
                    };

                    var foodPreference = new SoftGym.Data.Models.FoodPreference()
                    {
                        Preference = SoftGym.Data.Models.Enums.FoodPreference.Egg,
                    };

                    var mealPreference = new MealPreference()
                    {
                        MealId = meal.Id,
                        Meal = meal,
                        PreferenceId = foodPreference.Id,
                        Preference = foodPreference,
                    };

                    await foodPreferencesRepositoryInMemory.AddAsync(foodPreference);
                    await mealPreferenceReposityoryInMemory.AddAsync(mealPreference);

                    meal.FoodPreferences.ToList().Add(new SoftGym.Data.Models.MealPreference()
                    {
                        MealId = meal.Id,
                        Preference = foodPreference,
                    });
                    await mealsRepositoryInMemory.AddAsync(meal);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                if (i <= 4)
                {
                    await mealsRepositoryInMemory.AddAsync(new Meal()
                    {
                        Name = "Supa",
                        CaloriesPer100Grams = 100,
                        PictureUrl = "some url",
                        Type = MealType.Dinner,
                    });
                }
                else
                {
                    var meal = new Meal()
                    {
                        Name = "Supa",
                        CaloriesPer100Grams = 100,
                        PictureUrl = "some url",
                        Type = MealType.Dinner,
                    };

                    var foodPreference = new SoftGym.Data.Models.FoodPreference()
                    {
                        Preference = SoftGym.Data.Models.Enums.FoodPreference.Egg,
                    };

                    var mealPreference = new MealPreference()
                    {
                        MealId = meal.Id,
                        Meal = meal,
                        PreferenceId = foodPreference.Id,
                        Preference = foodPreference,
                    };

                    await foodPreferencesRepositoryInMemory.AddAsync(foodPreference);
                    await mealPreferenceReposityoryInMemory.AddAsync(mealPreference);

                    meal.FoodPreferences.ToList().Add(new SoftGym.Data.Models.MealPreference()
                    {
                        MealId = meal.Id,
                        Preference = foodPreference,
                    });
                    await mealsRepositoryInMemory.AddAsync(meal);
                }
            }

            for (int i = 0; i < 16; i++)
            {
                if (i <= 8)
                {
                    await mealsRepositoryInMemory.AddAsync(new Meal()
                    {
                        Name = "Supa",
                        CaloriesPer100Grams = 100,
                        PictureUrl = "some url",
                        Type = MealType.Snack,
                    });
                }
                else
                {
                    var meal = new Meal()
                    {
                        Name = "Supa",
                        CaloriesPer100Grams = 100,
                        PictureUrl = "some url",
                        Type = MealType.Snack,
                    };

                    var foodPreference = new SoftGym.Data.Models.FoodPreference()
                    {
                        Preference = SoftGym.Data.Models.Enums.FoodPreference.Egg,
                    };

                    var mealPreference = new MealPreference()
                    {
                        MealId = meal.Id,
                        Meal = meal,
                        PreferenceId = foodPreference.Id,
                        Preference = foodPreference,
                    };

                    await foodPreferencesRepositoryInMemory.AddAsync(foodPreference);
                    await mealPreferenceReposityoryInMemory.AddAsync(mealPreference);

                    meal.FoodPreferences.ToList().Add(new SoftGym.Data.Models.MealPreference()
                    {
                        MealId = meal.Id,
                        Preference = foodPreference,
                    });
                    await mealsRepositoryInMemory.AddAsync(meal);
                }
            }

            await mealsRepositoryInMemory.SaveChangesAsync();

            return service;
        }

        [Theory]
        [InlineData("high", 17, 30, "male", Goal.Gain, 180, 80)]
        [InlineData("light", 30, 60, "female", Goal.Mantain, 170, 60)]
        [InlineData("medium", 20, 90, "male", Goal.Gain, 150, 80)]
        [InlineData("high", 19, 30, "female", Goal.Lose, 145, 40)]
        [InlineData("light", 15, 30, "female", Goal.Lose, 190, 140)]
        [InlineData("high", 30, 60, "male", Goal.Mantain, 210, 160)]
        public async Task GenerateEatingPlanGenerates15MealsPlanEveryTime(
            string activity,
            int age,
            int durationInDays,
            string gender,
            Goal goal,
            double height,
            double weight)
        {
            var service = await this.Before();

            var user = new ApplicationUser();

            this.usersService.Setup(x => x.GetUserByIdAsync(user.Id))
                .Returns(Task.FromResult(user));
            var inputModel = new GenerateInputModel
            {
                Activity = activity,
                Age = age,
                HasUserActivePlan = false,
                DurationInDays = durationInDays,
                FoodPreferences = new List<SoftGym.Data.Models.Enums.FoodPreference>(),
                Gender = gender,
                Goal = goal,
                Height = height,
                Id = user.Id,
                Weight = weight,
            };

            var result = await service.GenerateEatingPlanAsync(inputModel);

            Assert.Equal(15, result.Meals.Count);
        }

        [Theory]
        [InlineData(180, 80, "medium", Goal.Mantain, 2000)]
        [InlineData(140, 42, "high", Goal.Gain, 1400)]
        [InlineData(180, 80, "light", Goal.Lose, 1800)]
        [InlineData(230, 120, "light", Goal.Lose, 2500)]
        [InlineData(190, 97, "high", Goal.Gain, 3500)]
        public async Task GenerateEatingPlanGeneratesPlanWithRightCalories(
            int height,
            int weight,
            string activity,
            Goal goal,
            double minCalories)
        {
            var service = await this.Before();

            var user = new ApplicationUser();

            this.usersService.Setup(x => x.GetUserByIdAsync(user.Id))
                .Returns(Task.FromResult(user));
            var inputModel = new GenerateInputModel
            {
                Activity = activity,
                Age = 25,
                HasUserActivePlan = false,
                DurationInDays = 30,
                FoodPreferences = new List<SoftGym.Data.Models.Enums.FoodPreference>(),
                Gender = "Male",
                Goal = goal,
                Height = height,
                Id = user.Id,
                Weight = weight,
            };

            var result = await service.GenerateEatingPlanAsync(inputModel);

            Assert.True(result.CaloriesPerDay >= minCalories);
        }

        [Fact]
        public async Task GenerateEatingPlanGeneratesRightMealTypes()
        {
            var service = await this.Before();

            var user = new ApplicationUser();

            this.usersService.Setup(x => x.GetUserByIdAsync(user.Id))
                .Returns(Task.FromResult(user));
            var inputModel = new GenerateInputModel
            {
                Activity = "light",
                Age = 25,
                HasUserActivePlan = false,
                DurationInDays = 30,
                FoodPreferences = new List<SoftGym.Data.Models.Enums.FoodPreference>(),
                Gender = "Male",
                Goal = Goal.Mantain,
                Height = 190,
                Id = user.Id,
                Weight = 85,
            };

            var result = await service.GenerateEatingPlanAsync(inputModel);

            Assert.Equal(3, result.Meals.Where(x => x.Meal.Type == MealType.Breakfast).Count());
            Assert.Equal(3, result.Meals.Where(x => x.Meal.Type == MealType.Lunch).Count());
            Assert.Equal(3, result.Meals.Where(x => x.Meal.Type == MealType.Dinner).Count());
            Assert.Equal(6, result.Meals.Where(x => x.Meal.Type == MealType.Snack).Count());
        }

        [Theory]
        [InlineData(SoftGym.Data.Models.Enums.FoodPreference.Egg)]
        public async Task GenerateEatingPlanDontGenerateMealsForUsersPreferences(
            params SoftGym.Data.Models.Enums.FoodPreference[] foodPreferences)
        {
            var service = await this.Before();

            var user = new ApplicationUser();

            this.usersService.Setup(x => x.GetUserByIdAsync(user.Id))
                .Returns(Task.FromResult(user));
            var inputModel = new GenerateInputModel
            {
                Activity = "light",
                Age = 25,
                HasUserActivePlan = false,
                DurationInDays = 30,
                FoodPreferences = foodPreferences,
                Gender = "Male",
                Goal = Goal.Mantain,
                Height = 190,
                Id = user.Id,
                Weight = 85,
            };

            var result = await service.GenerateEatingPlanAsync(inputModel);

            Assert.True(
                result.Meals
                .Any(x => x.Meal.FoodPreferences
                .Any(y => y.Preference.Preference == SoftGym.Data.Models.Enums.FoodPreference.Egg)) == false);
        }

        [Fact]
        public async Task GetCardViewModelAsyncShouldReturnCorrectModel()
        {
            var service = await this.Before();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Test Db").Options;
            var db = new ApplicationDbContext(options);
            var eatingPlansRepositoryInMemory = new EfDeletableEntityRepository<EatingPlan>(db);

            var user = new ApplicationUser();

            this.usersService.Setup(x => x.GetUserByIdAsync(user.Id))
                .Returns(Task.FromResult(user));

            var inputModel = new GenerateInputModel
            {
                Activity = "light",
                Age = 25,
                HasUserActivePlan = false,
                DurationInDays = 30,
                FoodPreferences = new List<SoftGym.Data.Models.Enums.FoodPreference>(),
                Gender = "Male",
                Goal = Goal.Mantain,
                Height = 190,
                Id = user.Id,
                Weight = 85,
            };

            var plan = await service.GenerateEatingPlanAsync(inputModel);

            await eatingPlansRepositoryInMemory.AddAsync(plan);
            await eatingPlansRepositoryInMemory.SaveChangesAsync();

            AutoMapperConfig.RegisterMappings(typeof(TestModel).Assembly);
            var result = await service.GetPlanAsync<TestModel>(plan.Id);

            Assert.Equal(user.EatingPlans.First().Id, result.Id);
            Assert.Equal(15, result.MealsCount);
        }

        public class TestModel : IMapFrom<EatingPlan>
        {
            public string Id { get; set; }

            public int MealsCount { get; set; }
        }
    }
}
