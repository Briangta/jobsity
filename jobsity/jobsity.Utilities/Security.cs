using jobsity.Repository;
using System.Web.Mvc;
using System.Web;
using jobsity.Utilities.Enumerators;

namespace jobsity.Utilities
{
    public class Security : AuthorizeAttribute
    {
        private const string SessiionUser = "STR_SESSION_USER";
        public static User LoggedUser
        {
            get
            {
                return (User)HttpContext.Current.Session[SessiionUser];
            }
            set
            {
                HttpContext.Current.Session[SessiionUser] = value;
            }
        }
        public bool exit
        {
            get;
            set;
        }
        public Security(bool exit = false, bool admin = false)
        {
            this.exit = exit;
            if (!exit && (LoggedUser == null || (admin && LoggedUser.IdRole != (int)EnumRole.Admin)))
            {
                this.exit = true;
            }

        }

        public static void Login(User user)
        {
            LoggedUser = user;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            ////Valida la funcion principal si existe el usuario
            return this.exit;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (this.exit)
            {
                filterContext.Result = new RedirectToRouteResult(
                       new System.Web.Routing.RouteValueDictionary
                                   {
                                       { "action", "Login" },
                                       { "controller", "User" },
                                       { "area", ""}
                                   });
            }
        }
    }
}
