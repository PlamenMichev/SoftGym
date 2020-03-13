namespace SoftGym.Web.ViewModels.EatingPlans
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SoftGym.Data.Models.Enums;
    using SoftGym.Web.ViewModels.EatingPlans.Enums;

    public class GenerateInputModel
    {
        public string Id { get; set; }

        [Range(1, 1000, ErrorMessage = "Enter valid weight!")]
        public double Weight { get; set; }

        [Range(1, 300, ErrorMessage = "Enter valid height!")]
        public double Height { get; set; }

        [Range(1, 150, ErrorMessage = "Enter valid age!")]
        public double Age { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public Goal Goal { get; set; }

        [Required]
        public int DurationInDays { get; set; }

        [Required]
        public string Activity { get; set; }

        public IEnumerable<FoodPreference> FoodPreferences { get; set; } = new HashSet<FoodPreference>();
    }
}
