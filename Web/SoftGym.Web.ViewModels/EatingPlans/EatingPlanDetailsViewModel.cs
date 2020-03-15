namespace SoftGym.Web.ViewModels.EatingPlans
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class EatingPlanDetailsViewModel : IMapFrom<EatingPlan>, IHaveCustomMappings
    {
        public DateTime CreatedOn { get; set; }

        public DateTime ExpireDate { get; set; }

        public double CaloriesPerDay { get; set; }

        public ICollection<Meal> MealEnitities { get; set; }

        public ICollection<MealPlan> Meals { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<EatingPlan, EatingPlanDetailsViewModel>().ForMember(
                m => m.MealEnitities,
                opt => opt.MapFrom(x => x.Meals.Select(y => y.Meal)));
        }
    }
}
