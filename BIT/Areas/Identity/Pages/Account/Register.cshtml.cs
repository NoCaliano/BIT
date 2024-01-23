// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using BIT.Areas.Identity.Data;
using BIT.DataStuff;
using BIT.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace BIT.Areas.Identity.Pages.Account
{
    
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager, // Add RoleManager injection
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            AppDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager; // Assign injected RoleManager to the field
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
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

            [Display(Name = "Розширена реєстрація")]
            public bool IsExtendedRegistration { get; set; }

            [Display(Name = "Ім'я")]
            [Required(ErrorMessage = "Поле 'Ім'я' обов'язкове.")]
            [MaxLength(35, ErrorMessage = "Максимальна довжина імені - 35 символів.")]
            [RegularExpression(@"^([А-ЯІ]{1}[а-яі]{1,35}|[A-Z]{1}[a-z]{1,35})$", ErrorMessage = "Дозволені тільки літери алфавіту.")]
            public string FirstName { get; set; }

            [Display(Name = "Прізвище")]
            [Required(ErrorMessage = "Поле 'Прізвище' обов'язкове.")]
            [MaxLength(35, ErrorMessage = "Максимальна довжина прізвища - 35 символів.")]
            [RegularExpression(@"^([А-ЯІ]{1}[а-яі]{1,35}|[A-Z]{1}[a-z]{1,35})$", ErrorMessage = "Дозволені тільки літери алфавіту.")]
            public string LastName { get; set; }

            [Display(Name = "Телефон")]
            [Required(ErrorMessage = "Поле 'Телефон' обов'язкове.")]
            [RegularExpression(@"^\+\d{7,13}$", ErrorMessage = "Телефонний номер повинен бути у форматі '+XXXXXXXXXXXXX', де X - це цифра.")]
            public string PhoneNumber { get; set; }
            [Required]
            [MaxLength(70,ErrorMessage ="Максимальна довжина адреси - 70 символів")]
            [Display(Name = "Місце проживання")]
            public string Address { get; set; }
            [Required]
            [MaxLength(50, ErrorMessage = "Максимальна довжина - 50 символів")]
            [Display(Name = "Вид транспорту")]
            public string VehicleType { get; set; }
        }



        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (!Input.IsExtendedRegistration) 
            {
                Input.FirstName = null;
                Input.LastName = null;
                Input.Address = null;
                Input.VehicleType = null;
                Input.PhoneNumber = null;
            }
            if (ModelState.IsValid || (!ModelState.IsValid && Input.FirstName == null && Input.LastName == null
                && Input.PhoneNumber == null && Input.VehicleType == null && Input.Address == null))
            {
                var user = CreateUser();
                
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);


                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (Input.IsExtendedRegistration && ModelState.IsValid)
                    {
                        user.FirstName = Input.FirstName;
                        user.LastName = Input.LastName;
                        Requisition requisition = new Requisition()
                        {
                            UserId = user.Id.ToString(),
                            Name = Input.FirstName,
                            LastName = Input.LastName,
                            PhoneNumber = Input.PhoneNumber,
                            Address = Input.Address,
                            VehicleType = Input.VehicleType,
                        };
                        _context.Requisitions.Add(requisition);
                        await _context.SaveChangesAsync();
                    }

                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user, "User");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
