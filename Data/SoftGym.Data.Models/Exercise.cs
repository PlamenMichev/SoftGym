namespace SoftGym.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using SoftGym.Data.Common.Models;
    using SoftGym.Data.Models.Enums;

    public class Exercise : IDeletableEntity
    {
        public Exercise()
        {
            this.Id = Guid.NewGuid().ToString();
            this.WorkoutPlans = new HashSet<WorkoutExercise>();
        }

        public string Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public string VideoUrl { get; set; }

        public Difficulty Difficulty { get; set; }

        public virtual ICollection<WorkoutExercise> WorkoutPlans { get; set; }

        public ExerciseType Type { get; set; }
    }
}
