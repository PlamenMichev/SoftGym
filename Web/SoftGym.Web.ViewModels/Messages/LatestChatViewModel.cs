namespace SoftGym.Web.ViewModels.Messages
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class LatestChatViewModel : IMapFrom<Message>
    {
        public string RecieverId { get; set; }

        public string RecieverProfilePictureUrl { get; set; }

        public string RecieverFirstName { get; set; }

        public string Content { get; set; }

        public string SenderId { get; set; }

        public string SenderFirstName { get; set; }

        public string SenderProfilePicture { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Message
            => this.Content.Substring(0, this.Content.Length > 21 ? 21 : this.Content.Length) + (this.Content.Length > 21 ? "..." : "");
    }
}
