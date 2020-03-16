namespace SoftGym.Web.ViewModels.Trainers.Exercises
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class EditExerciseInputModel : IMapFrom<Exercise>
    {
        public string Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Name { get; set; }

        public string VideoUrl { get; set; }

        [AutoMapper.IgnoreMap]
        public IFormFile VideoFile { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        [Required]
        public ExerciseType Type { get; set; }
    }
}
