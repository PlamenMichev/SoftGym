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
            this.TrainingDays = new HashSet<TrainingDay>();
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

        public virtual ICollection<TrainingDay> TrainingDays { get; set; }

        public Difficulty Difficulty { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
