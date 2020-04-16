namespace SoftGym.Web.ViewModels.Appointments
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class ClientSchedulerViewModel : IMapFrom<Appointment>
    {
        public string TrainerFirstName { get; set; }

        public string TrainerLastName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public AppointmentType Type { get; set; }

        public string Notes { get; set; }
    }
}
