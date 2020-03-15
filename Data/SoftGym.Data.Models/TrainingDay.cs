namespace SoftGym.Data.Models
{
    using System;
    using System.Collections.Generic;

    using SoftGym.Data.Models.Enums;

    public class TrainingDay
    {
        public TrainingDay()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Exercises = new HashSet<WorkoutExercise>();
        }

        public string Id { get; set; }

        public Day Day { get; set; }

        public string WorkoutPlanId { get; set; }

        public virtual WorkoutPlan WorkoutPlan { get; set; }

        public virtual ICollection<WorkoutExercise> Exercises { get; set; }
    }
}
