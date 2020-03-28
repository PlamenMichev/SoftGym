namespace SoftGym.Web.Controllers
{
    using SoftGym.Services.Data.Contracts;

    public class NotificationsController : BaseController
    {
        private readonly INotificationsService notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }
    }
}
