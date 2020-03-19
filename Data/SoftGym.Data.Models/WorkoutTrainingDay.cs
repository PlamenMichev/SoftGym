namespace SoftGym.Data.Models
{
    public class WorkoutTrainingDay
    {
        public string ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public string TrainingDayId { get; set; }

        public virtual TrainingDay TrainingDay { get; set; }

        public int MinRepsCount { get; set; }

        public int MaxRepsCount { get; set; }
    }
}
