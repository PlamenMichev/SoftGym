namespace SoftGym.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.WorkoutPlans;

    public class WorkoutPlansController : BaseController
    {
        private readonly IWorkoutPlansService workoutPlansService;

        public WorkoutPlansController(IWorkoutPlansService workoutPlansService)
        {
            this.workoutPlansService = workoutPlansService;
        }

        public IActionResult Index([FromQuery] bool redirected = false)
        {
            if (redirected == true)
            {
                this.ViewData["Redirected"] = true;
            }

            return this.View();
        }

        public async Task<IActionResult> GenerateWorkoutPlan()
        {
            string userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (await this.workoutPlansService.HasUserActivePlan(userId))
            {
                return this.Redirect($"/WorkoutPlans/Index?redirected=true");
            }

            var viewModel = new GenerateWorkoutPlanInputModel();
            viewModel.UserId = userId;
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateWorkoutPlan(GenerateWorkoutPlanInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var plan = await this.workoutPlansService.GenerateWorkoutPlanAsync(inputModel);
            return this.Redirect($"/WorkoutPlans/Details/{plan.Id}");
        }

        public async Task<IActionResult> MyWorkoutPlans()
        {
            string userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new MyWorkoutPlansViewModel
            {
                ActivePlan = await this.workoutPlansService.GetWorkoutPlansAsync<ListWorkoutPlanViewModel>(userId),
                InactivePlans = await this.workoutPlansService.GetWorkoutPlansAsync<ListWorkoutPlanViewModel>(userId),
            };

            viewModel.ActivePlan =
                viewModel
                .ActivePlan
                .Where(x => x.ExpireDate.Subtract(DateTime.UtcNow).Hours > 0);

            viewModel.InactivePlans =
                viewModel
                .InactivePlans
                .Where(x => x.ExpireDate.Subtract(DateTime.UtcNow).Hours < 0);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.workoutPlansService.GetWorkoutPlanAsync<DetailsWorkoutPlanViewModel>(id);
            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            await this.workoutPlansService.DeleteAsync(id);
            return this.Redirect("/WorkoutPlans/MyWorkoutPlans");
        }
    }
}
