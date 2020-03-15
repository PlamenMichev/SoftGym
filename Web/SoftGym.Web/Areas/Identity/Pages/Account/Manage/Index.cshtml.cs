using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SoftGym.Common;
using SoftGym.Data.Models;
using SoftGym.Services.Data.Contracts;
using SoftGym.Web.ViewModels.Users;

namespace SoftGym.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUsersService usersService;
        private readonly ITrainersService trainersService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUsersService usersService,
            ITrainersService trainersService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.usersService = usersService;
            this.trainersService = trainersService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name")]
            [Required]
            [MinLength(3)]
            [MaxLength(25)]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            [Required]
            [MinLength(3)]
            [MaxLength(25)]
            public string LastName { get; set; }

            public string ProfilePictureUrl { get; set; }

            [Required]
            [MinLength(20)]
            [MaxLength(300)]
            public string Description { get; set; }

            public ChangeProfilePhotoInputModel PhotoModel { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                PhotoModel = new ChangeProfilePhotoInputModel
                {
                    UserId = user.Id,
                },
                ProfilePictureUrl = user.ProfilePictureUrl,
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
            {
                this.Input.Description = user.Description;
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            if (this.User.IsInRole(GlobalConstants.TrainerRoleName) && user.Description != null)
            {
                this.Input.Description = user.Description;
            }

            this.Input.FirstName = user.FirstName;
            this.Input.LastName = user.LastName;
            this.Input.PhoneNumber = user.PhoneNumber;
            this.Input.ProfilePictureUrl = await this.usersService.GetProfilePictureUrlAsync(user.Id);
            this.Input.PhotoModel = new ChangeProfilePhotoInputModel
            {
                UserId = user.Id,
            };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                if (!this.User.IsInRole(GlobalConstants.TrainerRoleName)
                    && this.ModelState.ErrorCount > 1)
                {
                    await LoadAsync(user);
                    return Page();
                }
                else if (this.User.IsInRole(GlobalConstants.TrainerRoleName))
                {
                    await LoadAsync(user);
                    return Page();
                }
            }

            var phoneNumber = await this._userManager.GetPhoneNumberAsync(user);
            var firstName = await this.usersService.GetFirstNameAsync(user.Id);
            var lastName = await this.usersService.GetLastNameAsync(user.Id);
            if (this.Input.FirstName != firstName)
            {
                await this.usersService.ChangeFirstNameAsync(user.Id, this.Input.FirstName);
            }

            if (this.Input.LastName != lastName)
            {
                await this.usersService.ChangeLastNameAsync(user.Id, this.Input.LastName);
            }

            if (this.User.IsInRole(GlobalConstants.TrainerRoleName) && user?.Description != this.Input.Description)
            {
                await this.trainersService.ChangeDescriptionAync(user.Id, this.Input.Description);
            }

            if (this.Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
