namespace SoftGym.Data.Models
{
    public class MealPreference
    {
        public string MealId { get; set; }

        public virtual Meal Meal { get; set; }

        public int PreferenceId { get; set; }

        public virtual FoodPreference Preference { get; set; }
    }
}
