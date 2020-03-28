namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;

    public interface INotificationsService
    {
        public Task<Notification> CreateNotificationAsync(string content, string url, string userId = null);

        public Task<IEnumerable<T>> GetNotifications<T>(string userId);

        public Task<Notification> DeleteNotification(int notificationId);

        public Task<int> GetNotificationsCount(string userId);

        public Task ReadNotification(int notificationId);
    }
}
