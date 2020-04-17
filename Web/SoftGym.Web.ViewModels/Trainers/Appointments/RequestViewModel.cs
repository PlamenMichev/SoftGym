namespace SoftGym.Web.ViewModels.Trainers.Appointments
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class RequestViewModel : IMapFrom<Appointment>
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public string ClientFirstName { get; set; }

        public string ClientLastName { get; set; }

        public string ClientProfilePictureUrl { get; set; }

        public string ClientEmail { get; set; }

        public string ClientPhoneNumber { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public AppointmentType Type { get; set; }

        public string Notes { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
