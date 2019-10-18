using jobsity.Repository;
using System.Web.Mvc;
using System.Web;
using jobsity.Utilities.Enumerators;
using System.Security.Cryptography;

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

        public static bool isAdmin = false;

        public Security(bool exit = false, bool admin = false)
        {
            this.exit = exit;
            if (!exit && (LoggedUser == null || (admin && LoggedUser.IdRole != (int)EnumRole.Admin)))
            {
                this.exit = true;
            }

        }

        public static string Md5Enc(string clave)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(clave);
            data = provider.ComputeHash(data);
            string md5 = string.Empty;
            for (int i = 0; i < data.Length; i++)
                md5 += data[i].ToString("x2").ToLower();

            return md5;
        }

        public static void Login(User user)
        {
            LoggedUser = user;
            isAdmin = user.IdRole == (int)EnumRole.Admin;
        }

        public static void SignOut()
        {
            if (HttpContext.Current.Session != null)
                HttpContext.Current.Session.Clear();
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
