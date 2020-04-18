namespace SoftGym.Web.ViewModels.Trainers.Appointments
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Data.Models.Enums;
    using SoftGym.Services.Mapping;

    public class ListAppointmentViewModel : IMapFrom<Appointment>
    {
        public int Id { get; set; }

        public string TrainerId { get; set; }

        public DateTime StartTime { get; set; }

        public AppointmentType Type { get; set; }

        public string TrainerFirstName { get; set; }

        public string TrainerLastName { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
