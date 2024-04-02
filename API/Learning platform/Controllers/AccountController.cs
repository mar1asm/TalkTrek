using AutoMapper;
using Learning_platform.Entities;
using Learning_platform.Models;
using Learning_platform.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Org.BouncyCastle.Crypto.Generators;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace Learning_platform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly Security _security;
        private readonly EmailConfirmationClient _emailConfirmationClient;
        private readonly IMapper _mapper;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, UserRepository userRepository, IEmailSender emailSender, EmailConfirmationClient emailConfirmationClient, Security security, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _emailConfirmationClient = emailConfirmationClient;
            _security = security;
            _mapper = mapper;
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


                var result = await _signInManager.PasswordSignInAsync(model.Email,
                               decryptedPassword, true, lockoutOnFailure: true);
                await _signInManager.SignOutAsync();

                var userRoles = await _userManager.GetRolesAsync(ApplicationUser);
                var tokenResult = _security.GenerateEncodedToken(ApplicationUser.Id, "", userRoles);

                //var ress = _security.ValidateToken(tokenResult);


                if (result.Succeeded)
                {
                    return Ok(new { message = "Login successful", token=tokenResult });
                }

                if (result.IsLockedOut)
                {
                    return BadRequest(new { message = "Account locked out"});
                }
            }

            // If the user doesn't exist or passwords don't match, return an error indicating invalid credentials
            return BadRequest(new
            {
                message = "Invalid email or password"
            });
        }


        [HttpPost("logout")]

        public async Task<IActionResult> Logout([FromBody] string email)
        {
            ApplicationUser ApplicationUser = await _userManager.FindByEmailAsync(email);
            // Check if the user exists and if the stored hashed password is not null
            if (ApplicationUser != null)
            {
                await _signInManager.SignOutAsync();
                return Ok();
            }
            return BadRequest(new
            {
                message = "Invalid email"
            });

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
                    return BadRequest(new { message = "Failed to assign user role." });
                }

                var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
                emailConfirmationCode = System.Web.HttpUtility.UrlEncode(emailConfirmationCode);
                var emailConfirmationLink = "https://localhost:4200/confirm-email?userId=" + ApplicationUser.Id + "&confirmationCode=" + emailConfirmationCode;


                await _emailSender.SendEmailAsync(ApplicationUser.Email, "Confirm your email", $"Please confirm your email by <a href='{emailConfirmationLink}'>clicking here</a>.");

                return Ok(new { message = "Registration successful. Please check your email for confirmation." });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }


        [HttpPut("change-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Invalid model" });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound(new { message = "User not found." });
            }

            var ApplicationUser = await _userManager.FindByIdAsync(userId);
            if (ApplicationUser == null)
            {
                return NotFound(new { message = "Invalid user" });
            }

            var decryptedOldPassword = Security.DecryptPassword(model.OldPassword, "abcdefghijklmnop"); 
            var decryptedNewPassword = Security.DecryptPassword(model.NewPassword, "abcdefghijklmnop"); 


            var changeResult = await _userManager.ChangePasswordAsync(ApplicationUser, decryptedOldPassword, decryptedNewPassword);
            if (!changeResult.Succeeded)
            {
                return BadRequest(new { message = "Failed to change password" });
            }

            await _signInManager.SignOutAsync();

            return Ok(new { message = "Password updated successfully" });
        }


        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new
                {
                    message = "User ID is required."
                });
            }

            var ApplicationUser = await _userManager.FindByEmailAsync(email);

            if (ApplicationUser == null)
            {
                return NotFound(new
                {
                    message = "Invalid user"
                });
            }


            var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(ApplicationUser);
            var emailConfirmationLink = "localhost:4200/confirm-email?userId=" + ApplicationUser.Id + "&confirmationCode=" + emailConfirmationCode;

            await _emailSender.SendEmailAsync(ApplicationUser.Email, "Confirm your email", $"Please confirm your email by <a href='{emailConfirmationLink}'>clicking here</a>.");

            return Ok(new
            {
                message = "Email resent. Please check your email."
            });

        }

        [HttpPost("confirm-email", Name = "ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail([FromBody] EmailConfirmationModel model)
        {
            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.ConfirmationCode))
            {
                return BadRequest(new
                {
                    message = "User ID and confirmation code are required."
                });
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "Invalid user"
                });
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.ConfirmationCode);
            if (result.Succeeded)
            {
                return Ok(new
                {
                    message = "Code confirmed"
                });
            }

            return BadRequest(new
            {
                message = "Invalid request"
            });
        }

        [HttpDelete("account/photo")]

        public async Task<IActionResult> DeleteProfilePhoto()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound("User not found.");
            }
            var success = await _userRepository.DeleteProfilePhoto(userId);
            if (success)
            {
                return Ok(new
                {
                    message = "Profile photo deleted successfully."
                });
            }

            return BadRequest(new
            {
                message = "Failed to deleted profile photo."
            });
        }


        [HttpPut("account/photo")]
        public async Task<IActionResult> UpdateProfilePhoto([FromForm(Name = "photo")] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return NotFound("User not found.");
            }

            // Read the file contents into a byte array
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                // Convert the byte array to a Base64-encoded string
                var base64String = Convert.ToBase64String(imageBytes);

                var success = await _userRepository.UpdateProfilePhoto(userId, base64String);
                if (success)
                {
                    return Ok(new
                    {
                        message = "Profile photo updated successfully."
                    });
                }
            }

            return BadRequest(new
            {
                message = "Failed to update profile photo."
            });
        }


        [HttpPut("account/profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UserDto model)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _userRepository.UpdateProfile(userId, model);
            if (success)
            {
                return Ok(new
                {
                    message = "Profile completed successfully."
                });
            }
            else
            {
                return BadRequest(new
                {
                    message = "Failed to complete profile."
                });
            }
        }

        //GET: Profile
        [HttpGet("account/details")]
        public async Task<ActionResult<UserDto>> GetDetails()
        {
            // Get the user's ID from the token claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new
                {
                    message = "User ID not found in token claims."
                });
            }

            var ApplicationUser = await _userManager.FindByIdAsync(userId);

            if (ApplicationUser == null)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            var userEntity = await _userRepository.GetUserAsync(ApplicationUser.Id);

            var userRole = await _userManager.GetRolesAsync(ApplicationUser);

            var user = _mapper.Map<UserDto>(userEntity);
            if (user == null)
                user = new UserDto();
            _mapper.Map(ApplicationUser, user);
            user.UserType = userRole.FirstOrDefault();

            return Ok(new
            {
                user = user
            });

        }

        [HttpGet("restricted-endpoint")]
        [Authorize(Policy = "RequireStudentRole")]
        public async Task <IActionResult> TEST()
        {
            return Ok("You have access to the restricted endpoint.");
        }


        [HttpGet("account/check")]
        public async Task<ActionResult<bool>> CheckAccountComplete()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new
                {
                    message = "User ID not found in token claims."
                });
            }

            var ApplicationUser = await _userManager.FindByIdAsync(userId);

            if (ApplicationUser == null)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            if (ApplicationUser.IsProfileComplete)
                return Ok(true); else
                return Ok(false);
        } 
    }
}
