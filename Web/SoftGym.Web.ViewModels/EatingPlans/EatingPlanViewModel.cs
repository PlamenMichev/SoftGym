namespace SoftGym.Web.ViewModels.EatingPlans
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class EatingPlanViewModel : IMapFrom<EatingPlan>
    {
        public string Id { get; set; }

        public double CaloriesPerDay { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ExpireDate { get; set; }
    }
}
