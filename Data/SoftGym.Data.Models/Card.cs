namespace SoftGym.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using SoftGym.Data.Common.Models;

    public class Card : IAuditInfo, IDeletableEntity
    {
        public Card()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        // Aduit Info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string Id { get; set; }

        [Range(0, 35)]
        public int Visits { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        [Required]
        public string PictureUrl { get; set; }
    }
}
