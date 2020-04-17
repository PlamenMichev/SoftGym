namespace SoftGym.Web.Areas.Trainers.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Trainers.Appointments;

    public class AppointmentsController : TrainersController
    {
        private readonly IAppointmentsService appointmentsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;

        public AppointmentsController(
            IAppointmentsService appointmentsService,
            UserManager<ApplicationUser> userManager,
            IUsersService usersService)
        {
            this.appointmentsService = appointmentsService;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Add()
        {
            var trainer = await this.userManager.GetUserAsync(this.User);
            string trainerId = await this.userManager.GetUserIdAsync(trainer);
            var viewModel = new AddAppointmentInputModel()
            {
                ClientsOptions = await this.usersService
                .GetAllUsersAsync<ClientOptionsViewModel>(trainerId),
            };
            viewModel.TrainerId = trainerId;

            return this.View(viewModel);
        }

        public async Task<IActionResult> Requests()
        {
            var trainer = await this.userManager.GetUserAsync(this.User);
            string trainerId = await this.userManager.GetUserIdAsync(trainer);
            var viewModel = new RequestsViewModel()
            {
                Requests = await this.appointmentsService
                .GetAppointmentRequestsForTrainer<RequestViewModel>(trainerId),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> GetAppointments()
        {
            var trainer = await this.userManager.GetUserAsync(this.User);
            string trainerId = await this.userManager.GetUserIdAsync(trainer);

            var viewModel = await this.appointmentsService.GetAllAppointmentsForTrainer<AppointmentViewModel>(trainerId);
            return this.Json(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAppointmentInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                inputModel.ClientsOptions = await this.usersService
                .GetAllUsersAsync<ClientOptionsViewModel>(inputModel.TrainerId);

                return this.View(inputModel);
            }

            if (this.appointmentsService.IsEndTimeSoonerThanStartTime(inputModel.StartTime.Value, inputModel.EndTime.Value))
            {
                this.ModelState.AddModelError("StartTime", "End time should be later than start time!");

                inputModel.ClientsOptions = await this.usersService
                .GetAllUsersAsync<ClientOptionsViewModel>(inputModel.TrainerId);

                return this.View(inputModel);
            }

            if (this.appointmentsService.IsStartTimePast(inputModel.StartTime.Value))
            {
                this.ModelState.AddModelError("StartTime", "Appointment cannot be in the past!");

                inputModel.ClientsOptions = await this.usersService
                .GetAllUsersAsync<ClientOptionsViewModel>(inputModel.TrainerId);

                return this.View(inputModel);
            }

            inputModel.IsApproved = true;
            var result = await this.appointmentsService.AddAppoinmentAsync(inputModel);
            if (result == null)
            {
                return this.BadRequest();
            }

            return this.Redirect("/Trainers/Dashboard/Index");
        }

        public async Task<IActionResult> Approve(ApproveInputModel inputModel)
        {
            var trainer = await this.userManager.GetUserAsync(this.User);
            string trainerId = await this.userManager.GetUserIdAsync(trainer);

            await this.appointmentsService.ApproveAppointment(inputModel.AppointmentId, trainerId, inputModel.ClientId);

            return this.Redirect("/Trainers/Appointments/");
        }
    }
}
