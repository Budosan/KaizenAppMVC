using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using static KaizenAppMVC.Models.ProcessorModel;

namespace KaizenAppMVC.Models
{
    public class AuthorizationDataModel
    {
        public bool ManagerUsername { get; set; }

    }
}