using Learning_platform.Entities;
using Learning_platform.Models;
using Learning_platform.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            // Retrieve the user record from the database based on the email
            ApplicationUser ApplicationUser = await _userManager.FindByEmailAsync(model.Email);

            // Check if the user exists and if the stored hashed password is not null
            if (ApplicationUser != null && !string.IsNullOrEmpty(ApplicationUser.PasswordHash))
            {
                   var decryptedPassword = Security.DecryptPassword(model.Password, "abcdefghijklmnop"); //change this
                // Compare the decrypted password received from the client with the hashed password stored in the database for the user
                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(ApplicationUser, decryptedPassword);

                if (isPasswordCorrect)
                {
                    // Passwords match, authenticate the user
                    // You can generate a token or perform any other authentication mechanism here
                    return Ok("Login successful");
                }
            }

            // If the user doesn't exist or passwords don't match, return an error indicating invalid credentials
            return BadRequest("Invalid email or password");
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] InitialRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ApplicationUser = new ApplicationUser(
                model.Email,
                model.Email
            );

            var decryptedPassword = Security.DecryptPassword(model.Password, "abcdefghijklmnop");


            var result = await _userManager.CreateAsync(ApplicationUser, decryptedPassword);

            if (result.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(ApplicationUser, model.UserType);


                if (!roleResult.Succeeded)
                {
                    // Handle role assignment failure
                    return BadRequest("Failed to assign user role.");
                }

                var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
                var emailConfirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = ApplicationUser.Id, confirmationCode = emailConfirmationCode, httpMethod = "POST" }, Request.Scheme);

                await _emailSender.SendEmailAsync(ApplicationUser.Email, "Confirm your email", $"Please confirm your email by <a href='{emailConfirmationLink}'>clicking here</a>.");

                return Ok(new { message = "Registration successful. Please check your email for confirmation." });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("User ID is required.");
            }

            var ApplicationUser = await _userManager.FindByEmailAsync(email);

            if (ApplicationUser == null)
            {
                return NotFound("Invalid user");
            }


            var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
            var emailConfirmationLink = Url.Action("ConfirmEmail", "Account", new { ApplicationUser.Id, confirmationCode = emailConfirmationCode, httpMethod = "POST" }, Request.Scheme);

            await _emailSender.SendEmailAsync(ApplicationUser.Email, "Confirm your email", $"Please confirm your email by <a href='{emailConfirmationLink}'>clicking here</a>.");

            return Ok("Email resent. Please check your email.");

            return BadRequest("Invalid request");
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

        [HttpGet("confirm-email")] // idk why the browser makes a GET request even if the method is configured as POST in the link
        public async Task<IActionResult> ConfirmEmailGet(string userId, string confirmationCode)
        {
            // Call the ConfirmEmailAsync method in EmailConfirmationClient
            var result = await _emailConfirmationClient.ConfirmEmailAsync(userId, confirmationCode);

            if (result)
            {
                // Add a delay of 10 seconds ( maybe move the logic to frontend?? )
                await Task.Delay(10000); 

                // Redirect to a success page
                return Redirect("/success-page");
            }
            else
            {
                return BadRequest("Failed to confirm email address");
            }
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

        

    }
}
