namespace SoftGym.Data.Seeding
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using SoftGym.Data.Models;
    using SoftGym.Data.Seeding.Dtos;

    public class ExercisesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (!dbContext.Exercises.Any())
            {
                var exercises = JsonConvert.DeserializeObject<ExerciseDto[]>(File.ReadAllText(@"../../Data/SoftGym.Data/Seeding/Data/Exercises.json"));

                foreach (var exercise in exercises)
                {
                    var newExercise = new Exercise()
                    {
                        Name = exercise.Name,
                        VideoUrl = exercise.VideoUrl,
                        Type = exercise.Type,
                        Difficulty = exercise.Difficulty,
                    };

                    await dbContext.Exercises.AddAsync(newExercise);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
