using FairfieldAllergy.Data;
using FairfieldAllergy.Domain;
using FairfieldAllergy.Domain.Models;
using FairfieldAllergy.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FairfieldAllergy.Api.Controllers
{
    //[ApiController] //<- Make sure it is there
    public class AccountController : Controller
    {
        private readonly UserManager<FairfieldIdentityUser> userManager;
        private readonly SignInManager<FairfieldIdentityUser> signInManager;
        private readonly ILogger<AccountController> logger;
        private readonly IPasswordHasher<FairfieldIdentityUser> hasher;
        private IConfiguration config;

        public AccountController(UserManager<FairfieldIdentityUser> userManager,
            SignInManager<FairfieldIdentityUser> signInManager,
            ILogger<AccountController> logger,
            IPasswordHasher<FairfieldIdentityUser> hasher,
            IConfiguration config)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.config = config;
            this.hasher = hasher;
        }

        [HttpGet]
        public async Task<IActionResult> AddPassword()
        {
            var user = await userManager.GetUserAsync(User);

            var userHasPassword = await userManager.HasPasswordAsync(user);

            if (userHasPassword)
            {
                return RedirectToAction("ChangePassword");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddPassword(AddPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);

                var result = await userManager.AddPasswordAsync(user, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await signInManager.RefreshSignInAsync(user);

                return View("AddPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await userManager.GetUserAsync(User);

            var userHasPassword = await userManager.HasPasswordAsync(user);

            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }

            return View();
        }

        [HttpPost("api/changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }

                var result = await userManager.ChangePasswordAsync(user,
                    model.CurrentPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }

                await signInManager.RefreshSignInAsync(user);
                return View("ChangePasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid password reset token");
            }
            return View();
        }

        [HttpPost("api/resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

                //fairfieldAllergeryRepository.GetUseridFromEmail(model.UserName);

                //var checkPassword = await signInManager.CheckPasswordSignInAsync.PasswordSignInAsync(model.Email, model.Password, true, false);
                //var user = await userManager.FindByNameAsync(fairfieldAllergeryRepository.GetUseridFromEmail(model.UserName));
                var user = await userManager.FindByEmailAsync(model.UserName);
                //var checkPassword = await signInManager.CheckPasswordSignInAsync(user, model.OldPassword, true);
                //if (checkPassword.Succeeded)
                //{
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    try
                    {

                    }
                    catch (Exception er)
                    {
                        string s1 = er.ToString();
                    }

                    if (user != null)
                    {
                        var result = await userManager.ResetPasswordAsync(user, token, model.Password);
                        if (result.Succeeded)
                        {
                            if (await userManager.IsLockedOutAsync(user))
                            {
                                await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                            }

                            return Ok(new { status = "Success" });
                        }
                        else
                        {
                            return Ok(new { status = "Failed" });
                        }
                    }
                    else
                    {
                        return Ok(new { status = "Failed" });
                    }
                //}
                //else
                //{
                //    return Ok(new { status = "Failed" });
                //}
            }
            else
            {
                return Ok(new { status = "Failed" });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("api/forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
        {
            ChangePassword changePassword = new ChangePassword();

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);

                    changePassword.Token = token;
                    changePassword.Email = model.Email;
                    changePassword.Status = "Success";

                    return Ok(changePassword);
                }
                else 
                {
                    changePassword.Status = "Failure";
                    return Ok(changePassword);
               }
            }
            changePassword.Status = "Failure";
            return Ok(changePassword);
        }

        [AllowAnonymous]
        [HttpPost("api/adminlogout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return Ok(new { status = "Success" });
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        [AllowAnonymous]
        [HttpPost("api/register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new FairfieldIdentityUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Sex = model.Sex,
                    HomePhone = model.HomePhone
                };

                var result = await userManager.CreateAsync(user, model.Password);

                //var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                            new { userId = user.Id, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, confirmationLink);

                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }

                    ViewBag.ErrorTitle = "Registration successful";
                    ViewBag.ErrorMessage = "Before you can Login, please confirm your "
                       + "email, by clicking on the confirmation link we have emailed you";
                    //return View("Error");
                    return Ok(new { status = "Success" });
                }
                else
                {
                    return Ok(new { status = "Failure" });
                }

                //foreach (var error in result.Errors)
                //{
                //    ModelState.AddModelError(string.Empty, error.Description);
                //}
            }
            return Ok(new { status = "Failure" });
            //return View(model);
        }

        //var user = await UserManager.FindByEmailAsync(model.Email);
        //var user = await UserManager.FindByEmailAsync(model.Email);

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(model);
        }

        [HttpPost("api/adminlogin")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                    if (result.Succeeded)
                    {
                        return Ok(new { status = "Success" });
                    }
                }
                catch (Exception ex)
                {
                    string s1 = ex.ToString();
                }
            }

            return Ok(new { status = "Failure" });
        }

        [HttpPost("api/login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserInformation userInformation = new UserInformation();

                try
                {

                    //var result = await _signInMgr.PasswordSignInAsync(model.UserName, model.Password, false, false);
                    //var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                    var result = await signInManager.PasswordSignInAsync(
                                 model.UserName, model.Password, true, false);
                    //var user = await userManager.FindByNameAsync(.FindByLoginAsync(.FindAsync(context.UserName, context.Password);

                    if (result.Succeeded)
                    {
                        FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

                        if (fairfieldAllergeryRepository.HasUserChangedIdAndPassword(model.UserName))
                        {
                            userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                            userInformation.Password = model.Password;
                            userInformation.Status = "Success";
                            return Ok(userInformation);
                            //return Ok(new { status = "Success" });
                        }
                        else
                        {
                            userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                            userInformation.Password = model.Password;
                            userInformation.Status = "Change";
                            return Ok(userInformation);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s1 = ex.ToString();
                }
            }

            return Ok(new { status = "Failure" });
        }


        [HttpPost("api/changecredential")]
        [AllowAnonymous]
        public async Task<IActionResult> ChangeCredentials([FromBody] ChangeCredentialsViewModel model)
        {

            ChangeCredentials changeCredentials = new ChangeCredentials();

            if (ModelState.IsValid)
            {
                UserInformation userInformation = new UserInformation();
                FairfieldAllergeryRepository fairfieldAllergeryRepository = new FairfieldAllergeryRepository();

                try
                {
                    var result = await signInManager.PasswordSignInAsync(
                                 model.OldUserName, model.OldPassword, true, false);

                    //If signin works
                    if (result.Succeeded)
                    {
                        //Check to see if user llready exists
                        if (fairfieldAllergeryRepository.CheckToSeeIfIdIsAlreadyUsed(model.UserName))
                        {
                            var user = await userManager.FindByNameAsync(model.OldUserName);
                            var checkPassword = await signInManager.CheckPasswordSignInAsync(user, model.OldPassword, true);
                            if (checkPassword.Succeeded)
                            {
                                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                                if (user != null)
                                {
                                    var passwordChangeResult = await userManager.ResetPasswordAsync(user, token, model.Password);
                                    if (passwordChangeResult.Succeeded)
                                    {
                                        //if (await userManager.IsLockedOutAsync(user))
                                        //{
                                        //    await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                                        //}
                                        fairfieldAllergeryRepository.UpdateUseridAndPassword(model);

                                    }
                                    else
                                    {
                                        userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                                        userInformation.Status = "Failure";
                                        userInformation.ErrorMessage = "Password change did not succeed";
                                        return Ok(userInformation);
                                    }
                                }
                                else
                                {
                                    userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                                    userInformation.Status = "Failure";
                                    userInformation.ErrorMessage = "User is null";
                                    return Ok(userInformation);
                                }
                            }
                            else
                            {
                                userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                                userInformation.Status = "Failure";
                                userInformation.ErrorMessage = "Password check failed";
                                return Ok(userInformation);
                            }
                        }
                        else
                        {
                            userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                            userInformation.Status = "Failure";
                            userInformation.ErrorMessage = "UserId already taken.  Please select another";
                            return Ok(userInformation);
                        }
                        //***************************************************************************************************
                        if (fairfieldAllergeryRepository.HasUserChangedIdAndPassword(model.UserName))
                        {
                            userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                            userInformation.Status = "Success";
                            return Ok(userInformation);
                        }
                        else
                        {
                            userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                            userInformation.Status = "Change";
                            return Ok(userInformation);
                        }
                    }
                    else
                    {
                        changeCredentials.Status = "Failure";
                        changeCredentials.ErrorMessage = "None";
                        userInformation = fairfieldAllergeryRepository.GetUserInformation(model.UserName);
                        return Ok(userInformation);

                        return Ok(changeCredentials);
                    }
                }
                catch (Exception ex)
                {
                    string s1 = ex.ToString();
                }
            }

            changeCredentials.Status = "Failure";
            changeCredentials.ErrorMessage = "None";

            return Ok(changeCredentials);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                    new { ReturnUrl = returnUrl });

            var properties =
                signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult>
            ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginViewModel loginViewModel = new LoginViewModel
            {
                ReturnUrl = returnUrl,
                ExternalLogins =
                (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty,
                    $"Error from external provider: {remoteError}");

                return View("Login", loginViewModel);
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty,
                    "Error loading external login information.");

                return View("Login", loginViewModel);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            FairfieldIdentityUser user = null;

            if (email != null)
            {
                user = await userManager.FindByEmailAsync(email);

                if (user != null && !user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Email not confirmed yet");
                    return View("Login", loginViewModel);
                }
            }

            var signInResult = await signInManager.ExternalLoginSignInAsync(
                                        info.LoginProvider, info.ProviderKey,
                                        isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                if (email != null)
                {
                    if (user == null)
                    {
                        user = new FairfieldIdentityUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await userManager.CreateAsync(user);

                        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                        var confirmationLink = Url.Action("ConfirmEmail", "Account",
                                        new { userId = user.Id, token = token }, Request.Scheme);

                        logger.Log(LogLevel.Warning, confirmationLink);

                        ViewBag.ErrorTitle = "Registration successful";
                        ViewBag.ErrorMessage = "Before you can Login, please confirm your "
                          +  "email, by clicking on the confirmation link we have emailed you";
                        return View("Error");
                    }

                    await userManager.AddLoginAsync(user, info);
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }

                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
                ViewBag.ErrorMessage = "Please contact support on Pragim@PragimTech.com";

                return View("Error");
            }
        }

        [HttpPost("api/token")]
        public async Task<IActionResult> CreateToken([FromBody] CredentialModel model)
        {
            try
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {

                    var x = hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

                    if (hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var userClaims = await userManager.GetClaimsAsync(user);

                        var claims = new[]
                        {
              new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
              new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
              new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }.Union(userClaims);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                          issuer: config["Tokens:Issuer"],
                          audience: config["Tokens:Audience"],
                          claims: claims,
                          expires: DateTime.UtcNow.AddMinutes(15),
                          signingCredentials: creds
                          );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                string s1 = ex.ToString();
            }

            return BadRequest("Failed to generate token");
        }
    }
}

