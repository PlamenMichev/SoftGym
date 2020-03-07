namespace SoftGym.Web.ViewModels.Administration.Facilities
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class EditViewModel : IMapFrom<Facility>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(300)]
        public string Description { get; set; }

        public string PictureUrl { get; set; }

        [AutoMapper.IgnoreMap]
        public IFormFile NewPictureFile { get; set; }

        [Required]
        public FacilityType Type { get; set; }
    }
}
