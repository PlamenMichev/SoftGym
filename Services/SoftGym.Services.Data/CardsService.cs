namespace SoftGym.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Services.Mapping;

    public class CardsService : ICardsService
    {
        private readonly IDeletableEntityRepository<Card> cardRepository;
        private readonly IQrCodeService qrCodeService;
        private readonly INotificationsService notificationsService;

        public CardsService(
            IDeletableEntityRepository<Card> cardRepository,
            IQrCodeService qrCodeService,
            INotificationsService notificationsService)
        {
            this.cardRepository = cardRepository;
            this.qrCodeService = qrCodeService;
            this.notificationsService = notificationsService;
        }

        public async Task<Card> AddVisitsToUser(string userId, int visits)
        {
            var card = await this.cardRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (card.Visits != 0)
            {
                return card;
            }

            card.Visits = visits;
            await this.cardRepository.SaveChangesAsync();
            await this.notificationsService.CreateNotificationAsync(
                $"You had successfully {visits} visits added to your card.",
                $"/Users/MyCard",
                userId);

            return card;
        }

        public async Task<Card> GenerateCardAsync(ApplicationUser user)
        {
            Card card = new Card
            {
                Visits = 0,
                User = user,
                UserId = user.Id,
            };
            card.PictureUrl = await this.qrCodeService.GenerateQrCodeAsync(user.Id);
            await this.cardRepository.AddAsync(card);

            return card;
        }

        public async Task<T> GetCardViewModelAsync<T>(string userId)
        {
            var result = await this.cardRepository
                .All()
                .Where(x => x.UserId == userId)
                .To<T>()
                .FirstOrDefaultAsync();

            return result;
        }

        public decimal GetPrice(int visits)
        {
            decimal result;
            switch (visits)
            {
                case 12: result = 20; break;
                case 16: result = 26; break;
                case 20: result = 32; break;
                case 30: result = 42; break;
                default:
                    result = 0;
                    break;
            }

            return result;
        }

        public bool HasCardVisits(string id)
        {
            return this.cardRepository
                .All()
                .Where(x => x.Id == id)
                .Any(x => x.Visits > 0);
        }

        public async Task<Card> RemoveVisitFromCard(string cardId)
        {
            var card = await this.cardRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == cardId);

            if (card != null)
            {
                card.Visits -= 1;
                await this.cardRepository.SaveChangesAsync();
                await this.notificationsService.CreateNotificationAsync(
                    $"One visit was removed from your card.",
                    $"/Users/MyCard",
                    card.UserId);
            }

            return card;
        }
    }
}
