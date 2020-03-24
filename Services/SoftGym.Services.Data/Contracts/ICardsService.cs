namespace SoftGym.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using SoftGym.Data.Models;

    public interface ICardsService
    {
        public Task<Card> GenerateCardAsync(ApplicationUser user);

        public Task<T> GetCardViewModelAsync<T>(string userId);

        public decimal GetPrice(int visits);

        public Task<Card> AddVisitsToUser(string userId, int visits);

        public bool HasCardVisits(string id);

        public Task<Card> RemoveVisitFromCard(string cardId);
    }
}
