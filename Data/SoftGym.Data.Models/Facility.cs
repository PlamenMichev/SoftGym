namespace SoftGym.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using SoftGym.Data.Common.Models;
    using SoftGym.Data.Models.Enums;

    public class Facility : IDeletableEntity, IAuditInfo
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string PictureUrl { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(400)]
        public string Description { get; set; }

        public FacilityType Type { get; set; }
    }
}
