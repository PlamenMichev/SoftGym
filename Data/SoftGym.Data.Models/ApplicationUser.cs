// ReSharper disable VirtualMemberCallInConstructor
namespace SoftGym.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;
    using SoftGym.Data.Common.Models;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Trainers = new HashSet<ClientTrainer>();
            this.Clients = new HashSet<ClientTrainer>();
            this.EatingPlans = new HashSet<EatingPlan>();
            this.WorkoutPlans = new HashSet<WorkoutPlan>();
            this.ClientAppointments = new HashSet<Appointment>();
            this.TrainerAppointments = new HashSet<Appointment>();
            this.RestDays = new HashSet<DayOfWeek>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(25)]
        public string LastName { get; set; }

        public string CardId { get; set; }

        public virtual Card Card { get; set; }

        public string Description { get; set; }

        public string ProfilePictureUrl { get; set; }

        [NotMapped]
        public virtual ICollection<DayOfWeek> RestDays { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<ClientTrainer> Trainers { get; set; }

        public virtual ICollection<ClientTrainer> Clients { get; set; }

        public virtual ICollection<EatingPlan> EatingPlans { get; set; }

        public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; }

        public virtual ICollection<Appointment> TrainerAppointments { get; set; }

        public virtual ICollection<Appointment> ClientAppointments { get; set; }
    }
}
