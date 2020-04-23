namespace SoftGym.Web.ViewComponents
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Messages;

    [ViewComponent(Name = "LatestChats")]
    public class LatestChatsViewComponent : ViewComponent
    {
        private readonly IMessagesService messagesService;

        public LatestChatsViewComponent(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public IViewComponentResult Invoke()
        {
            var userId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var viewModel = new LatestsChatsViewModel()
            {
                Chats = this.messagesService
                .GetLatestChatsAsync(userId),
            };

            return this.View(viewModel);
        }
    }
}
