namespace SoftGym.Web.Areas.Trainers.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Trainers;

    public class ClientsController : TrainersController
    {
        private readonly ITrainersService trainersService;

        public ClientsController(ITrainersService trainersService)
        {
            this.trainersService = trainersService;
        }

        public async Task<IActionResult> MyClients()
        {
            var trainerId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var viewModel = new MyTrainersViewModel()
            {
                Clients = await this.trainersService.GetClientsForTrainer<MyClientViewModel>(trainerId),
            };

            return this.View("MyTrainers", viewModel);
        }
    }
}
