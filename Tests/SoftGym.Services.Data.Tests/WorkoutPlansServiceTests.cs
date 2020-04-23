namespace SoftGym.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SoftGym.Data;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Data.Repositories;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.EatingPlans.Enums;
    using SoftGym.Web.ViewModels.WorkoutPlans;
    using SoftGym.Web.ViewModels.WorkoutPlans.Enums;
    using Xunit;

    public class WorkoutPlansServiceTests
    {

        public WorkoutPlansServiceTests()
        {
            new MapperInitializationProfile();
        }

        public async Task<WorkoutPlansService> Before()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var workoutPlansRepositoryInMemory = new EfDeletableEntityRepository<WorkoutPlan>(db);
            var exercisesRepositoryInMemory = new EfDeletableEntityRepository<Exercise>(db);
            var workoutTrainingDaysRepositoryInMemory = new EfRepository<WorkoutTrainingDay>(db);
            var eatingPlansRepositoryInMemory = new EfRepository<TrainingDay>(db);
            var notificationsService = new Mock<INotificationsService>();

            var service = new WorkoutPlansService(
                workoutPlansRepositoryInMemory,
                exercisesRepositoryInMemory,
                workoutTrainingDaysRepositoryInMemory,
                eatingPlansRepositoryInMemory,
                notificationsService.Object);

            for (int i = 0; i < 56; i++)
            {
                if (i < 7)
                {
                    var exercise = new Exercise()
                    {
                        Name = "Test Name",
                        Difficulty = Difficulty.Medium,
                        Type = ExerciseType.Chest,
                        VideoUrl = "some video url",
                    };

                    await exercisesRepositoryInMemory.AddAsync(exercise);
                }
                else if (i < 14)
                {
                    var exercise = new Exercise()
                    {
                        Name = "Test Name",
                        Difficulty = Difficulty.Medium,
                        Type = ExerciseType.Back,
                        VideoUrl = "some video url",
                    };

                    await exercisesRepositoryInMemory.AddAsync(exercise);
                }
                else if (i < 21)
                {
                    var exercise = new Exercise()
                    {
                        Name = "Test Name",
                        Difficulty = Difficulty.Medium,
                        Type = ExerciseType.Abs,
                        VideoUrl = "some video url",
                    };

                    await exercisesRepositoryInMemory.AddAsync(exercise);
                }
                else if (i < 28)
                {
                    var exercise = new Exercise()
                    {
                        Name = "Test Name",
                        Difficulty = Difficulty.Medium,
                        Type = ExerciseType.Biceps,
                        VideoUrl = "some video url",
                    };

                    await exercisesRepositoryInMemory.AddAsync(exercise);
                }
                else if (i < 35)
                {
                    var exercise = new Exercise()
                    {
                        Name = "Test Name",
                        Difficulty = Difficulty.Medium,
                        Type = ExerciseType.Cardio,
                        VideoUrl = "some video url",
                    };

                    await exercisesRepositoryInMemory.AddAsync(exercise);
                }
                else if (i < 42)
                {
                    var exercise = new Exercise()
                    {
                        Name = "Test Name",
                        Difficulty = Difficulty.Medium,
                        Type = ExerciseType.Legs,
                        VideoUrl = "some video url",
                    };

                    await exercisesRepositoryInMemory.AddAsync(exercise);
                }
                else if (i < 49)
                {
                    var exercise = new Exercise()
                    {
                        Name = "Test Name",
                        Difficulty = Difficulty.Medium,
                        Type = ExerciseType.Shoulder,
                        VideoUrl = "some video url",
                    };

                    await exercisesRepositoryInMemory.AddAsync(exercise);
                }
                else if (i < 56)
                {
                    var exercise = new Exercise()
                    {
                        Name = "Test Name",
                        Difficulty = Difficulty.Medium,
                        Type = ExerciseType.Triceps,
                        VideoUrl = "some video url",
                    };

                    await exercisesRepositoryInMemory.AddAsync(exercise);
                }
            }

            await exercisesRepositoryInMemory.SaveChangesAsync();

            return service;
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public async Task GenerateWorkoutPlanAsyncShouldGenerateProgramForDifferentDaysCount(int daysCount)
        {
            var service = await this.Before();
            var user = new ApplicationUser();

            var inputModel = new GenerateWorkoutPlanInputModel()
            {
                DurationInDays = 30,
                Experience = Experience.Intermediate,
                Goal = Goal.Mantain,
                UserId = user.Id,
                WeekdaysCount = daysCount,
            };

            var result = await service.GenerateWorkoutPlanAsync(inputModel);

            Assert.NotNull(result.Id);
        }

        [Theory]
        [InlineData(Experience.Advanced)]
        [InlineData(Experience.Beginner)]
        [InlineData(Experience.Intermediate)]
        public async Task GenerateWorkoutPlanAsyncShouldGenerateProgramWuthAllExperiences(Experience experience)
        {
            var service = await this.Before();
            var user = new ApplicationUser();

            var inputModel = new GenerateWorkoutPlanInputModel()
            {
                DurationInDays = 30,
                Experience = experience,
                Goal = Goal.Mantain,
                UserId = user.Id,
                WeekdaysCount = 4,
            };

            var result = await service.GenerateWorkoutPlanAsync(inputModel);

            Assert.NotNull(result.Id);
        }

        [Theory]
        [InlineData(Goal.Gain)]
        [InlineData(Goal.Lose)]
        [InlineData(Goal.Mantain)]
        public async Task GenerateWorkoutPlanAsyncShouldGenerateProgramWuthAllGoals(Goal goal)
        {
            var service = await this.Before();
            var user = new ApplicationUser();

            var inputModel = new GenerateWorkoutPlanInputModel()
            {
                DurationInDays = 30,
                Experience = Experience.Intermediate,
                Goal = goal,
                UserId = user.Id,
                WeekdaysCount = 4,
            };

            var result = await service.GenerateWorkoutPlanAsync(inputModel);

            Assert.NotNull(result.Id);
        }

        [Fact]
        public async Task GetWorkoutPlansAsyncShouldGiveTheCorrectPlans()
        {
            var service = await this.Before();
            var user = new ApplicationUser();

            for (int i = 0; i < 10; i++)
            {
                var inputModel = new GenerateWorkoutPlanInputModel()
                {
                    DurationInDays = 30,
                    Experience = Experience.Intermediate,
                    Goal = Goal.Mantain,
                    UserId = user.Id,
                    WeekdaysCount = 4,
                };

                var plan = await service.GenerateWorkoutPlanAsync(inputModel);
            }

            var result = await service.GetWorkoutPlansAsync<TestWorkoutPlanModel>();
            Assert.Equal(10, result.Count());
        }

        [Fact]
        public async Task GetWorkoutPlanAsyncShouldGiveTheCorrectPlan()
        {
            var service = await this.Before();
            var user = new ApplicationUser();

            var inputModel = new GenerateWorkoutPlanInputModel()
            {
                DurationInDays = 30,
                Experience = Experience.Intermediate,
                Goal = Goal.Mantain,
                UserId = user.Id,
                WeekdaysCount = 4,
            };

            var plan = await service.GenerateWorkoutPlanAsync(inputModel);

            var result = await service.GetWorkoutPlanAsync<TestWorkoutPlanModel>(plan.Id);
            Assert.Equal(plan.Id, result.Id);
            Assert.Equal(4, result.TrainingDaysCount);
        }

        [Fact]
        public async Task DeleteAsyncShouldDelete()
        {
            var service = await this.Before();
            var user = new ApplicationUser();

            var inputModel = new GenerateWorkoutPlanInputModel()
            {
                DurationInDays = 30,
                Experience = Experience.Intermediate,
                Goal = Goal.Mantain,
                UserId = user.Id,
                WeekdaysCount = 4,
            };

            var plan = await service.GenerateWorkoutPlanAsync(inputModel);
            var deletedPlan = await service.DeleteAsync(plan.Id);

            Assert.True(deletedPlan.IsDeleted);
        }

        public class TestWorkoutPlanModel : IMapFrom<WorkoutPlan>
        {
            public string Id { get; set; }

            public int TrainingDaysCount { get; set; }
        }
    }
}
