using System;
using System.Security.Claims;
using System.Threading.Tasks;
using ForFun.API.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ForFun.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            
            var resultcontext= await next();

            var UserID=int.Parse(resultcontext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo=resultcontext.HttpContext.RequestServices.GetService<IFunRepository>();
            var user= await repo.GetUser(UserID);
            user.LastActive=DateTime.Now;
            await repo.SaveAll();
        }
    }
}