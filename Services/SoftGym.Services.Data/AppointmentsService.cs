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
            };

            trainer.TrainerAppointments.Add(appointment);
            client.ClientAppointments.Add(appointment);

            await this.appointmentsRepository.AddAsync(appointment);
            await this.appointmentsRepository.SaveChangesAsync();

            await this.notificationsService.CreateNotificationAsync(
                $"You have new appointment with {trainer.FirstName} {trainer.LastName} for " +
                $"{appointment.StartTime.Date.ToString("hh:mm")} " +
                $"o'clock in {appointment.StartTime.Date.DayOfWeek}",
                $"/Appointments/MyAppointments/{appointment.Id}",
                client.Id);

            await this.notificationsService.CreateNotificationAsync(
                $"You have new appointment with {client.FirstName} {client.LastName} for " +
                $"{appointment.StartTime.Date.ToString("hh:mm")} " +
                $"o'clock in {appointment.StartTime.Date.DayOfWeek}",
                $"/Appointments/MyAppointments/{appointment.Id}",
                trainer.Id);

            return appointment;
        }

        public async Task<IEnumerable<T>> GetAllAppointmentsForTrainer<T>(string trainerId)
        {
            return await this.appointmentsRepository
                .All()
                .Where(x => x.TrainerId == trainerId)
                .To<T>()
                .ToListAsync();
        }

        public bool IsEndTimeSoonerThanStartTime(DateTime startTime, DateTime endTime)
        {
            var result = endTime.Subtract(startTime).TotalMinutes;
            return endTime.Subtract(startTime).TotalMinutes <= 0;
        }
    }
}
