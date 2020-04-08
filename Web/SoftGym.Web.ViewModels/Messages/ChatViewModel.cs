namespace SoftGym.Web.ViewModels.Messages
{
    using System.Collections.Generic;
    using System.Linq;

    public class ChatViewModel
    {
        public IEnumerable<MessageViewModel> Messages { get; set; }

        public int Count
            => this.Messages.Count();
    }
}
