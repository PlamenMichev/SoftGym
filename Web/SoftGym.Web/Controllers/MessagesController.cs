namespace SoftGym.Web.Controllers
{
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Ganss.XSS;
    using Microsoft.AspNetCore.Mvc;
    using SoftGym.Services.Data.Contracts;
    using SoftGym.Web.ViewModels.Messages;

    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : BaseController
    {
        private readonly IMessagesService messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SendMessageInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.NoContent();
            }

            var sanitizer = new HtmlSanitizer();
            var message = sanitizer.Sanitize(inputModel.Message);
            var senderId = this.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var model = await this.messagesService.Create(senderId, inputModel.UserId, message);

            return this.Json(model);
        }
    }
}
