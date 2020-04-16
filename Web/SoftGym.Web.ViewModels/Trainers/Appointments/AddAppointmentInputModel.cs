namespace SoftGym.Web.ViewModels.Trainers.Appointments
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using SoftGym.Data.Models.Enums;

    public class AddAppointmentInputModel
    {
        [Required(ErrorMessage = "Appointment start time is required")]
        public DateTime? StartTime { get; set; }

        [Required(ErrorMessage = "Appointment end time is required")]
        public DateTime? EndTime { get; set; }

        [Required(ErrorMessage = "Trainer is required")]
        public string TrainerId { get; set; }

        [Required(ErrorMessage = "Client is required")]
        public string ClientId { get; set; }

        [Required(ErrorMessage = "Please input a appointment goal")]
        public AppointmentType Type { get; set; }

        public IEnumerable<ClientOptionsViewModel> ClientsOptions { get; set; }

        public IEnumerable<TrainerOptionsViewModel> TrainersOptions { get; set; }

        public string Notes { get; set; }

        public bool IsApproved { get; set; }
    }
}
