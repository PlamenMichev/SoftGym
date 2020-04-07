namespace SoftGym.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SoftGym.Data.Models;

    public interface IMessagesService
    {
        public Task<Message> Create(string senderId, string recieverId, string message);

        public Task<Message> Delete(int messageId);

        public Task<IEnumerable<T>> GetMessagesAsync<T>(string senderId, string recieverId);
    }
}
