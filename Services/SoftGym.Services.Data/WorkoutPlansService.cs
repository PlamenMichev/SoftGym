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
    using SoftGym.Web.ViewModels.EatingPlans.Enums;
    using SoftGym.Web.ViewModels.WorkoutPlans;
    using SoftGym.Web.ViewModels.WorkoutPlans.Enums;

    public class WorkoutPlansService : IWorkoutPlansService
    {
        private readonly IDeletableEntityRepository<WorkoutPlan> workoutPlansRepository;
        private readonly IDeletableEntityRepository<Exercise> exercisesRepository;
        private readonly IRepository<WorkoutTrainingDay> workoutTrainingDayRepository;
        private readonly IRepository<TrainingDay> trainingDaysRepository;
        private readonly INotificationsService notificationsService;

        public WorkoutPlansService(
            IDeletableEntityRepository<WorkoutPlan> workoutPlansRepository,
            IDeletableEntityRepository<Exercise> exercisesRepository,
            IRepository<WorkoutTrainingDay> workoutTrainingDayRepository,
            IRepository<TrainingDay> trainingDaysRepository,
            INotificationsService notificationsService)
        {
            this.workoutPlansRepository = workoutPlansRepository;
            this.exercisesRepository = exercisesRepository;
            this.workoutTrainingDayRepository = workoutTrainingDayRepository;
            this.trainingDaysRepository = trainingDaysRepository;
            this.notificationsService = notificationsService;
        }

        public async Task<WorkoutPlan> GenerateWorkoutPlanAsync(GenerateWorkoutPlanInputModel inputModel)
        {
            WorkoutPlan result;
            switch (inputModel.WeekdaysCount)
            {
                case 1:
                    result =
                    await this.GenerateShortPlan(inputModel); break;
                case 2:
                    result =
                    await this.GenerateShortPlan(inputModel); break;
                default:
                    result =
               await this.GenerateLongWeekPlan(inputModel); break;
            }

            await this.workoutPlansRepository.AddAsync(result);
            await this.workoutPlansRepository.SaveChangesAsync();
            await this.notificationsService.CreateNotificationAsync(
                "You have sucessfully generated a workout program",
                $"/WorkoutPlans/Details/{result.Id}",
                result.UserId);

            return result;
        }

        public async Task<IEnumerable<T>> GetWorkoutPlansAsync<T>(string id = null)
        {
            if (id != null)
            {
                return await this.workoutPlansRepository
                    .All()
                    .Where(x => x.UserId == id)
                    .To<T>()
                    .ToListAsync();
            }

            return await this.workoutPlansRepository
                    .All()
                    .To<T>()
                    .ToListAsync();
        }

        public async Task<T> GetWorkoutPlanAsync<T>(string planId)
        {
            return await this.workoutPlansRepository
                .All()
                .Where(x => x.Id == planId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<WorkoutPlan> DeleteAsync(string planId)
        {
            var entity = await this.workoutPlansRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == planId);

            this.workoutPlansRepository.Delete(entity);
            await this.workoutPlansRepository.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> HasUserActivePlan(string userId)
        {
            var userPlans = await this.workoutPlansRepository
                .All()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (userPlans.Any(x => x.ExpireDate.Subtract(DateTime.Now).Hours > 0))
            {
                return true;
            }

            return false;
        }

        private async Task<WorkoutPlan> GenerateShortPlan(GenerateWorkoutPlanInputModel inputModel)
        {
            var workoutPlan = new WorkoutPlan()
            {
                ExpireDate = DateTime.UtcNow.AddDays(inputModel.DurationInDays),
                DaysInWeek = inputModel.WeekdaysCount,
                UserId = inputModel.UserId,
            };

            switch (inputModel.Experience)
            {
                case Experience.Beginner:
                    workoutPlan.Difficulty = Difficulty.Easy;
                    break;
                case Experience.Intermediate:
                    workoutPlan.Difficulty = Difficulty.Medium;
                    break;
                case Experience.Advanced:
                    workoutPlan.Difficulty = Difficulty.Hard;
                    break;
            }

            for (int i = 0; i < inputModel.WeekdaysCount; i++)
            {
                TrainingDay trainingDay = await this.GenerateFullBodyDay(inputModel.Goal, workoutPlan.Difficulty, workoutPlan);

                workoutPlan.TrainingDays.Add(trainingDay);
            }

            return workoutPlan;
        }

        private async Task<TrainingDay> GenerateFullBodyDay(Goal goal, Difficulty difficulty, WorkoutPlan workoutPlan)
        {
            var trainingDay = new TrainingDay()
            {
                Day = Day.Undefined,
                WorkoutPlanId = workoutPlan.Id,
                WorkoutPlan = workoutPlan,
            };

            int exercisesCount;
            List<Exercise> currentDayExercises = new List<Exercise>();
            List<Exercise> exercises = new List<Exercise>();
            exercises.AddRange(workoutPlan.TrainingDays.SelectMany(x => x.Exercises.Select(x => x.Exercise)));
            switch (difficulty)
            {
                case Difficulty.Easy:
                    exercisesCount = 6;
                    break;
                case Difficulty.Medium:
                    exercisesCount = 7;
                    break;
                case Difficulty.Hard:
                    exercisesCount = 8;
                    break;
                default:
                    exercisesCount = 5;
                    break;
            }

            if (goal == Goal.Lose)
            {
                exercisesCount -= 2;
            }

            for (int i = 0; i < exercisesCount; i++)
            {
                var currentExercises = await this.exercisesRepository
                    .All()
                    .Where(x => (x.Type != ExerciseType.Abs
                    && x.Type != ExerciseType.Cardio) &&
                            exercises.Contains(x) == false)
                    .ToListAsync();
                var rand = new Random();
                var currentExercise = currentExercises[rand.Next(0, currentExercises.Count)];
                while (currentDayExercises.Any(x => x.Type == currentExercise.Type))
                {
                    if (i == 6 || i == 7)
                    {
                        currentExercises =
                            currentExercises
                            .Where(x => x.Type == ExerciseType.Chest ||
                            x.Type == ExerciseType.Back ||
                            x.Type == ExerciseType.Legs)
                            .ToList();
                        currentExercise = currentExercises[rand.Next(0, currentExercises.Count)];
                        break;
                    }

                    currentExercise = currentExercises[rand.Next(0, currentExercises.Count)];
                }

                exercises.Add(currentExercise);
                currentDayExercises.Add(currentExercise);

                var workoutTrainingDay = new WorkoutTrainingDay()
                {
                    ExerciseId = currentExercise.Id,
                    Exercise = currentExercise,
                    TrainingDay = trainingDay,
                    TrainingDayId = trainingDay.Id,
                    MinRepsCount = currentExercise.Difficulty == Difficulty.Hard
                    ? 6
                    : currentExercise.Difficulty == Difficulty.Medium
                    ? 8
                    : 10,
                    MaxRepsCount = currentExercise.Difficulty == Difficulty.Hard
                    ? 10
                    : currentExercise.Difficulty == Difficulty.Medium
                    ? 12
                    : 14,
                };

                await this.workoutTrainingDayRepository.AddAsync(workoutTrainingDay);
                trainingDay.Exercises.Add(workoutTrainingDay);
                currentExercise.WorkoutPlans.Add(workoutTrainingDay);
            }

            await this.AddCardio(difficulty, goal, trainingDay);

            await this.trainingDaysRepository.AddAsync(trainingDay);

            return trainingDay;
        }

        private async Task<WorkoutPlan> GenerateLongWeekPlan(GenerateWorkoutPlanInputModel inputModel)
        {
            var workoutPlan = new WorkoutPlan()
            {
                ExpireDate = DateTime.UtcNow.AddDays(inputModel.DurationInDays),
                DaysInWeek = inputModel.WeekdaysCount,
                UserId = inputModel.UserId,
            };

            switch (inputModel.Experience)
            {
                case Experience.Beginner:
                    workoutPlan.Difficulty = Difficulty.Easy;
                    break;
                case Experience.Intermediate:
                    workoutPlan.Difficulty = Difficulty.Medium;
                    break;
                case Experience.Advanced:
                    workoutPlan.Difficulty = Difficulty.Hard;
                    break;
            }

            var days = new List<Day>();
            var muscleGroups = new List<List<ExerciseType>>();
            switch (inputModel.WeekdaysCount)
            {
                case 3:
                    {
                        days.Add(Day.Monday);
                        days.Add(Day.Wednesday);
                        days.Add(Day.Friday);
                        break;
                    }

                case 4:
                    {
                        days.Add(Day.Monday);
                        days.Add(Day.Tuesday);
                        days.Add(Day.Thursday);
                        days.Add(Day.Friday);
                        break;
                    }

                case 5:
                    {
                        days.Add(Day.Monday);
                        days.Add(Day.Tuesday);
                        days.Add(Day.Wednesday);
                        days.Add(Day.Thursday);
                        days.Add(Day.Friday);
                        break;
                    }

                case 6:
                    {
                        days.Add(Day.Monday);
                        days.Add(Day.Tuesday);
                        days.Add(Day.Wednesday);
                        days.Add(Day.Thursday);
                        days.Add(Day.Friday);
                        days.Add(Day.Saturday);
                        break;
                    }

                case 7:
                    {
                        days.Add(Day.Monday);
                        days.Add(Day.Tuesday);
                        days.Add(Day.Wednesday);
                        days.Add(Day.Thursday);
                        days.Add(Day.Friday);
                        days.Add(Day.Saturday);
                        days.Add(Day.Sunday);
                        break;
                    }

                default:
                    break;
            }

            muscleGroups = this.GenerateMuscleGroupsPerDay(days.Count);
            for (int i = 0; i < days.Count; i++)
            {
                TrainingDay trainingDay =
                    await this.GenerateSeparateMusclesDay(inputModel.Goal, workoutPlan.Difficulty, workoutPlan, days[i], muscleGroups[i]);

                workoutPlan.TrainingDays.Add(trainingDay);
            }

            return workoutPlan;
        }

        private List<List<ExerciseType>> GenerateMuscleGroupsPerDay(int count)
        {
            var result = new List<List<ExerciseType>>();
            switch (count)
            {
                case 3:
                    {
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Chest,
                            ExerciseType.Triceps,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Back,
                            ExerciseType.Biceps,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Legs,
                            ExerciseType.Shoulder,
                        });

                        break;
                    }

                case 4:
                    {
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Chest,
                            ExerciseType.Shoulder,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Back,
                            ExerciseType.Abs,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Legs,
                            ExerciseType.Cardio,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Triceps,
                            ExerciseType.Biceps,
                        });

                        break;
                    }

                case 5:
                    {
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Chest,
                            ExerciseType.Back,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Legs,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Biceps,
                            ExerciseType.Triceps,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Shoulder,
                            ExerciseType.Abs,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Chest,
                            ExerciseType.Back,
                        });

                        break;
                    }

                case 6:
                    {
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Chest,
                            ExerciseType.Back,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Legs,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Biceps,
                            ExerciseType.Triceps,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Shoulder,
                            ExerciseType.Back,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Legs,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Chest,
                            ExerciseType.Back,
                        });

                        break;
                    }

                case 7:
                    {
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Chest,
                            ExerciseType.Back,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Legs,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Biceps,
                            ExerciseType.Triceps,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Shoulder,
                            ExerciseType.Back,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Legs,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Back,
                            ExerciseType.Chest,
                        });
                        result.Add(new List<ExerciseType>()
                        {
                            ExerciseType.Cardio,
                        });

                        break;
                    }

                default:
                    result = null;
                    break;
            }

            return result;
        }

        private async Task<TrainingDay> GenerateSeparateMusclesDay(
            Goal goal, Difficulty difficulty, WorkoutPlan workoutPlan, Day day, List<ExerciseType> exercises)
        {
            var trainingDay = new TrainingDay()
            {
                Day = day,
                WorkoutPlan = workoutPlan,
                WorkoutPlanId = workoutPlan.Id,
            };

            var pickedExercises = new List<Exercise>();
            int exercisesCount;
            int firstSet;
            switch (difficulty)
            {
                case Difficulty.Easy:
                    exercisesCount = 5;
                    firstSet = 3;
                    break;
                case Difficulty.Medium:
                    exercisesCount = 6;
                    firstSet = 3;
                    break;
                case Difficulty.Hard:
                    exercisesCount = 7;
                    firstSet = 4;
                    break;
                default:
                    exercisesCount = 12;
                    firstSet = 12;
                    break;
            }

            if (exercises.Count == 1)
            {
                firstSet = exercisesCount;
            }

            for (int i = 0; i < firstSet; i++)
            {
                var currentExercises = await this.exercisesRepository
                    .All()
                    .Where(x => x.Type == exercises[0] && pickedExercises.Contains(x) == false)
                    .ToListAsync();
                if (currentExercises.Any() == false)
                {
                    break;
                }

                var rand = new Random();
                var currentExercise = currentExercises[rand.Next(0, currentExercises.Count)];
                pickedExercises.Add(currentExercise);

                var workoutTrainingDay = new WorkoutTrainingDay()
                {
                    ExerciseId = currentExercise.Id,
                    Exercise = currentExercise,
                    TrainingDay = trainingDay,
                    TrainingDayId = trainingDay.Id,
                    MinRepsCount = currentExercise.Difficulty == Difficulty.Hard
                    ? 6
                    : currentExercise.Difficulty == Difficulty.Medium
                    ? 8
                    : 10,
                    MaxRepsCount = currentExercise.Difficulty == Difficulty.Hard
                    ? 10
                    : currentExercise.Difficulty == Difficulty.Medium
                    ? 12
                    : 14,
                };

                await this.workoutTrainingDayRepository.AddAsync(workoutTrainingDay);
                trainingDay.Exercises.Add(workoutTrainingDay);
                currentExercise.WorkoutPlans.Add(workoutTrainingDay);
            }

            if (exercises.Count == 2)
            {
                int secondSet = exercisesCount - firstSet;
                for (int i = 0; i < secondSet; i++)
                {
                    var currentExercises = await this.exercisesRepository
                        .All()
                        .Where(x => x.Type == exercises[1] && pickedExercises.Contains(x) == false)
                        .ToListAsync();
                    var rand = new Random();
                    var currentExercise = currentExercises[rand.Next(0, currentExercises.Count)];
                    pickedExercises.Add(currentExercise);

                    var workoutTrainingDay = new WorkoutTrainingDay()
                    {
                        ExerciseId = currentExercise.Id,
                        Exercise = currentExercise,
                        TrainingDay = trainingDay,
                        TrainingDayId = trainingDay.Id,
                        MinRepsCount = currentExercise.Difficulty == Difficulty.Hard
                        ? 6
                        : currentExercise.Difficulty == Difficulty.Medium
                        ? 8
                        : 10,
                        MaxRepsCount = currentExercise.Difficulty == Difficulty.Hard
                        ? 10
                        : currentExercise.Difficulty == Difficulty.Medium
                        ? 12
                        : 14,
                    };

                    await this.workoutTrainingDayRepository.AddAsync(workoutTrainingDay);
                    trainingDay.Exercises.Add(workoutTrainingDay);
                    currentExercise.WorkoutPlans.Add(workoutTrainingDay);
                }
            }

            await this.trainingDaysRepository.AddAsync(trainingDay);

            return trainingDay;
        }

        private async Task AddCardio(Difficulty difficulty, Goal goal, TrainingDay trainingDay)
        {
            int exerciseCount = goal == Goal.Lose
                ? 2
                : 1;
            var exercises = new List<Exercise>();
            for (int i = 0; i < exerciseCount; i++)
            {
                var currentExercises = await this.exercisesRepository
                    .All()
                    .Where(x => (x.Type == ExerciseType.Cardio) &&
                            exercises.Contains(x) == false)
                    .ToListAsync();
                var rand = new Random();
                var currentExercise = currentExercises[rand.Next(0, currentExercises.Count)];
                while (goal != Goal.Lose && currentExercise.Difficulty == Difficulty.Hard)
                {
                    currentExercise = currentExercises[rand.Next(0, currentExercises.Count)];
                }

                exercises.Add(currentExercise);
                var workoutTrainingDay = new WorkoutTrainingDay()
                {
                    ExerciseId = currentExercise.Id,
                    Exercise = currentExercise,
                    TrainingDay = trainingDay,
                    TrainingDayId = trainingDay.Id,
                    MinRepsCount = currentExercise.Difficulty == Difficulty.Hard
                    ? 10
                    : currentExercise.Difficulty == Difficulty.Medium
                    ? 15
                    : 20,
                    MaxRepsCount = currentExercise.Difficulty == Difficulty.Hard
                    ? 20
                    : currentExercise.Difficulty == Difficulty.Medium
                    ? 25
                    : 30,
                };

                if (goal != Goal.Lose && currentExercise.Difficulty != Difficulty.Hard)
                {
                    workoutTrainingDay.MinRepsCount -= 5;
                    workoutTrainingDay.MaxRepsCount -= 5;
                }

                await this.workoutTrainingDayRepository.AddAsync(workoutTrainingDay);
                trainingDay.Exercises.Add(workoutTrainingDay);
                currentExercise.WorkoutPlans.Add(workoutTrainingDay);
            }
        }
    }
}
