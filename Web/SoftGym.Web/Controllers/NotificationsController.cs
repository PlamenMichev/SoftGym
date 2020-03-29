namespace SoftGym.Web.Controllers
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Notifications;

    public class NotificationsController : BaseController
    {
        private readonly INotificationsService notificationsService;

        public NotificationsController(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var viewModel = new IndexViewModel()
            {
                Notifications = await this.notificationsService.GetNotifications<NotificationViewModel>(userId),
            };

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SeeAll(IndexViewModel inputModel)
        {
            if (inputModel.UserId != null)
            {
                List<Task> tasks = new List<Task>();
                var notifications = await this.notificationsService.GetFilteredNotifications<NotificationViewModel>(inputModel.UserId, false);
                foreach (var notification in notifications)
                {
                    tasks.Add(this.notificationsService.ReadNotification(notification.Id));
                }

                await Task.WhenAll(tasks);
            }

            return this.Redirect("/Notifications/Index");
        }

        [HttpPost]
        public async Task<IActionResult> See(int notificationId, string url)
        {
            await this.notificationsService.ReadNotification(notificationId);

            return this.Redirect(url);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int notificationId)
        {
            await this.notificationsService.DeleteNotification(notificationId);

            return this.Redirect("/Notifications/Index");
        }
    }
}
