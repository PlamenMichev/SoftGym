namespace SoftGym.Web.ViewModels.Users
{
    using Microsoft.AspNetCore.Http;

    public class ChangeProfilePhotoInputModel
    {
        public IFormFile ProfilePictureFile { get; set; }

        public string UserId { get; set; }
    }
}
