namespace SoftGym.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;

    public class NotificationsService : INotificationsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationsService(
            IDeletableEntityRepository<Notification> notificationsRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.notificationsRepository = notificationsRepository;
            this.userManager = userManager;
        }

        public async Task<Notification> CreateNotificationAsync(string content, string url, string userId = null)
        {
            if (userId != null)
            {
                var notification = new Notification()
                {
                    Content = content,
                    Url = url,
                    UserId = userId,
                };

                await this.notificationsRepository.AddAsync(notification);
                await this.notificationsRepository.SaveChangesAsync();

                return notification;
            }
            else
            {
                foreach (var user in this.userManager.Users)
                {
                    var notification = new Notification()
                    {
                        Content = content,
                        Url = url,
                        UserId = user.Id,
                    };

                    await this.notificationsRepository.AddAsync(notification);
                }

                await this.notificationsRepository.SaveChangesAsync();

                return await this.notificationsRepository.All().FirstAsync();
            }
        }

        public async Task<Notification> DeleteNotification(int notificationId)
        {
            var notification = await this.notificationsRepository
                .All()
                .FirstAsync(x => x.Id == notificationId);

            this.notificationsRepository.Delete(notification);
            await this.notificationsRepository.SaveChangesAsync();

            return notification;
        }

        public async Task<IEnumerable<T>> GetFilteredNotifications<T>(string userId, bool isRead)
        {
            return await this.notificationsRepository
                .All()
                .Where(x => x.UserId == userId && x.IsRead == isRead)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetNewNotificationsCount(string userId)
        {
            return await this.notificationsRepository
                .All()
                .Where(x => x.UserId == userId && x.IsRead == false)
                .CountAsync();
        }

        public async Task<IEnumerable<T>> GetNotifications<T>(string userId)
        {
            return await this.notificationsRepository
                .All()
                .Where(x => x.UserId == userId)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetNotificationsCount(string userId)
        {
            return await this.notificationsRepository
                .All()
                .Where(x => x.UserId == userId)
                .CountAsync();
        }

        public async Task ReadNotification(int notificationId)
        {
            var notification = await this.notificationsRepository
                .All()
                .FirstAsync(x => x.Id == notificationId);

            if (notification.IsRead == false)
            {
                notification.IsRead = true;
            }

            await this.notificationsRepository.SaveChangesAsync();
        }
    }
}
