namespace SoftGym.Data.Models
{
    using System;

    using SoftGym.Data.Common.Models;

    public class ClientTrainer : IDeletableEntity
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public string ClientId { get; set; }

        public virtual ApplicationUser Client { get; set; }

        public string TrainerId { get; set; }

        public virtual ApplicationUser Trainer { get; set; }
    }
}
