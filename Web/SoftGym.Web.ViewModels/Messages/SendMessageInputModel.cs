namespace SoftGym.Web.ViewModels.Messages
{
    using System.ComponentModel.DataAnnotations;

    public class SendMessageInputModel
    {
        [Required]
        public string Message { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
