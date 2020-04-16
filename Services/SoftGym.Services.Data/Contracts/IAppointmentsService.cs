namespace SoftGym.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;
    using SoftGym.Web.ViewModels.Trainers.Appointments;

    public interface IAppointmentsService
    {
        public Task<Appointment> AddAppoinmentAsync(AddAppointmentInputModel inputModel);

        public bool IsEndTimeSoonerThanStartTime(DateTime startTime, DateTime endTime);

        public Task<IEnumerable<T>> GetAllAppointmentsForTrainer<T>(string trainerId);

        public Task<int> GetAppointmentsCountForTrainer(string trainerId);

        public bool IsStartTimePast(DateTime startTime);

        public Task<T> GetAppointmentForUser<T>(int appointmentId);

        public Task<IEnumerable<T>> GetAppointmentsForClient<T>(string clientId);

        public Task<IEnumerable<T>> GetAppointmentRequestsForTrainer<T>(string trainerId);
    }
}
