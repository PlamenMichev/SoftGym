namespace SoftGym.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;

    public class PaypalController : BaseController
    {
        private readonly IPaypalService paypalService;
        private readonly ICardsService cardsService;

        public PaypalController(
            IPaypalService paypalService,
            ICardsService cardsService)
        {
            this.paypalService = paypalService;
            this.cardsService = cardsService;
        }

        public async Task<IActionResult> CreatePayment([FromQuery] int visits)
        {
            decimal value = this.cardsService.GetPrice(visits);
            var result = await this.paypalService.CreatePayment(value, visits);

            foreach (var link in result.links)
            {
                if (link.rel.Equals("approval_url"))
                {
                    return this.Redirect(link.href);
                }
            }

            return this.NotFound();
        }

        public async Task<IActionResult> SuccessedPayment(string paymentId, string token, string payerId, [FromQuery] int visits)
        {
            var result = await this.paypalService.ExecutePayment(payerId, paymentId, token);
            await this.cardsService.AddVisitsToUser(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, visits);

            return this.View(visits);
        }
    }
}
