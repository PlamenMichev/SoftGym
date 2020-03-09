namespace SoftGym.Web.ViewModels.Trainers.Meals
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class MealViewModel : IMapFrom<Meal>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public double CaloriesPer100Grams { get; set; }

        public string PictureUrl { get; set; }

        public MealType Type { get; set; }

        public ICollection<Data.Models.FoodPreference> FoodPreferences { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meal, MealViewModel>().ForMember(
                   m => m.FoodPreferences,
                   opt => opt.MapFrom(x => x.FoodPreferences.Select(y => y.Preference)));
        }
    }
}
