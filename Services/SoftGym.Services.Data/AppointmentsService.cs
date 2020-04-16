namespace SoftGym.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.Trainers.Appointments;

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IDeletableEntityRepository<Appointment> appointmentsRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly INotificationsService notificationsService;

        public AppointmentsService(
            IDeletableEntityRepository<Appointment> appointmentsRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            INotificationsService notificationsService)
        {
            this.appointmentsRepository = appointmentsRepository;
            this.usersRepository = usersRepository;
            this.notificationsService = notificationsService;
        }

        public async Task<Appointment> AddAppoinmentAsync(AddAppointmentInputModel inputModel)
        {
            var trainer = await this.usersRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == inputModel.TrainerId && x.Roles.Any());

            var client = await this.usersRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == inputModel.ClientId && !x.Roles.Any());

            if (trainer == null || client == null)
            {
                return null;
            }

            var appointment = new Appointment()
            {
                Client = client,
                ClientId = inputModel.ClientId,
                Trainer = trainer,
                TrainerId = inputModel.TrainerId,
                StartTime = inputModel.StartTime.Value,
                EndTime = inputModel.EndTime.Value,
                Type = inputModel.Type,
                Notes = inputModel.Notes,
                IsApproved = inputModel.IsApproved,
            };

            trainer.TrainerAppointments.Add(appointment);
            client.ClientAppointments.Add(appointment);

            await this.appointmentsRepository.AddAsync(appointment);
            await this.appointmentsRepository.SaveChangesAsync();

            if (inputModel.IsApproved)
            {
                await this.notificationsService.CreateNotificationAsync(
                $"You have new appointment with {trainer.FirstName} {trainer.LastName} for " +
                $"{appointment.StartTime.Date.ToString("hh:mm")} " +
                $"o'clock on {appointment.StartTime.Date.DayOfWeek}",
                $"/Appointments/MyAppointments",
                client.Id);

                await this.notificationsService.CreateNotificationAsync(
                    $"You have new appointment with {client.FirstName} {client.LastName} for " +
                    $"{appointment.StartTime.Date.ToString("hh:mm")} " +
                    $"o'clock on {appointment.StartTime.Date.DayOfWeek}",
                    $"/Trainers/Appointments/",
                    trainer.Id);
            }
            else
            {
                await this.notificationsService.CreateNotificationAsync(
                $"You have sent an appointment request for {appointment.Type.ToString()} " +
                $"with {trainer.FirstName} {trainer.LastName} for " +
                $"{appointment.StartTime.Date.ToString("hh:mm")} " +
                $"o'clock on {appointment.StartTime.Date.DayOfWeek}",
                $"#",
                client.Id);

                await this.notificationsService.CreateNotificationAsync(
                $"You have recieved an appointment request for {appointment.Type.ToString()} " +
                $"with {trainer.FirstName} {trainer.LastName} for " +
                $"{appointment.StartTime.Date.ToString("hh:mm")} " +
                $"o'clock on {appointment.StartTime.Date.DayOfWeek}",
                $"/Trainers/Appointments/Requests",
                trainer.Id);
            }

            return appointment;
        }

        public async Task<IEnumerable<T>> GetAllAppointmentsForTrainer<T>(string trainerId)
        {
            return await this.appointmentsRepository
                .All()
                .Where(x => x.TrainerId == trainerId && x.IsApproved == true)
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetAppointmentForUser<T>(int appointmentId)
        {
            var appointment = await this.appointmentsRepository
                .All()
                .Where(x => x.Id == appointmentId)
                .To<T>()
                .FirstOrDefaultAsync();

            return appointment;
        }

        public async Task<IEnumerable<T>> GetAppointmentsForClient<T>(string clientId)
        {
            var appointment = await this.appointmentsRepository
                .All()
                .Where(x => x.ClientId == clientId && x.IsApproved == true)
                .To<T>()
                .ToListAsync();

            return appointment;
        }

        public async Task<int> GetAppointmentsCountForTrainer(string trainerId)
        {
            return await this.appointmentsRepository
                .All()
                .Where(x => x.TrainerId == trainerId && x.IsApproved == true)
                .CountAsync();
        }

        public bool IsEndTimeSoonerThanStartTime(DateTime startTime, DateTime endTime)
        {
            return endTime.Subtract(startTime).TotalMinutes <= 0;
        }

        public bool IsStartTimePast(DateTime startTime)
        {
            return DateTime.Now.Subtract(startTime).TotalMinutes >= 0;
        }

        public async Task<IEnumerable<T>> GetAppointmentRequestsForTrainer<T>(string trainerId)
        {
            return await this.appointmentsRepository
                .All()
                .Where(x => x.TrainerId == trainerId && x.IsApproved == false)
                .To<T>()
                .ToListAsync();
        }
    }
}
