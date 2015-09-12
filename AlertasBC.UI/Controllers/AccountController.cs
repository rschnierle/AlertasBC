using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using AlertasBC.UI.Models;
using AlertasBC.Repository;
using AlertasBC.Model;
using System.Threading.Tasks;

namespace AlertasBC.UI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        UserRepository userRepository = new UserRepository();

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Session["User"] = null;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
             var response = userRepository.ValidateUser(new User(){USERNAME=model.UserName, PASSWORD=model.Password} );

            //var response = new AlertasBC.Model.Utils.RepositoryResponse<User>() { Data = new User() { NAME = "Karina", ID_DEPENDENCY = "1", USERNAME = "kcro" }, Success=true };



            if (ModelState.IsValid && response.Success)
            {
                var user = response.Data;
                FormsAuthentication.SetAuthCookie(user.USERNAME, false);
                DependencyRepository dependencyRepository = new DependencyRepository();
                var getDependency = await dependencyRepository.Get(user.ID_DEPENDENCY);
                if (getDependency.Success)
                {
                    Session["Dependency"] = getDependency.Data.NAME;
                    Session["User"] = user;
                    return RedirectToLocal(returnUrl);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "El usuario o contraseña es incorrecto.");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        
        
        public ActionResult LogOff()
        {
            userRepository.Logout();
            Session["Dependency"] = null;
            Session["User"] = null;

            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public async Task<ActionResult> Register()
        {
            DependencyRepository dependencyRepository= new DependencyRepository();
            var x= await dependencyRepository.GetDependencies();
            ViewBag.DependencyList = new SelectList(x.Data, "ID", "NAME");
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(User model)
        {
            DependencyRepository dependencyRepository = new DependencyRepository();
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    var response = await userRepository.RegisterUser(model);
                    if (response.Success)
                    {
                        var user = response.Data;
                        FormsAuthentication.SetAuthCookie(user.USERNAME, false);
                        var getDependency = await dependencyRepository.Get(user.ID_DEPENDENCY);
                        if (getDependency.Success)
                        {
                            Session["Dependency"] = getDependency.Data.NAME;
                            Session["User"] = user;
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Error);
                    }
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            // If we got this far, something failed, redisplay form
            dependencyRepository = new DependencyRepository();
            var x = await dependencyRepository.GetDependencies();
            ViewBag.DependencyList = new SelectList(x.Data, "ID", "NAME");
            
            return View(model);
        }

        //
        // POST: /Account/Disassociate

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            public ExternalLoginResult(string provider, string returnUrl)
            {
                Provider = provider;
                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }
            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                OAuthWebSecurity.RequestAuthentication(Provider, ReturnUrl);
            }
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
