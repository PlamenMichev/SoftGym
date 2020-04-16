namespace SoftGym.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SoftGym.Data.Common.Models;
    using SoftGym.Data.Models.Enums;

    public class Appointment : BaseDeletableModel<int>
    {
        public string TrainerId { get; set; }

        public ApplicationUser Trainer { get; set; }

        public string ClientId { get; set; }

        public ApplicationUser Client { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public AppointmentType Type { get; set; }

        [MaxLength(300)]
        public string Notes { get; set; }

        public bool IsApproved { get; set; }
    }
}
