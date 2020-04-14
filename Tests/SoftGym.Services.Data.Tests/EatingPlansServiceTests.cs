namespace SoftGym.Services.Data.Tests
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SoftGym.Data;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Data.Repositories;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.EatingPlans;
    using SoftGym.Web.ViewModels.EatingPlans.Enums;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class EatingPlansServiceTests
    {
        private Mock<IDeletableEntityRepository<EatingPlan>> eatingPlansRepository;
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
            this.mealsPlansRepository = new Mock<IRepository<MealPlan>>();
            this.notificationsService = new Mock<NotificationsService>(
                new Mock<IDeletableEntityRepository<Notification>>().Object,
                new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null).Object);
            this.mealsRepository = new Mock<IDeletableEntityRepository<Meal>>();
            this.usersService =
                new Mock<IUsersService>();
        }

        [Fact]
        public async Task GenerateEatingPlanReturnsPlan()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CardsDb").Options;
            var db = new ApplicationDbContext(options);
            var mealsRepositoryInMemory = new EfDeletableEntityRepository<Meal>(db);

            var service = new EatingPlansService(
                this.eatingPlansRepository.Object,
                mealsRepositoryInMemory,
                this.mealsPlansRepository.Object,
                this.usersService.Object,
                this.notificationsService.Object);

            for (int i = 0; i < 4; i++)
            {
                await mealsRepositoryInMemory.AddAsync(new Meal()
                {
                    Name = "Supa",
                    CaloriesPer100Grams = 100,
                    PictureUrl = "neshto stranno",
                    Type = MealType.Breakfast,
                });
            }

            for (int i = 0; i < 4; i++)
            {
                await mealsRepositoryInMemory.AddAsync(new Meal()
                {
                    Name = "Supa",
                    CaloriesPer100Grams = 100,
                    PictureUrl = "neshto stranno",
                    Type = MealType.Lunch,
                });
            }

            for (int i = 0; i < 4; i++)
            {
                await mealsRepositoryInMemory.AddAsync(new Meal()
                {
                    Name = "Supa",
                    CaloriesPer100Grams = 100,
                    PictureUrl = "neshto stranno",
                    Type = MealType.Dinner,
                });
            }

            for (int i = 0; i < 7; i++)
            {
                await mealsRepositoryInMemory.AddAsync(new Meal()
                {
                    Name = "Supa",
                    CaloriesPer100Grams = 100,
                    PictureUrl = "neshto stranno",
                    Type = MealType.Snack,
                });
            }

            await mealsRepositoryInMemory.SaveChangesAsync();
            var user = new ApplicationUser();

            this.usersService.Setup(x => x.GetUserByIdAsync(user.Id))
                .Returns(Task.FromResult(user));

            var inputModel = new GenerateInputModel
            {
                Activity = "high",
                Age = 17,
                HasUserActivePlan = false,
                DurationInDays = 30,
                FoodPreferences = new List<SoftGym.Data.Models.Enums.FoodPreference>(),
                Gender = "male",
                Goal = Goal.Gain,
                Height = 180,
                Id = user.Id,
                Weight = 80,
            };

            var result = await service.GenerateEatingPlanAsync(inputModel);

            Assert.Equal(15, result.Meals.Count);
        }
    }
}
