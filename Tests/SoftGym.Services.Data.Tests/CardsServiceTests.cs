namespace SoftGym.Services.Data.Tests
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using SoftGym.Data;
    using SoftGym.Data.Common.Repositories;
    using SoftGym.Data.Models;
    using SoftGym.Data.Repositories;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Users;
    using Xunit;

    public class CardsServiceTests
    {
        private readonly Mock<IDeletableEntityRepository<Card>> cardRepository;
        private readonly Mock<IQrCodeService> qrcodeService;
        private readonly Mock<INotificationsService> notificationsService;

        public CardsServiceTests()
        {
            this.cardRepository = new Mock<IDeletableEntityRepository<Card>>();
            this.qrcodeService = new Mock<IQrCodeService>();
            this.notificationsService = new Mock<INotificationsService>();
        }

        [Theory]
        [InlineData(20, 12)]
        [InlineData(26, 16)]
        [InlineData(32, 20)]
        [InlineData(42, 30)]
        [InlineData(0, 300)]
        public void GetPriceShouldReturnCorrectPrice(decimal expectedPrice, int visits)
        {
            var service = new CardsService(this.cardRepository.Object, this.qrcodeService.Object, this.notificationsService.Object);

            var price = service.GetPrice(visits);

            Assert.Equal(expectedPrice, price);
        }

        [Fact]
        public async Task GenerateCardShouldReturnNewCard()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CardsDb").Options;
            var db = new ApplicationDbContext(options);
            var repository = new EfDeletableEntityRepository<Card>(db);

            var service = new CardsService(repository, this.qrcodeService.Object, this.notificationsService.Object);

            var result = await service.GenerateCardAsync(new ApplicationUser());

            Assert.IsType<Card>(result);
        }

        [Fact]
        public async Task GenerateCardShouldHavePropertiesAfterCreation()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CardsDb").Options;
            var db = new ApplicationDbContext(options);
            var repository = new EfDeletableEntityRepository<Card>(db);

            var service = new CardsService(repository, this.qrcodeService.Object, this.notificationsService.Object);

            var result = await service.GenerateCardAsync(new ApplicationUser());

            Assert.NotNull(result.Id);
            Assert.NotNull(result.UserId);
            Assert.Equal(0, result.Visits);
        }

        [Fact]
        public async Task AddVisitsToUserShouldWorkCorrectly()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CardsDb").Options;
            var db = new ApplicationDbContext(options);
            var repository = new EfDeletableEntityRepository<Card>(db);
            var user = new ApplicationUser();
            await repository.AddAsync(new Card()
            {
                User = user,
                UserId = user.Id,
            });
            await repository.SaveChangesAsync();

            var service = new CardsService(repository, this.qrcodeService.Object, this.notificationsService.Object);

            var result = await service.AddVisitsToUser(user.Id, 12);
            var result2 = await service.AddVisitsToUser(user.Id, 12);

            Assert.Equal(12, result.Visits);
            Assert.Equal(12, result2.Visits);
        }

        [Theory]
        [InlineData("plamen")]
        [InlineData(null)]
        [InlineData("")]
        public async Task AddVisitsToUserShouldThrow(string userId)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("CardsDb").Options;
            var db = new ApplicationDbContext(options);
            var repository = new EfDeletableEntityRepository<Card>(db);
            var user = new ApplicationUser();
            await repository.AddAsync(new Card()
            {
                User = user,
                UserId = user.Id,
            });
            await repository.SaveChangesAsync();

            var service = new CardsService(repository, this.qrcodeService.Object, this.notificationsService.Object);

            await Assert.ThrowsAnyAsync<NullReferenceException>(async () => await service.AddVisitsToUser(userId, 12));
        }
    }
}
