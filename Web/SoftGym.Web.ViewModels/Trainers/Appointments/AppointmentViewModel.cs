namespace SoftGym.Web.ViewModels.Trainers.Appointments
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class AppointmentViewModel : IMapFrom<Appointment>
    {
        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public AppointmentType Type { get; set; }

        public string Notes { get; set; }
    }
}
