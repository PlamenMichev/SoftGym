namespace SoftGym.Web.ViewModels.Messages
{
    using System;

    using SoftGym.Data.Models;
    using SoftGym.Services.Mapping;

    public class MessageViewModel : IMapFrom<Message>
    {
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public string SenderId { get; set; }

        public string RecieverId { get; set; }

        public string SenderProfilePictureUrl { get; set; }
    }
}
