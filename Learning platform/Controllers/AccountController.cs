using Learning_platform.Entities;
using Learning_platform.Models;
using Learning_platform.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Learning_platform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly EmailConfirmationClient _emailConfirmationClient;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, IUserRepository userRepository, IEmailSender emailSender, EmailConfirmationClient emailConfirmationClient)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _emailConfirmationClient = emailConfirmationClient; 
        }

        [HttpPost("register/tutor")]
        public async Task<IActionResult> RegisterTutor(InitialRegisterModel model)
        {
            return await RegisterUser(model, UserTypeOptions.Tutor);
        }

        [HttpPost("register/student")]
        public async Task<IActionResult> RegisterStudent(InitialRegisterModel model)
        {
            return await RegisterUser(model, UserTypeOptions.Student);
        }


        [HttpPost("complete-profile")]
        public async Task<IActionResult> CompleteProfile( AccountBasicDetailsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _userRepository.CompleteUserProfileAsync(model);
            if (success)
            {
                return Ok("Profile completed successfully.");
            }
            else
            {
                return BadRequest("Failed to complete profile.");
            }
        }


        private async Task<IActionResult> RegisterUser(InitialRegisterModel model, string userType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ApplicationUser = new ApplicationUser(
                model.Email,
                model.Email
            );

            var result = await _userManager.CreateAsync(ApplicationUser, model.Password);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(ApplicationUser, userType);


                if (!roleResult.Succeeded)
                {
                    // Handle role assignment failure
                    return BadRequest("Failed to assign user role.");
                }

                var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
                var emailConfirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = ApplicationUser.Id, confirmationCode = emailConfirmationCode, httpMethod = "POST" }, Request.Scheme);

                await _emailSender.SendEmailAsync(ApplicationUser.Email, "Confirm your email", $"Please confirm your email by <a href='{emailConfirmationLink}'>clicking here</a>.");

                return Ok("Registration successful. Please check your email for confirmation.");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("confirm-email", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string confirmationCode)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(confirmationCode))
            {
                return BadRequest("User ID and confirmation code are required.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            { 
                return NotFound("Invalid user"); 
            }

            var result = await _userManager.ConfirmEmailAsync(user, confirmationCode);
            if (result.Succeeded)
            {
                return Ok("Email address confirmed");
            }

            return BadRequest("Invalid request");
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailGet(string userId, string confirmationCode)
        {
            // Call the ConfirmEmailAsync method in EmailConfirmationClient
            var result = await _emailConfirmationClient.ConfirmEmailAsync(userId, confirmationCode);

            if (result)
            {
                return Ok("Email address confirmed");
            }
            else
            {
                return BadRequest("Failed to confirm email address");
            }
        }

    }
}
