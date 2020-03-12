namespace SoftGym.Web.ViewModels.Trainers.Meals
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using SoftGym.Data.Models.Enums;

    public class AddMealInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Range(1, 10000, ErrorMessage = "Calories must be between 1 and 1000")]
        [Required(ErrorMessage = "Calories Field is Required.")]
        public double CaloriesPer100Grams { get; set; }

        [Required]
        public IFormFile PictureFile { get; set; }

        [Required(ErrorMessage = "Meal Type field is Required.")]
        public MealType Type { get; set; }

        public IEnumerable<FoodPreference> FoodPreferences { get; set; }
    }
}
