namespace SoftGym.Data.Seeding.Dtos
{
    using SoftGym.Data.Models.Enums;

    public class MealDto
    {
        public string Name { get; set; }

        public string PictureUrl { get; set; }

        public MealType Type { get; set; }

        public double CaloriesPer100Grams { get; set; }

        public MealPreferenceDto[] MealsPreferences { get; set; }
    }
}
