namespace SoftGym.Data.Models
{
    using SoftGym.Data.Common.Models;

    public class Message : BaseDeletableModel<int>
    {
        public string SenderId { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string RecieverId { get; set; }

        public virtual ApplicationUser Reciever { get; set; }

        public string Content { get; set; }
    }
}
