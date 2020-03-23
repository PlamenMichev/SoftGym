namespace SoftGym.Web.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class ChangeProfilePhotoInputModel
    {
        [Required(ErrorMessage = "Please upload new profile picture!")]
        public IFormFile ProfilePictureFile { get; set; }

        public string UserId { get; set; }
    }
}
