namespace SoftGym.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using SoftGym.Data.Models;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedUserAsync(userManager, "admin@admin.com", "Admin", "https://res.cloudinary.com/dzivpr6fj/image/upload/v1585728535/ClubestPics/TrainersCard_ctfm4x.png");
            await SeedUserAsync(userManager, "trainer@trainer.com", "Trainer", "https://res.cloudinary.com/dzivpr6fj/image/upload/v1585728607/ClubestPics/TrainerCard_bzpckd.png");
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, string userEmail, string firstName, string cardUrl)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == userEmail);
            if (user == null)
            {
                var result = new ApplicationUser
                {
                    Email = userEmail,
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAELxw4vfUA5l1hwOrKUTNLvXLOPE+JNLo4//uPS+Yc7erOF9pCK45CCd8Ti64nQTFSQ==",
                    FirstName = firstName,
                    LastName = "Ivanov",
                    UserName = userEmail,
                    ProfilePictureUrl = "https://image.flaticon.com/icons/svg/21/21104.svg",
                };

                var card = new Card
                {
                    Id = cardUrl,
                    Visits = 0,
                    UserId = result.Id,
                    User = result,
                    PictureUrl = cardUrl,
                };
                result.Card = card;
                result.CardId = card.Id;

                var creationResult = await userManager.CreateAsync(result);
            }
        }
    }
}
