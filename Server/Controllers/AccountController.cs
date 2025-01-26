using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Tripbuk.Server.Models;

namespace Tripbuk.Server.Controllers
{
    [Route("Account/[action]")]
    public partial class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly Data.ApplicationIdentityDbContext _context;

        public AccountController(Data.ApplicationIdentityDbContext context, IWebHostEnvironment env, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._env = env;
            this._configuration = configuration;

            this._context = context;
        }

        private IActionResult RedirectWithError(string error, string redirectUrl = null)
        {
             if (!string.IsNullOrEmpty(redirectUrl))
             {
                 return Redirect($"/auth/login?error={error}&redirectUrl={Uri.EscapeDataString(redirectUrl.Replace("~", ""))}");
             }
             else
             {
                 return Redirect($"/auth/login?error={error}");
             }
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            if (returnUrl != "/" && !string.IsNullOrEmpty(returnUrl))
            {
                return Redirect($"/auth/login?redirectUrl={Uri.EscapeDataString(returnUrl)}");
            }

            return Redirect("/auth/login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string redirectUrl)
        {
            redirectUrl = string.IsNullOrEmpty(redirectUrl) ? "/" : redirectUrl.StartsWith("/") ? redirectUrl : $"/{redirectUrl}";

            if (_env.EnvironmentName == "Development" && userName == "admin" && password == "admin")
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, "Administrator"),
                    new Claim(ClaimTypes.Email, "admin@devstroop.com"),
                    new Claim(ClaimTypes.Role, "admin")
                };

                _roleManager.Roles.ToList().ForEach(r => claims.Add(new Claim(ClaimTypes.Role, r.Name)));
                await _signInManager.SignInWithClaimsAsync(new ApplicationUser { UserName = userName, Email = userName }, isPersistent: false, claims);

                return Redirect(redirectUrl);
            }

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {

                var user = await _userManager.FindByNameAsync(userName);

                if (user == null)
                {
                    return RedirectWithError("Invalid user or password", redirectUrl);
                }

                if (!user.EmailConfirmed)
                {
                    return RedirectWithError("User email not confirmed", redirectUrl);
                }

                var isTenantsAdmin = userName == "tenantsadmin";    
                var isTwoFactor = await _userManager.GetTwoFactorEnabledAsync(user);
                if (!isTwoFactor && !isTenantsAdmin)
                {
                    await _userManager.SetTwoFactorEnabledAsync(user, true);
                }
                var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);


                if (result.RequiresTwoFactor && !isTenantsAdmin)
                {
                    var code = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");

                    var text = $@"Hi, <br /> <br />
We received your request for a single-use code to use with your Tripbuk account. <br /> <br />
Your single-use code is: {code} <br /> <br />
If you didn't request this code, you can safely ignore this email. Someone else might have typed your email address by mistake.";

                    await SendEmailAsync(user.Email, "Your single-use code", text);

                    return Redirect($"/auth/security-code?email={Uri.EscapeDataString(user.Email)}");
                }
                if (result.Succeeded)
                {

                    if (user != null)
                    {
                        var tenant = _context.Tenants.Where(t => t.Id == user.TenantId).FirstOrDefault();
                        if (tenant != null && !tenant.Hosts.Split(',').Where(h => h.Contains(this.HttpContext.Request.Host.Value)).Any())
                        {
                            await _signInManager.SignOutAsync();
                            return RedirectWithError("Invalid user or password", redirectUrl);
                        }
                    }
                    return Redirect(redirectUrl);
                }
            }

            return RedirectWithError("Invalid user or password", redirectUrl);
        }

        [HttpPost]
        public async Task<IActionResult> VerifySecurityCode(string code)
        {
            var result = await _signInManager.TwoFactorSignInAsync("Email", code, false, false);

            if (!result.Succeeded)
            {
                return RedirectWithError("Invalid security code");
            }

            return Redirect("/");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
            {
                return BadRequest("Invalid password");
            }

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(id);

            _userManager.UserValidators.Clear();
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (result.Succeeded)
            {
                return Ok();
            }

            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return BadRequest(message);
        }

        [HttpPost]
        public ApplicationAuthenticationState CurrentUser()
        {
            return new ApplicationAuthenticationState
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                Name = User.Identity.Name,
                Claims = User.Claims.Select(c => new ApplicationClaim { Type = c.Type, Value = c.Value })
            };
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Invalid user name or password.");
            }

            var user = new ApplicationUser { UserName = userName, Email = userName };

            var tenant = _context.Tenants.ToList().Where(t => t.Hosts.Split(',').Where(h => h.Contains(this.HttpContext.Request.Host.Value)).Any()).FirstOrDefault();
            if (tenant != null)
            {
                _userManager.UserValidators.Clear();

                if (_context.Users.Any(u => u.TenantId == tenant.Id && u.UserName == user.Name))
                {
                    return BadRequest("User with the same name already exist for this tenant.");
                }
            }
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                try
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code }, protocol: Request.Scheme);

                    var text = $@"Hi, <br /> <br />
