namespace SoftGym.Web.ViewModels.Trainers.Meals
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class EditMealInputModel : IMapFrom<Meal>, IHaveCustomMappings
    {
        public string Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Range(0, 10000)]
        [Required(ErrorMessage = "Calories Field is Required.")]
        public double CaloriesPer100Grams { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        public MealType Type { get; set; }

        [AutoMapper.IgnoreMap]
        public IFormFile NewImageFile { get; set; }

        public ICollection<Data.Models.Enums.FoodPreference> FoodPreferences { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Meal, EditMealInputModel>().ForMember(
                   m => m.FoodPreferences,
                   opt => opt.MapFrom(x => x.FoodPreferences.Select(y => y.Preference.Preference)));
        }
    }
}
