using Core.UserClient.Data.DB;
using Core.UserClient.Data.Domain;
using Core.UserClient.Encryption;
using Core.UserClient.Models;
using Core.UserClient.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.UserClient.Controllers
{
    public class UserController : Controller
    {
        private const string SharedViewName = "~/Views/Shared/DisplayDetails.cshtml";

        public UserController(IRepository repository, IEncryptionHelper encryptionHelper)
        {
            Repository = repository;
            EncryptionHelper = encryptionHelper;
        }

        public IRepository Repository { get; }
        public IEncryptionHelper EncryptionHelper { get; }

        #region User Registration

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            return View(new UserRegisterModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (Repository.FetchUserByUsername(model.Username).HasEntity)
            {
                model.Errors.Add(Resource.UserAlreadyRegistered);
                return View(model);
            }

            var entity = new ClientUser()
            {
                Username = model.Username,
                SecurityLevel = model.SecurityLevel,
                Password = model.Password,
                DateOfBirth = model.DateOfBirth
            };

            entity.Password = EncryptionHelper.EncryptString(ClientUser.Concatenate(entity.Username, entity.Password));

            if (Repository.CreateUser(entity)
                .Evaluate(OnInternalFailure: (op) => throw op.Exception))
            {
                ViewData[Resource.SharedDisplayMessageProperty] = Resource.SharedDisplayMessageOnRegister + model.Username;
                return View(SharedViewName);
            }

            return View(model);
        }

        #endregion

        #region User Login

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn(string returnUrl = null)
        {
            return View(new UserLoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = AuthenticationSchemes.OAuth)]
        public IActionResult SignInWithOAuth()
        {
            return RedirectToAction(actionName: "Index", controllerName: "Secret");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(UserLoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = Repository.FetchUserByUsername(model.Username);

            if (!result.HasEntity)
            {
                model.Errors.Add(Resource.UserNotRegistered);
                return View(model);
            }

            var clientUser = result.Entity as ClientUser;

            var decryptedPassword = EncryptionHelper.DecryptString(clientUser.Password);

            bool isPasswordMatching = ClientUser.Concatenate(clientUser.Username, model.Password) == decryptedPassword;

            if (!isPasswordMatching)
            {
                model.Errors.Add(Resource.IncorrectCredentials);
                return View(model);
            }

            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, clientUser.Username),
                new Claim(JwtRegisteredClaimNames.Birthdate, clientUser.DateOfBirth.ToShortDateString()),
                new Claim(Utility.ClaimTypes.security_level, clientUser.SecurityLevel.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(authenticationType: AuthenticationSchemes.Cookie, claims: claims);

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.AuthenticateAsync(AuthenticationSchemes.Cookie);

            await HttpContext.SignInAsync(scheme: AuthenticationSchemes.Cookie, claimsPrincipal, properties: new AuthenticationProperties
            {
                AllowRefresh = false,
                IsPersistent = true,
                IssuedUtc = System.DateTimeOffset.Now,
                ExpiresUtc = System.DateTimeOffset.Now.AddMinutes(2),
            });

            if (string.IsNullOrEmpty(model.ReturnUrl))
                return RedirectToAction(actionName: "Index", controllerName: "Secret");
            else
                return Redirect(model.ReturnUrl);
        }

        #endregion

        [Authorize]
        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction(actionName: "SignIn", controllerName: "User");
        }

        [Authorize]
        [HttpGet]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            ViewData[Resource.SharedDisplayMessageProperty] = Resource.SharedDisplayMessageOnAccessDenied + returnUrl;
            return View(SharedViewName);
        }
    }
}
