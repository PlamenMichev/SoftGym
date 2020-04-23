namespace SoftGym.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SoftGym.Data;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Data.Repositories;
    using SoftGym.Services.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.Trainers.Exercises;
    using Xunit;

    public class ExercisesServiceTests
    {
        public ExercisesServiceTests()
        {
            new MapperInitializationProfile();
        }

        [Theory]
        [InlineData("Test name", Difficulty.Easy, ExerciseType.Cardio)]
        [InlineData("Sho", Difficulty.Hard, ExerciseType.Abs)]
        [InlineData("Really long name to test", Difficulty.Easy, ExerciseType.Back)]
        [InlineData("Name with digists12312", Difficulty.Medium, ExerciseType.Legs)]
        public async Task AddExerciseAsyncShouldAddExerciseProperly(
            string exerciseName,
            Difficulty difficulty,
            ExerciseType type)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var inputModel = new AddExerciseInputModel()
            {
                Name = exerciseName,
                Difficulty = difficulty,
                Type = type,
                VideoFile = fileMock.Object,
            };

            var result = await service.AddExerciseAsync(inputModel);

            Assert.Single(exercisesRepository.All());
            Assert.NotNull(result.Id);
            Assert.Equal(type, inputModel.Type);
        }

        [Fact]
        public async Task DeleteExerciseAsyncShouldDeleteExercise()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var inputModel = new AddExerciseInputModel()
            {
                Name = "Plamen",
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoFile = fileMock.Object,
            };

            var exercise = await service.AddExerciseAsync(inputModel);
            var result = await service.DeleteExerciseAsync(exercise.Id);

            Assert.True(result.IsDeleted);
        }

        [Fact]
        public async Task DeleteExerciseAsyncThrows()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var inputModel = new AddExerciseInputModel()
            {
                Name = "Plamen",
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoFile = fileMock.Object,
            };

            var exercise = await service.AddExerciseAsync(inputModel);

            await Assert.ThrowsAnyAsync<Exception>(async () => await service.DeleteExerciseAsync("random id"));
        }

        [Theory]
        [InlineData("name", "editedName")]
        [InlineData("Longer name", "sho")]
        public async Task EditExerciseAsyncShouldEditProperly(
            string exerciseName,
            string exerciseEditedName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var inputModel = new AddExerciseInputModel()
            {
                Name = exerciseName,
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoFile = fileMock.Object,
            };

            var exercise = await service.AddExerciseAsync(inputModel);

            var editInputModel = new EditExerciseInputModel()
            {
                Id = exercise.Id,
                Name = exerciseEditedName,
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoUrl = "some video url",
                VideoFile = fileMock.Object,
            };

            var editedExercise = await service.EditExerciseAsync(editInputModel);

            Assert.Equal(exerciseEditedName, editedExercise.Name);
        }

        [Theory]
        [InlineData("name", "s")]
        [InlineData("Longer name", null)]
        public async Task EditExerciseAsyncShouldThrow(
            string exerciseName,
            string exerciseEditedName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var inputModel = new AddExerciseInputModel()
            {
                Name = exerciseName,
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoFile = fileMock.Object,
            };

            var exercise = await service.AddExerciseAsync(inputModel);

            var editInputModel = new EditExerciseInputModel()
            {
                Id = exercise.Id,
                Name = exerciseEditedName,
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoUrl = "some video url",
                VideoFile = fileMock.Object,
            };

            Assert.DoesNotContain(exerciseEditedName, exercisesRepository.All().Select(x => x.Name));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(50)]
        [InlineData(1)]
        public async Task GetExercisesCountAsyncShouldReturnCorrectCount(int addionsCount)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var tasks = new List<Task>();
            for (int i = 0; i < addionsCount; i++)
            {
                var inputModel = new AddExerciseInputModel()
                {
                    Name = "Test name",
                    Difficulty = Difficulty.Easy,
                    Type = ExerciseType.Abs,
                    VideoFile = fileMock.Object,
                };

                tasks.Add(service.AddExerciseAsync(inputModel));
            }

            await Task.WhenAll(tasks);

            var count = await service.GetExercisesCountAsync();

            Assert.Equal(addionsCount, count);
        }

        [Fact]
        public async Task GetExerciseAsyncShouldReturnTheCorrectExercises()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var inputModel = new AddExerciseInputModel()
            {
                Name = "Test name",
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoFile = fileMock.Object,
            };

            var exercise = await service.AddExerciseAsync(inputModel);
            var result = await service.GetExerciseAsync<TestExerciseModel>(exercise.Id);

            Assert.Equal(exercise.Id, result.Id);
            Assert.Equal(exercise.Name, result.Name);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Fake Id")]
        public async Task GetExerciseAsyncShouldReturnNull(string id)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var inputModel = new AddExerciseInputModel()
            {
                Name = "Test name",
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoFile = fileMock.Object,
            };

            var exercise = await service.AddExerciseAsync(inputModel);
            var result = await service.GetExerciseAsync<TestExerciseModel>(id);

            Assert.Null(result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("1")]
        [InlineData("2")]
        public async Task GetAllExercisesAsyncShouldWorkCorrectly(
            string filterType)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new ApplicationDbContext(options);
            var exercisesRepository = new EfDeletableEntityRepository<Exercise>(db);
            var cloudinaryService = new Mock<ICloudinaryService>();
            var fileMock = new Mock<IFormFile>();

            var service = new ExercisesService(exercisesRepository, cloudinaryService.Object);

            var inputModel = new AddExerciseInputModel()
            {
                Name = "Test name",
                Difficulty = Difficulty.Easy,
                Type = ExerciseType.Abs,
                VideoFile = fileMock.Object,
            };

            await service.AddExerciseAsync(inputModel);
            var result = await service.GetAllExercisesAsync<TestExerciseModel>(filterType);

            Assert.NotNull(result);
        }

        public class TestExerciseModel : IMapFrom<Exercise>
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}
