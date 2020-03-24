namespace SoftGym.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Administration.Cards;

    public class CardsController : AdministrationController
    {
        private readonly ICardsService cardsService;

        public CardsController(ICardsService cardsService)
        {
            this.cardsService = cardsService;
        }

        [HttpPost]
        public async Task<IActionResult> AddVisits(UserCardViewModel inputModel)
        {
            if (inputModel.Visits != 12 &&
                inputModel.Visits != 16 &&
                inputModel.Visits != 20 &&
                inputModel.Visits != 30)
            {
                return this.Redirect("/Administration/Cards/UserCard");
            }

            if (this.cardsService.HasCardVisits(inputModel.Id))
            {
                return this.Redirect($"/Administration/Cards/UserCard?hasVisits=true&userId={inputModel.UserId}");
            }

            await this.cardsService.AddVisitsToUser(inputModel.UserId, inputModel.Visits);
            return this.Redirect($"/Administration/Cards/AddSuccess?userId={inputModel.UserId}");
        }

        public async Task<IActionResult> UserCard(string userId, [FromQuery] bool hasVisits = false, [FromQuery] bool noVisits = false)
        {
            this.ViewData["hasVisits"] = hasVisits;
            this.ViewData["noVisits"] = noVisits;

            var viewModel = await this.cardsService.GetCardViewModelAsync<UserCardViewModel>(userId);

            return this.View(viewModel);
        }

        public IActionResult AddSuccess([FromQuery] string userId)
        {
            return this.View("AddSuccess", userId);
        }

        public IActionResult RemoveSuccess([FromQuery] string userId)
        {
            return this.View("RemoveSuccess", userId);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveVisit(UserCardViewModel inputModel)
        {
            if (this.cardsService.HasCardVisits(inputModel.Id) == false)
            {
                return this.Redirect($"/Administration/Cards/UserCard?noVisits=true&userId={inputModel.UserId}");
            }

            await this.cardsService.RemoveVisitFromCard(inputModel.Id);
            return this.Redirect($"/Administration/Cards/RemoveSuccess?userId={inputModel.UserId}");
        }
    }
}
