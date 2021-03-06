﻿namespace SoftGym.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Contracts;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.Trainers.Exercises;

    public class ExercisesService : IExercisesService
    {
        private readonly IDeletableEntityRepository<Exercise> exercisesRepository;
        private readonly ICloudinaryService cloudinaryService;

        public ExercisesService(
            IDeletableEntityRepository<Exercise> exercisesRepository,
            ICloudinaryService cloudinaryService)
        {
            this.exercisesRepository = exercisesRepository;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<Exercise> AddExerciseAsync(AddExerciseInputModel inputModel)
        {
            Exercise exercise = new Exercise()
            {
                Name = inputModel.Name,
                Type = inputModel.Type,
                Difficulty = inputModel.Difficulty,
                VideoUrl = await this.cloudinaryService.UploadVideoAsync(inputModel.VideoFile),
            };

            await this.exercisesRepository.AddAsync(exercise);
            await this.exercisesRepository.SaveChangesAsync();

            return exercise;
        }

        public async Task<Exercise> DeleteExerciseAsync(string exerciseId)
        {
            var exercise = await this.exercisesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == exerciseId);

            this.exercisesRepository.Delete(exercise);
            await this.exercisesRepository.SaveChangesAsync();

            return exercise;
        }

        public async Task<Exercise> EditExerciseAsync(EditExerciseInputModel inputModel)
        {
            var exercise = await this.exercisesRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == inputModel.Id);

            exercise.Name = inputModel.Name;
            exercise.Type = inputModel.Type;
            exercise.Difficulty = inputModel.Difficulty;
            exercise.VideoUrl = inputModel.VideoFile ==
                null || !this.cloudinaryService.IsVideoFileValid(inputModel.VideoFile)
                ? exercise.VideoUrl
                : await this.cloudinaryService.UploadVideoAsync(inputModel.VideoFile);

            await this.exercisesRepository.SaveChangesAsync();
            return exercise;
        }

        public async Task<IEnumerable<T>> GetAllExercisesAsync<T>(string filterType = null)
        {
            if (filterType == null)
            {
                return await this.exercisesRepository
                    .All()
                    .To<T>()
                    .ToListAsync();
            }
            else
            {
                ExerciseType currentType = (ExerciseType)int.Parse(filterType);
                return await this.exercisesRepository
                    .All()
                    .Where(x => x.Type == currentType)
                    .To<T>()
                    .ToListAsync();
            }
        }

        public async Task<T> GetExerciseAsync<T>(string exerciseId)
        {
            return await this.exercisesRepository
                .All()
                .Where(x => x.Id == exerciseId)
                .To<T>()
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetExercisesCountAsync()
        {
            return await this.exercisesRepository
                .All()
                .CountAsync();
        }
    }
}
