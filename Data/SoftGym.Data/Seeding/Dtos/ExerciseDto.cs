namespace SoftGym.Data.Seeding.Dtos
{
    using SoftGym.Data.Models.Enums;

    public class ExerciseDto
    {
        public string Name { get; set; }

        public string VideoUrl { get; set; }

        public ExerciseType Type { get; set; }

        public Difficulty Difficulty { get; set; }
    }
}
