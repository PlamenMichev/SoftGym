namespace SoftGym.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Trainers;

    [AllowAnonymousAttribute]
    public class TrainersController : BaseController
    {
        private readonly ITrainersService trainersService;

        public TrainersController(ITrainersService trainersService)
        {
            this.trainersService = trainersService;
        }

        public async Task<IActionResult> All()
        {
            var viewModel = new AllTrainersViewModel
            {
                Trainers = await this.trainersService.GetAllTrainersAsync<TrainerViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}
