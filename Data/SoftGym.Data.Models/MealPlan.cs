namespace SoftGym.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MealPlan
    {
        public string EatingPlanId { get; set; }

        public virtual EatingPlan EatingPlan { get; set; }

        public string MealId { get; set; }

        public virtual Meal Meal { get; set; }

        [Range(0, 1500)]
        public double MealWeight { get; set; }

        [Range(0, 5000)]
        public double TotalCalories { get; set; }
    }
}
