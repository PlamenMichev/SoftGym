namespace SoftGym.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MoreLinq;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;
    using SoftGym.Web.ViewModels.Messages;

    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public MessagesService(
            IDeletableEntityRepository<Message> messagesRepository,
            IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.messagesRepository = messagesRepository;
            this.usersRepository = usersRepository;
        }

        public async Task<Message> Create(string senderId, string recieverId, string message)
        {
            var newMessage = new Message()
            {
                SenderId = senderId,
                RecieverId = recieverId,
                Content = message,
            };

            await this.messagesRepository.AddAsync(newMessage);
            await this.messagesRepository.SaveChangesAsync();

            return newMessage;
        }

        public async Task<Message> Delete(int messageId)
        {
            var message = await this.messagesRepository
                .All()
                .FirstAsync(x => x.Id == messageId);

            this.messagesRepository.Delete(message);
            return message;
        }

        public IEnumerable<LatestChatViewModel> GetLatestChatsAsync(string userId)
        {
            var messages = this.messagesRepository
                .All()
                .Where(x => x.RecieverId == userId || x.SenderId == userId)
                .Select(x => new LatestChatViewModel
                {
                    Content = x.Content,
                    RecieverFirstName = x.Reciever.FirstName,
                    RecieverProfilePictureUrl = x.Reciever.ProfilePictureUrl,
                    RecieverId = x.Reciever.Id,
                    SenderId = x.SenderId,
                    CreatedOn = x.CreatedOn,
                    SenderProfilePicture = x.Sender.ProfilePictureUrl,
                    SenderFirstName = x.Sender.FirstName,
                })
                .OrderByDescending(x => x.CreatedOn)
                .DistinctBy(x => x.RecieverId)
                .Take(5)
                .ToList();

            var result = new List<LatestChatViewModel>();
            foreach (var message in messages)
            {
                if (this.messagesRepository.AllAsNoTracking()
                    .Any(y => (y.CreatedOn > message.CreatedOn) && (y.RecieverId == message.RecieverId || y.SenderId == message.RecieverId)) == false)
                {
                    result.Add(message);
                }
            }

            return result;
        }

        public async Task<IEnumerable<T>> GetMessagesAsync<T>(string senderId, string recieverId)
        {
            return await this.messagesRepository
                .All()
                .Where(x => (x.SenderId == senderId && x.RecieverId == recieverId) ||
                (x.SenderId == recieverId && x.RecieverId == senderId))
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();
        }
    }
}
