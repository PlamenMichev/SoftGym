namespace SoftGym.Web.ViewModels.Notifications
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<NotificationViewModel> Notifications { get; set; }

        public string UserId { get; set; }
    }
}
