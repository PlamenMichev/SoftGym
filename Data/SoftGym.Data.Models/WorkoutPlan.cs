namespace SoftGym.Data.Models
{
    using System;
    using System.Collections.Generic;

    using SoftGym.Data.Common.Models;
    using SoftGym.Data.Models.Enums;

    public class WorkoutPlan : IDeletableEntity, IAuditInfo
    {
        public WorkoutPlan()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Exercises = new HashSet<WorkoutExercise>();
        }

        public string Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsValid
            => this.CreatedOn.Subtract(this.ExpireDate).TotalHours > 0;

        public int DaysInWeek { get; set; }

        public Difficulty Difficulty { get; set; }

        public virtual ICollection<WorkoutExercise> Exercises { get; set; }
    }
}
