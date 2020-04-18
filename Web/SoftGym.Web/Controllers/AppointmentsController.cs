namespace SoftGym.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Common;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Appointments;
    using SoftGym.Web.ViewModels.Trainers.Appointments;

    public class AppointmentsController : BaseController
    {
        private readonly IAppointmentsService appointmentsService;
        private readonly ITrainersService trainersService;

        public AppointmentsController(
            IAppointmentsService appointmentsService,
            ITrainersService trainersService)
        {
            this.appointmentsService = appointmentsService;
            this.trainersService = trainersService;
        }

        public IActionResult MyAppointments()
        {
            return this.View();
        }

        public async Task<IActionResult> Request()
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new AddAppointmentInputModel()
            {
                TrainersOptions = await this.trainersService
                .GetAllTrainersAsync<TrainerOptionsViewModel>(userId),
            };
            viewModel.ClientId = userId;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Request(AddAppointmentInputModel inputModel)
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!this.ModelState.IsValid)
            {
                inputModel.TrainersOptions = await this.trainersService
                .GetAllTrainersAsync<TrainerOptionsViewModel>(userId);

                return this.View(inputModel);
            }

            if (this.appointmentsService.IsEndTimeSoonerThanStartTime(inputModel.StartTime.Value, inputModel.EndTime.Value))
            {
                this.ModelState.AddModelError("StartTime", "End time should be later than start time!");

                inputModel.TrainersOptions = await this.trainersService
                .GetAllTrainersAsync<TrainerOptionsViewModel>(userId);

                return this.View(inputModel);
            }

            if (this.appointmentsService.IsStartTimePast(inputModel.StartTime.Value))
            {
                this.ModelState.AddModelError("StartTime", "Appointment cannot be in the past!");

                inputModel.TrainersOptions = await this.trainersService
                .GetAllTrainersAsync<TrainerOptionsViewModel>(userId);

                return this.View(inputModel);
            }

            inputModel.IsApproved = false;
            var result = await this.appointmentsService.AddAppoinmentAsync(inputModel);
            if (result == null)
            {
                return this.BadRequest();
            }

            return this.Redirect("/Appointments/MyAppointments");
        }

        public async Task<IActionResult> GetAppointments()
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var viewModel = await this.appointmentsService.GetAppointmentsForClient<ClientSchedulerViewModel>(userId);
            return this.Json(viewModel);
        }

        public async Task<IActionResult> Delete(DeleteInputModel inputModel)
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var isDeleterTrainer = this.User.IsInRole(GlobalConstants.TrainerRoleName);

            await this.appointmentsService.DeleteAppointment(inputModel.AttenderId, userId, inputModel.AppointmentId, isDeleterTrainer);
            return this.Redirect(inputModel.RedirectPath);
        }

        public async Task<IActionResult> AppointmentsList()
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new ListAppointmentsViewModel()
            {
                Appointments = await this.appointmentsService.GetAppointmentsForClient<ListAppointmentViewModel>(userId),
            };

            return this.View(viewModel);
        }
    }
}
