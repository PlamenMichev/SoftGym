namespace SoftGym.Data.Models
{
    public class WorkoutExercise
    {
        public string ExerciseId { get; set; }

        public virtual Exercise Exercise { get; set; }

        public string WorkoutPlanId { get; set; }

        public virtual WorkoutPlan WorkoutPlan { get; set; }

        public int MinRepsCount { get; set; }

        public int MaxRepsCount { get; set; }
    }
}
