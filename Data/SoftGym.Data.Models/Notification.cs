namespace SoftGym.Data.Models
{
    using SoftGym.Data.Common.Models;

    public class Notification : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public bool IsRead { get; set; }
    }
}
