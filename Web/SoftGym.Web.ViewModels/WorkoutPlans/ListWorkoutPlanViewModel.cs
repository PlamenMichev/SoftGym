namespace SoftGym.Web.ViewModels.WorkoutPlans
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class ListWorkoutPlanViewModel : IMapFrom<WorkoutPlan>
    {
        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpireDate { get; set; }

        public int DaysInWeek { get; set; }
    }
}