We received your registration request for Tripbuk. <br /> <br />
To confirm your registration please click the following link: <a href=""{callbackUrl}"">confirm your registration</a> <br /> <br />
If you didn't request this registration, you can safely ignore this email. Someone else might have typed your email address by mistake.";                    

                    await SendEmailAsync(user.Email, "Confirm your registration", text);


                    var newUser = _context.Users.FirstOrDefault(u => u.TenantId == null && u.UserName == userName);
                    if (newUser != null && tenant != null)
                    {
                        newUser.TenantId = tenant.Id;
                        _context.Users.Update(newUser);
                        _context.SaveChanges();
                    }

                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }

            var message = string.Join(", ", result.Errors.Select(error => error.Description));

            return BadRequest(message);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Redirect("/auth/login?info=Your registration has been confirmed");
            }

            return RedirectWithError("Invalid user or confirmation code");
        }

        public async Task<IActionResult> ResetPassword(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return BadRequest("Invalid user name.");
            }

            try
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);

                var callbackUrl = Url.Action("ConfirmPasswordReset", "Account", new { userId = user.Id, code }, protocol: Request.Scheme);

                var body = string.Format(@"<a href=""{0}"">{1}</a>", callbackUrl, "Please confirm your password reset.");

                await SendEmailAsync(user.Email, "Confirm your password reset", body);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> ConfirmPasswordReset(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Redirect("/auth/login?error=Invalid user");
            }

            var password = GenerateRandomPassword();

            var result = await _userManager.ResetPasswordAsync(user, code, password);

            if (result.Succeeded)
            {
                await SendEmailAsync(user.Email, "New password", $"<p>Your new password is: {password}</p><p>Please change it after login.</p>");

                return Redirect("/auth/login?info=Password reset successful. You will receive an email with your new password.");
            }

            return Redirect("/auth/login?error=Invalid user or confirmation code");
        }

        private static string GenerateRandomPassword()
        {
            var options = new PasswordOptions
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            var randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789",
                "!@$?_-"
            };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (options.RequireUppercase)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[0][rand.Next(0, randomChars[0].Length)]);
            }

            if (options.RequireLowercase)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[1][rand.Next(0, randomChars[1].Length)]);
            }

            if (options.RequireDigit)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[2][rand.Next(0, randomChars[2].Length)]);
            }

            if (options.RequireNonAlphanumeric)
            {
                chars.Insert(rand.Next(0, chars.Count), randomChars[3][rand.Next(0, randomChars[3].Length)]);
            }

            for (int i = chars.Count; i < options.RequiredLength || chars.Distinct().Count() < options.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count), rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        private async Task SendEmailAsync(string to, string subject, string body)
        {

            var mailMessage = new System.Net.Mail.MailMessage();
            mailMessage.From = new System.Net.Mail.MailAddress(_configuration.GetValue<string>("Smtp:User"));
            mailMessage.Body = body;
            mailMessage.Subject = subject;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(to);

            var client = new System.Net.Mail.SmtpClient(_configuration.GetValue<string>("Smtp:Host"))
            {
                UseDefaultCredentials = false,
                EnableSsl = _configuration.GetValue<bool>("Smtp:Ssl"),
                Port = _configuration.GetValue<int>("Smtp:Port"),
                Credentials = new System.Net.NetworkCredential(_configuration.GetValue<string>("Smtp:User"), _configuration.GetValue<string>("Smtp:Password"))
            };

            await client.SendMailAsync(mailMessage);
        }
    }
}
