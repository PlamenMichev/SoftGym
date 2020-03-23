namespace SoftGym.Web.ViewModels.Trainers.Exercises
{
    using System.Collections.Generic;

    using SoftGym.Data.Models.Enums;

    public class AllExercisesViewModel
    {
        public IEnumerable<ExerciseViewModel> Exercises { get; set; }

        public string ExerciseType { get; set; }
    }
}
