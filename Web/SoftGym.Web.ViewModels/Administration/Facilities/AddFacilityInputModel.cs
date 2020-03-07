namespace SoftGym.Web.ViewModels.Administration.Facilities
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using SoftGym.Data.Models.Enums;

    public class AddFacilityInputModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Picture is required.")]
        public IFormFile PictureFile { get; set; }

        [Required]
        public FacilityType Type { get; set; }
    }
}
