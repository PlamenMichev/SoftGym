namespace SoftGym.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.EatingPlans;

    [Authorize]
    public class EatingPlansController : BaseController
    {
        private readonly IEatingPlansService eatingPlansService;

        public EatingPlansController(IEatingPlansService eatingPlansService)
        {
            this.eatingPlansService = eatingPlansService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult GeneratePlan()
        {
            var viewModel = new GenerateInputModel()
            {
                Id = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlan(GenerateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.eatingPlansService.GenerateEatingPlanAsync(inputModel);
            return this.Redirect($"/EatingPlans/MyPlans/{inputModel.Id}");
        }
    }
}
