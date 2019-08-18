using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlogPresentation.Commond
{
    public class TestActionFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            UrlHelper url = new UrlHelper(filterContext.RequestContext);
            filterContext.Result = new RedirectResult(url.Action("Index","Home")); 
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
    }
}