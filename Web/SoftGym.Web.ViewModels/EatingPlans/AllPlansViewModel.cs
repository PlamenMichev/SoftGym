namespace SoftGym.Web.ViewModels.EatingPlans
{
    using System.Collections.Generic;

    public class AllPlansViewModel
    {
        public ICollection<EatingPlanViewModel> ActivePlans { get; set; }

        public ICollection<EatingPlanViewModel> InactivePlans { get; set; }
    }
}
