namespace SoftGym.Web.ViewModels.WorkoutPlans
{
    using System.ComponentModel.DataAnnotations;

    using SoftGym.Web.ViewModels.EatingPlans.Enums;
    using SoftGym.Web.ViewModels.WorkoutPlans.Enums;

    public class GenerateWorkoutPlanInputModel
    {
        public string UserId { get; set; }

        [Required]
        public Experience Experience { get; set; }

        [Range(1, 7, ErrorMessage = "Days in week are between 1 and 7")]
        public int WeekdaysCount { get; set; }

        [Required]
        public Goal Goal { get; set; }

        [Required]
        [Range(30, 90, ErrorMessage = "Please enter valid time period!")]
        public int DurationInDays { get; set; }
    }
}
