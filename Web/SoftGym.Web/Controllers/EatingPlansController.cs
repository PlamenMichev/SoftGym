namespace SoftGym.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.EatingPlans;
    using SoftGym.Web.ViewModels.Users;

    [Authorize]
    public class EatingPlansController : BaseController
    {
        private readonly IEatingPlansService eatingPlansService;

        public EatingPlansController(IEatingPlansService eatingPlansService)
        {
            this.eatingPlansService = eatingPlansService;
        }

        public IActionResult Index(bool redirected = false)
        {
            var viewModel = new PlansIndexView()
            {
                Id = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                Redirected = redirected,
            };

            return this.View(viewModel);
        }

        public IActionResult GeneratePlan()
        {
            var viewModel = new GenerateInputModel()
            {
                Id = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                HasUserActivePlan = this.eatingPlansService.HasUserActivePlan(this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value),
            };

            if (viewModel.HasUserActivePlan == true)
            {
                return this.Redirect($"/EatingPlans?redirected=true");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePlan(GenerateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var plan = await this.eatingPlansService.GenerateEatingPlanAsync(inputModel);
            return this.Redirect($"/EatingPlans/MyPlans/{inputModel.Id}");
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.eatingPlansService.GetPlanAsync<EatingPlanDetailsViewModel>(id);
            return this.View(viewModel);
        }

        public async Task<IActionResult> MyPlans(string id)
        {
            var viewModel = new AllPlansViewModel
            {
                ActivePlans = await this.eatingPlansService.GetAllPlansAsync<EatingPlanViewModel>(id),
                InactivePlans = await this.eatingPlansService.GetAllPlansAsync<EatingPlanViewModel>(id),
            };

            viewModel.ActivePlans = viewModel
                .ActivePlans
                .Where(x => x.ExpireDate.Subtract(DateTime.Now).Hours > 0)
                .ToList();
            viewModel.InactivePlans = viewModel
                .InactivePlans
                .Where(x => x.ExpireDate.Subtract(DateTime.Now).Hours < 0)
                .ToList();

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete (string id)
        {
            await this.eatingPlansService.DeletePlanAsync(id);
            return this.Redirect("/EatingPlans/MyPlans");
        }
    }
}
