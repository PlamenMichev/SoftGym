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
                $"{appointment.StartTime.Hour.ToString("d2")}:{appointment.StartTime.Minute.ToString("d2")} " +
                $"o'clock on {appointment.StartTime.Date.DayOfWeek}",
                $"/Appointments/MyAppointments",
                client.Id);

                await this.notificationsService.CreateNotificationAsync(
                    $"You have new appointment with {client.FirstName} {client.LastName} for " +
                    $"{appointment.StartTime.Hour.ToString("d2")}:{appointment.StartTime.Minute.ToString("d2")} " +
                    $"o'clock on {appointment.StartTime.Date.DayOfWeek}",
                    $"/Trainers/Appointments/",
                    trainer.Id);
            }
            else
            {
                await this.notificationsService.CreateNotificationAsync(
                $"You have sent an appointment request for {appointment.Type.ToString()} " +
                $"with {trainer.FirstName} {trainer.LastName} for " +
                $"{appointment.StartTime.Hour.ToString("d2")}:{appointment.StartTime.Minute.ToString("d2")} " +
                $"o'clock on {appointment.StartTime.Date.DayOfWeek}",
                $"/Appointments/MyAppointments",
                client.Id);

                await this.notificationsService.CreateNotificationAsync(
                $"You have recieved an appointment request for {appointment.Type.ToString()} " +
                $"with {trainer.FirstName} {trainer.LastName} for " +
                $"{appointment.StartTime.Hour.ToString("d2")}:{appointment.StartTime.Minute.ToString("d2")} " +
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

        public async Task<Appointment> DeleteAppointment(
            string appointmentAttenderId,
            string deleterId,
            int appointmentId,
            bool isDeleterTrainer)
        {
            var deleterName = await this.usersRepository
                .All()
                .Where(x => x.Id == deleterId)
                .Select(x => x.FirstName + " " + x.LastName)
                .FirstAsync();

            var appointment = await this.appointmentsRepository
                .All()
                .FirstAsync(x => x.Id == appointmentId);

            this.appointmentsRepository.Delete(appointment);
            await this.appointmentsRepository.SaveChangesAsync();

            var currentUserContent = $"You have cancelled {appointment.Type.ToString()} " +
                    $"on {appointment.StartTime.Day} {appointment.StartTime.ToString("MMMM")}";
            var content = $"{deleterName} has cancelled {appointment.Type.ToString()} " +
                    $"on {appointment.StartTime.Day} {appointment.StartTime.ToString("MMMM")}";

            var trainerUrl = "/Trainers/Appointments/";
            var clientUrl = "/Appointments/MyAppointments";

            if (isDeleterTrainer)
            {
                await this.notificationsService
                                .CreateNotificationAsync(
                                currentUserContent,
                                trainerUrl,
                                deleterId);

                await this.notificationsService
                    .CreateNotificationAsync(
                    content,
                    clientUrl,
                    appointmentAttenderId);
            }
            else
            {
                await this.notificationsService
                                .CreateNotificationAsync(
                                currentUserContent,
                                clientUrl,
                                deleterId);

                await this.notificationsService
                    .CreateNotificationAsync(
                    content,
                    trainerUrl,
                    appointmentAttenderId);
            }

            return appointment;
        }

        public async Task<Appointment> ApproveAppointment(int appointmentId, string trainerId, string clientId)
        {
            var appointment = await this.appointmentsRepository
                .All()
                .FirstAsync(x => x.Id == appointmentId);
            appointment.IsApproved = true;
            await this.appointmentsRepository.SaveChangesAsync();

            var clientFullName = await this.usersRepository
                .All()
                .Where(x => x.Id == clientId)
                .Select(x => x.FirstName + " " + x.LastName)
                .FirstAsync();

            var trainerFullName = await this.usersRepository
                .All()
                .Where(x => x.Id == trainerId)
                .Select(x => x.FirstName + " " + x.LastName)
                .FirstAsync();

            // Send Notifications
            var notificationContentForTrainer = $"You have accepted an appointment request by " +
                $"{clientFullName} for {appointment.Type.ToString()} on " +
                $"{appointment.StartTime.Day} {appointment.StartTime.ToString("MMMM")}";
            var notificationContentForClient = $"{trainerFullName} has accepted an appointment request sent by you" +
                $"for {appointment.Type.ToString()} on " +
                $"{appointment.StartTime.Day} {appointment.StartTime.ToString("MMMM")}";

            await this.notificationsService
                .CreateNotificationAsync(
                notificationContentForTrainer,
                "/Trainers/Appointments/",
                trainerId);

            await this.notificationsService
                .CreateNotificationAsync(
                notificationContentForClient,
                "/Appointments/MyAppointments",
                clientId);

            return appointment;
        }
    }
}
