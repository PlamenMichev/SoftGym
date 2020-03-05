namespace SoftGym.Services.Data
{
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;
    using System.Threading.Tasks;

    public class CardService : ICardService
    {
        private readonly IDeletableEntityRepository<Card> cardRepository;
        private readonly IQrCodeService qrCodeService;

        public CardService(
            IDeletableEntityRepository<Card> cardRepository,
            IQrCodeService qrCodeService)
        {
            this.cardRepository = cardRepository;
            this.qrCodeService = qrCodeService;
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
    }
}
