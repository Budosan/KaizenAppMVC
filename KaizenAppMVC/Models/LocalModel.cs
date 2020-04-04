using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;

namespace KaizenAppMVC.Models
{
    public class LocalModel
    {
        public string ManagerEmail { get; set; }
        public string DepartmentManager { get; set; }
        public string UseName = HttpContext.Current.User.Identity.Name.Substring(HttpContext.Current.User.Identity.Name.IndexOf("\\") + 1).ToString();


    }
     
         
        
        
        
    

}