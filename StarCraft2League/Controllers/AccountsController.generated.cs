// <auto-generated />
// This file was generated by R4Mvc.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the r4mvc.json file (i.e. the settings file), save it and run the generator tool again.

// Make sure the compiler doesn't complain about missing Xml comments and CLS compliance
// 0108: suppress "Foo hides inherited member Foo.Use the new keyword if hiding was intended." when a controller and its abstract parent are both processed
#pragma warning disable 1591, 3008, 3009, 0108
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using R4Mvc;

namespace StarCraft2League.Controllers
{
    public partial class AccountsController
    {
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected AccountsController(Dummy d)
        {
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(IActionResult result)
        {
            var callInfo = result.GetR4ActionResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<IActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(IActionResult result)
        {
            var callInfo = result.GetR4ActionResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<IActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToPage(IActionResult result)
        {
            var callInfo = result.GetR4ActionResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToPage(Task<IActionResult> taskResult)
        {
            return RedirectToPage(taskResult.Result);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToPagePermanent(IActionResult result)
        {
            var callInfo = result.GetR4ActionResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToPagePermanent(Task<IActionResult> taskResult)
        {
            return RedirectToPagePermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult Login()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Login);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public AccountsController Actions => MVC.Accounts;
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Area = "";
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Name = "Accounts";
        [GeneratedCode("R4Mvc", "1.0")]
        public const string NameConst = "Accounts";
        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ActionNamesClass s_ActionNames = new ActionNamesClass();
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames => s_ActionNames;
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Login = "Login";
            public readonly string Logout = "Logout";
            public readonly string Users = "Users";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Login = "Login";
            public const string Logout = "Logout";
            public const string Users = "Users";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames => s_ViewNames;
            public class _ViewNamesClass
            {
                public readonly string Users = "Users";
            }

            public readonly string Users = "~/Views/Accounts/Users.cshtml";
        }

        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ViewsClass s_Views = new ViewsClass();
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public ViewsClass Views => s_Views;
    }

    [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
    public partial class R4MVC_AccountsController : StarCraft2League.Controllers.AccountsController
    {
        public R4MVC_AccountsController(): base(Dummy.Instance)
        {
        }

        [NonAction]
        partial void LoginOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, string returnUrl);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Login(string returnUrl)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Login);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "returnUrl", returnUrl);
            LoginOverride(callInfo, returnUrl);
            return callInfo;
        }

        [NonAction]
        partial void LogoutOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Logout()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Logout);
            LogoutOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void UsersOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Users()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Users);
            UsersOverride(callInfo);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }

        [NonAction]
        partial void UsersOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, StarCraft2League.Models.Users.User[] users);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Users(StarCraft2League.Models.Users.User[] users)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Users);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "users", users);
            UsersOverride(callInfo, users);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }
    }
}
#pragma warning restore 1591, 3008, 3009, 0108
