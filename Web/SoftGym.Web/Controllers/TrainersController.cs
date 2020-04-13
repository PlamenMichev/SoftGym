namespace SoftGym.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Common;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Messaging;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTrainer(string id)
        {
            var clientId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await this.trainersService.AddClientToTrainer(clientId, id);

            return this.Redirect("/Trainers/MyTrainers");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveTrainer(string id)
        {
            var clientId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            await this.trainersService.RemoveClientFromTrainer(clientId, id);

            return this.Redirect("/Trainers/MyTrainers");
        }

        [Authorize]
        public async Task<IActionResult> MyTrainers()
        {
            var clientId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new MyTrainersViewModel()
            {
                Trainers = await this.trainersService.GetAllTrainersAsync<MyTrainerViewModel>(clientId),
            };

            return this.View(viewModel);
        }
    }
}
