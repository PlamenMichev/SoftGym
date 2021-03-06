﻿namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Web.ViewModels.Trainers.Exercises;

    public interface IExercisesService
    {
        public Task<Exercise> AddExerciseAsync(AddExerciseInputModel inputModel);

        public Task<IEnumerable<T>> GetAllExercisesAsync<T>(string filterType = null);

        public Task<T> GetExerciseAsync<T>(string exerciseId);

        public Task<Exercise> DeleteExerciseAsync(string exerciseId);

        public Task<Exercise> EditExerciseAsync(EditExerciseInputModel inputModel);

        public Task<int> GetExercisesCountAsync();
    }
}
