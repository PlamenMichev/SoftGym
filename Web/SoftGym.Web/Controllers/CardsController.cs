namespace SoftGym.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Users;

    public class CardsController : BaseController
    {
        private readonly ICardsService cardsService;

        public CardsController(ICardsService cardsService)
        {
            this.cardsService = cardsService;
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
