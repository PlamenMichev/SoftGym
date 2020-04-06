namespace SoftGym.Web.Hubs
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using SoftGym.Web.ViewModels.Messages;

    public class ChatHub : Hub
    {
        public async Task Send(SendMessageInputModel inputModel)
        {
            await this.Clients
                .User(inputModel.TrainerId)
                .SendAsync("RecieveMessage", inputModel.Message);

            await this.Clients
                .Caller
                .SendAsync("SendMessage", inputModel.Message);
        }
    }
}
