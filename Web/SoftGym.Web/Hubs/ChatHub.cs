namespace SoftGym.Web.Hubs
{
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Microsoft.AspNetCore.SignalR;
    using SoftGym.Web.ViewModels.Messages;

    public class ChatHub : Hub
    {
        public async Task Send(SendMessageInputModel inputModel)
        {
            if (string.IsNullOrEmpty(inputModel.Message) ||
                string.IsNullOrWhiteSpace(inputModel.Message) ||
                string.IsNullOrEmpty(inputModel.UserId))
            {
                return;
            }

            var sanitizer = new HtmlSanitizer();
            var message = sanitizer.Sanitize(inputModel.Message);
            await this.Clients
                .User(inputModel.UserId)
                .SendAsync("RecieveMessage", message);

            await this.Clients
                .Caller
                .SendAsync("SendMessage", message);
        }
    }
}
