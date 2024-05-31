using System.ComponentModel.DataAnnotations;

namespace HotPotProject.Models
{
    public class NutritionalInfo : IEquatable<NutritionalInfo>
    {
        [Key]
        public int NutritionId { get; set; }
        public float Calories { get; set; }
        public float Fats { get; set; }
        public float Proteins { get; set; }
        public float Carbohydrates { get; set; }

        public NutritionalInfo()
        {
            NutritionId = 0;
        }

        public NutritionalInfo(int nutritionId, float calories, float fats, float proteins, float carbohydrates)
        {
            NutritionId = nutritionId;
            Calories = calories;
            Fats = fats;
            Proteins = proteins;
            Carbohydrates = carbohydrates;
        }

        public NutritionalInfo(float calories, float fats, float proteins, float carbohydrates)
        {
            Calories = calories;
            Fats = fats;
            Proteins = proteins;
            Carbohydrates = carbohydrates;
        }

        public bool Equals(NutritionalInfo? other)
        {
            var nutritionalInfo = other ?? new NutritionalInfo();
            return this.NutritionId.Equals(nutritionalInfo.NutritionId);
        }
    }
}
