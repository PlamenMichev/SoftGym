namespace SoftGym.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Users;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly ICloudinaryService cloudinaryService;

        public UsersController(IUsersService usersService,
            ICloudinaryService cloudinaryService)
        {
            this.usersService = usersService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(ChangeProfilePhotoInputModel inputModel)
        {
            if (inputModel.ProfilePictureFile != null && this.cloudinaryService.IsFileValid(inputModel.ProfilePictureFile) == true)
            {
                string newProfilePicUrl = await this.cloudinaryService.UploudAsync(inputModel.ProfilePictureFile);
                await this.usersService.ChangeProfilePhotoAsync(inputModel.UserId, newProfilePicUrl);
            }

            return this.Redirect("/Identity/Account/Manage");
        }
    }
}
