namespace SoftGym.Web.ViewModels.Trainers.Exercises
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using SoftGym.Data.Models.Enums;

    public class AddExerciseInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public IFormFile VideoFile { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        [Required]
        public ExerciseType Type { get; set; }
    }
}
