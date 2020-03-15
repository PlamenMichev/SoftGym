namespace SoftGym.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SoftGym.Data.Common.Models;
    using SoftGym.Data.Models.Enums;

    public class Meal : IDeletableEntity
    {
        public Meal()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Plans = new HashSet<MealPlan>();
            this.FoodPreferences = new HashSet<MealPreference>();
        }

        public string Id { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Range(0, 4000)]
        public double CaloriesPer100Grams { get; set; }

        public virtual ICollection<MealPlan> Plans { get; set; }

        public string PictureUrl { get; set; }

        public MealType Type { get; set; }

        public virtual IEnumerable<MealPreference> FoodPreferences { get; set; }
    }
}
