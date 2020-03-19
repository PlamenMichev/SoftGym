namespace SoftGym.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;

    public class PaypalController : BaseController
    {
        private readonly IPaypalService paypalService;

        public PaypalController(IPaypalService paypalService)
        {
            this.paypalService = paypalService;
        }

        public async Task<IActionResult> CreatePayments()
        {
            var result = await this.paypalService.CreatePayment();

            foreach (var link in result.links)
            {
                if (link.rel.Equals("approval_url"))
                {
                    return this.Redirect(link.href);
                }
            }

            return this.NotFound();
        }

        public async Task<IActionResult> SuccessedPayment(string paymentId, string token, string payerId)
        {
            var result = await this.paypalService.ExecutePayment(payerId, paymentId, token);

            return this.Ok(result);
        }
    }
}
