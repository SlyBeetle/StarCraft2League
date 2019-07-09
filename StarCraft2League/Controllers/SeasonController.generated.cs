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
    public partial class SeasonController
    {
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        protected SeasonController(Dummy d)
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
        public virtual IActionResult Details()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Details);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult Groups()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Groups);
        }

        [NonAction]
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public virtual IActionResult Playoffs()
        {
            return new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Playoffs);
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public SeasonController Actions => MVC.Season;
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Area = "";
        [GeneratedCode("R4Mvc", "1.0")]
        public readonly string Name = "Season";
        [GeneratedCode("R4Mvc", "1.0")]
        public const string NameConst = "Season";
        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ActionNamesClass s_ActionNames = new ActionNamesClass();
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames => s_ActionNames;
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Register = "Register";
            public readonly string RegisterAll = "RegisterAll";
            public readonly string Details = "Details";
            public readonly string Participants = "Participants";
            public readonly string Groups = "Groups";
            public readonly string Schedule = "Schedule";
            public readonly string Playoffs = "Playoffs";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Register = "Register";
            public const string RegisterAll = "RegisterAll";
            public const string Details = "Details";
            public const string Participants = "Participants";
            public const string Groups = "Groups";
            public const string Schedule = "Schedule";
            public const string Playoffs = "Playoffs";
        }

        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames => s_ViewNames;
            public class _ViewNamesClass
            {
                public readonly string Details = "Details";
                public readonly string Groups = "Groups";
                public readonly string Participants = "Participants";
                public readonly string Playoffs = "Playoffs";
                public readonly string Schedule = "Schedule";
                public readonly string _Match = "_Match";
            }

            public readonly string Details = "~/Views/Season/Details.cshtml";
            public readonly string Groups = "~/Views/Season/Groups.cshtml";
            public readonly string Participants = "~/Views/Season/Participants.cshtml";
            public readonly string Playoffs = "~/Views/Season/Playoffs.cshtml";
            public readonly string Schedule = "~/Views/Season/Schedule.cshtml";
            public readonly string _Match = "~/Views/Season/_Match.cshtml";
        }

        [GeneratedCode("R4Mvc", "1.0")]
        static readonly ViewsClass s_Views = new ViewsClass();
        [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
        public ViewsClass Views => s_Views;
    }

    [GeneratedCode("R4Mvc", "1.0"), DebuggerNonUserCode]
    public partial class R4MVC_SeasonController : StarCraft2League.Controllers.SeasonController
    {
        public R4MVC_SeasonController(): base(Dummy.Instance)
        {
        }

        [NonAction]
        partial void RegisterOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Register()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Register);
            RegisterOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void RegisterAllOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult RegisterAll()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.RegisterAll);
            RegisterAllOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void DetailsOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, int id);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Details(int id)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Details);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "id", id);
            DetailsOverride(callInfo, id);
            return callInfo;
        }

        [NonAction]
        partial void ParticipantsOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Participants()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Participants);
            ParticipantsOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void GroupsOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, int seasonId);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Groups(int seasonId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Groups);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "seasonId", seasonId);
            GroupsOverride(callInfo, seasonId);
            return callInfo;
        }

        [NonAction]
        partial void ScheduleOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo);
        [NonAction]
        public override Microsoft.AspNetCore.Mvc.IActionResult Schedule()
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Schedule);
            ScheduleOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void PlayoffsOverride(R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult callInfo, int seasonId);
        [NonAction]
        public override System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> Playoffs(int seasonId)
        {
            var callInfo = new R4Mvc_Microsoft_AspNetCore_Mvc_ActionResult(Area, Name, ActionNames.Playoffs);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "seasonId", seasonId);
            PlayoffsOverride(callInfo, seasonId);
            return System.Threading.Tasks.Task.FromResult<Microsoft.AspNetCore.Mvc.IActionResult>(callInfo);
        }
    }
}
#pragma warning restore 1591, 3008, 3009, 0108