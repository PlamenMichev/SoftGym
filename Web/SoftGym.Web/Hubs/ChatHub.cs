namespace SoftGym.Web.Hubs
{
    using System.Linq;
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using SoftGym.Data.Models;
    using SoftGym.Web.ViewModels.Messages;

    public class ChatHub : Hub
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ChatHub(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task Send(SendMessageInputModel inputModel)
        {
            if (string.IsNullOrEmpty(inputModel.Message) ||
                string.IsNullOrWhiteSpace(inputModel.Message) ||
                string.IsNullOrEmpty(inputModel.UserId))
            {
                return;
            }

            var caller = this.userManager
                .Users
                .First(x => x.Id == inputModel.CallerId)
                .ProfilePictureUrl;

            var sanitizer = new HtmlSanitizer();
            var message = sanitizer.Sanitize(inputModel.Message);
            await this.Clients
                .User(inputModel.UserId)
                .SendAsync("RecieveMessage", message, caller);

            await this.Clients
                .Caller
                .SendAsync("SendMessage", message);
        }
    }
}
