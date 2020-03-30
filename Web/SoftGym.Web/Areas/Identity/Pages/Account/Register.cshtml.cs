namespace SoftGym.Web.Areas.Identity.Pages.Account
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using SoftGym.Data.Models;
    using SoftGym.Services.Data.Contracts;

    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly SoftGym.Services.Messaging.IEmailSender emailSender;
        private readonly ICloudinaryService cloudinaryService;
        private readonly ICardsService cardService;
        private readonly INotificationsService notificationsService;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            SoftGym.Services.Messaging.IEmailSender emailSender,
            ICloudinaryService cloudinaryService,
            ICardsService cardService,
            INotificationsService notificationsService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._logger = logger;
            this.emailSender = emailSender;
            this.cloudinaryService = cloudinaryService;
            this.cardService = cardService;
            this.notificationsService = notificationsService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [MinLength(3)]
            [MaxLength(25)]
            public string FirstName { get; set; }

            [Required]
            [MinLength(3)]
            [MaxLength(25)]
            public string LastName { get; set; }

            public IFormFile PictureFile { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? this.Url.Content("~/");
            this.ExternalLogins = (await this._signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (this.ModelState.IsValid)
            {

                string profilePicUrl = await this.cloudinaryService.UploudAsync(this.Input.PictureFile);

                var user = new ApplicationUser
                {
                    UserName = this.Input.Email,
                    Email = this.Input.Email,
                    ProfilePictureUrl = profilePicUrl,
                    FirstName = this.Input.FirstName,
                    LastName = this.Input.LastName,
                };
                var card = await this.cardService.GenerateCardAsync(user);
                user.Card = card;
                user.CardId = card.Id;

                var result = await this._userManager.CreateAsync(user, this.Input.Password);
                if (result.Succeeded)
                {
                    await this.notificationsService.CreateNotificationAsync(
                        $"You have your fitness card generated in your profile.",
                        $"/Users/MyCard",
                        user.Id);
                    this._logger.LogInformation("User created a new account with password.");

                    if (this._userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        var token = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
                        var confirmationLink = this.Url.Action("ConfirmEmail", "Account", new { token, email = user.Email }, this.Request.Scheme);

                        await this.emailSender.SendEmailAsync(
                            "michev10@abv.bg",
                            "Plamen Michev",
                            user.Email,
                            "Email Confirmation",
                            $"<h1>Thank you for your registration in SoftGym. Your email conformation link is {confirmationLink}</h1>");

                        return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email });
                    }
                    else
                    {
                        await this._signInManager.SignInAsync(user, isPersistent: false);
                        return this.LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
