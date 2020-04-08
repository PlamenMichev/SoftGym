namespace SoftGym.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;

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

        public async Task<IEnumerable<T>> GetMessagesAsync<T>(string senderId, string recieverId)
        {
            return await this.messagesRepository
                .All()
                .Where(x => x.SenderId == senderId && x.RecieverId == recieverId)
                .OrderBy(x => x.CreatedOn)
                .To<T>()
                .ToListAsync();
        }
    }
}
