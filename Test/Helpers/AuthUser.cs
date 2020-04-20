using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test.Helpers
{
    public class AuthUser :ActionFilterAttribute,IActionFilter
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var user=context.HttpContext.Session.GetString("user");            
            var  result=new RedirectToActionResult("Login","Home",null);
            if(string.IsNullOrEmpty(user)){
                context.Result=result;
            }
        }             
    }
}