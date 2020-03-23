namespace SoftGym.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Administration.Dashboard;

    public class DashboardController : AdministrationController
    {
        private readonly IUsersService usersService;
        private readonly IFacilitiesService facilitiesService;
        private readonly IExercisesService exercisesService;
        private readonly IMealsService mealsService;

        public DashboardController(
            IUsersService usersService,
            IFacilitiesService facilitiesService,
            IExercisesService exercisesService,
            IMealsService mealsService)
        {
            this.usersService = usersService;
            this.facilitiesService = facilitiesService;
            this.exercisesService = exercisesService;
            this.mealsService = mealsService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                UsersCount = await this.usersService.GetUsersCountAsync(),
                FacilitiesCount = await this.facilitiesService.GetFacilitiesCountAsync(),
                ExercisesCount = await this.exercisesService.GetExercisesCountAsync(),
                MealsCount = await this.mealsService.GetMealsCountAsync(),
            };

            return this.View(viewModel);
        }
    }
}
