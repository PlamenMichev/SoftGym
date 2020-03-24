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

        public CardsService(
            IDeletableEntityRepository<Card> cardRepository,
            IQrCodeService qrCodeService)
        {
            this.cardRepository = cardRepository;
            this.qrCodeService = qrCodeService;
        }

        public async Task<Card> AddVisitsToUser(string userId, int visits)
        {
            var card = await this.cardRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            card.Visits = visits;
            await this.cardRepository.SaveChangesAsync();

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
            card.PictureUrl = await this.qrCodeService.GenerateQrCodeAsync(card.Id);
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
                    result = 30;
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

            card.Visits -= 1;
            await this.cardRepository.SaveChangesAsync();

            return card;
        }
    }
}
