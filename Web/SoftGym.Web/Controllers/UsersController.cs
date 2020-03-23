namespace SoftGym.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Users;

    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly ICardsService cardsService;

        public UsersController(
            IUsersService usersService,
            ICloudinaryService cloudinaryService,
            ICardsService cardsService)
        {
            this.usersService = usersService;
            this.cloudinaryService = cloudinaryService;
            this.cardsService = cardsService;
        }

        public async Task<IActionResult> MyCard([FromQuery] bool hasVisits = false)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = claim.Value;

            this.ViewData["hasVisits"] = hasVisits;

            var model = await this.cardsService.GetCardViewModelAsync<MyCardViewModel>(userId);

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(ChangeProfilePhotoInputModel inputModel)
        {
            if (this.ModelState.IsValid && this.cloudinaryService.IsFileValid(inputModel.ProfilePictureFile) == true)
            {
                string newProfilePicUrl = await this.cloudinaryService.UploudAsync(inputModel.ProfilePictureFile);
                await this.usersService.ChangeProfilePhotoAsync(inputModel.UserId, newProfilePicUrl);
            }

            return this.Redirect("/Identity/Account/Manage");
        }

        [HttpPost]
        public IActionResult BuyCard(MyCardViewModel inputModel)
        {
            if (inputModel.Visits != 12 &&
                inputModel.Visits != 16 &&
                inputModel.Visits != 20 &&
                inputModel.Visits != 30)
            {
                return this.Redirect("/Users/MyCard");
            }

            if (this.cardsService.HasCardVisits(inputModel.Id))
            {
                return this.Redirect("/Users/MyCard?hasVisits=true");
            }

            return this.Redirect($"/Paypal/CreatePayment?visits={inputModel.Visits}");
        }
    }
}
