namespace SoftGym.Web.ViewModels.Notifications
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Url { get; set; }

        public bool IsRead { get; set; }

        public string UserId { get; set; }
    }
}
