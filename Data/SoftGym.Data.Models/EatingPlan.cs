namespace SoftGym.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using SoftGym.Data.Common.Models;

    public class EatingPlan : IDeletableEntity, IAuditInfo
    {
        public EatingPlan()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Meals = new HashSet<MealPlan>();
        }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string Id { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool IsValid
            => this.CreatedOn.Subtract(this.ExpireDate).TotalHours > 0;

        [Range(0, 15000)]
        public double CaloriesPerDay { get; set; }

        public virtual ICollection<MealPlan> Meals { get; set; }
    }
}
