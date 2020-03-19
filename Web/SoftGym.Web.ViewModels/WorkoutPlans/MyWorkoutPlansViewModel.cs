namespace SoftGym.Web.ViewModels.WorkoutPlans
{
    using System.Collections.Generic;

    public class MyWorkoutPlansViewModel
    {
        public IEnumerable<ListWorkoutPlanViewModel> ActivePlan { get; set; }

        public IEnumerable<ListWorkoutPlanViewModel> InactivePlans { get; set; }
    }
}
