namespace SoftGym.Data.Models
{
    using System.Collections.Generic;

    public class FoodPreference
    {
        public FoodPreference()
        {
            this.Meals = new HashSet<MealPreference>();
        }

        public int Id { get; set; }

        public Data.Models.Enums.FoodPreference Preference { get; set; }

        public virtual IEnumerable<MealPreference> Meals { get; set; }
    }
}
