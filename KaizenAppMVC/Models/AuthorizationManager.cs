using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static KaizenAppMVC.Models.ProcessorModel;

namespace KaizenAppMVC.Models
{
    public class  AuthorizationManager : AuthorizeAttribute
    {
        
        public string ManagerUsername { get; set; }
        

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var data = Task.Run(() => LoadManagerUsernameData()).GetAwaiter().GetResult();
            

            List<AuthorizationManager> ManagerUsername = new List<AuthorizationManager>();
            foreach (var row in data)
            {
                ManagerUsername.Add(new AuthorizationManager
                {

                    ManagerUsername = row.ManagerUsername
                });
            }
            
            string username = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1);
            var isAuthorized = ManagerUsername.AsEnumerable();
            var str = (isAuthorized.FirstOrDefault().ManagerUsername.ToLower().Contains(username.ToLower()));
            if (str==true )
            {
                Roles = "Manager";
            }
            return str;
            

            
        }

    }


}
 
             