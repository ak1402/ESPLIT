using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using DataAccessLayer.Models;
using System.Web.Security;
using SplitWise.Controllers;
namespace SplitWise.Custom
{
  
    public class CustomAdminFilter : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Convert.ToBoolean(filterContext.HttpContext.Session["IsAdmin"]))
            {
                ContentResult cont = new ContentResult();
                cont.Content = "This page is only for Admins";
                filterContext.Result = cont;
                
            }
        }
        
    }
}