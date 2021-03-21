using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdvikEnterprises.Attributes
{
    //https://www.andybutland.dev/2020/06/dependency-injection-in-aspnet-core-attributes.html
    //public class AddRandomNumberHeader2Attribute : TypeFilterAttribute
    //{
    //public AddRandomNumberHeader2Attribute() : base(typeof(AddRandomNumberHeader2Filter))
    //{
    //}

    public class AddRandomNumberHeader2Filter : IAsyncAuthorizationFilter
    {
        private readonly IAntiforgery _antiforgery;

        public AddRandomNumberHeader2Filter(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
            //Roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //Task.Run(async () =>
            //{
            //    var res = await _antiforgery.IsRequestValidAsync(context.HttpContext);
            //    if (res == false)
            //    {
            //        context.Result = new UnauthorizedResult();
            //        return;
            //    }
            //    else
            //    {
            //        var role = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
            //        //if (Roles.Split(',').Any(x => x.Trim() == role))
            //        if (role == "Admin")
            //            return;

            //        context.Result = new UnauthorizedResult();
            //        return;
            //    }
            //});

            //var res = _antiforgery.IsRequestValidAsync(context.HttpContext);
            //if (res.Result == false)
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}
            //else
            //{
            //    var role = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
            //    //if (Roles.Split(',').Any(x => x.Trim() == role))
            //    if (role == "Admin")
            //        return;

            //    context.Result = new UnauthorizedResult();
            //    return;
            //}


            //public void OnResultExecuted(ResultExecutedContext context)
            //{
            //}

            //public void OnResultExecuting(ResultExecutingContext context)
            //{
            //    context.HttpContext.Response.Headers.Add("X_Random_Number_3", _randomNumberService.GetRandomInteger().ToString());
            //}
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //Task.Run(async () =>
            //{
            try
            {
                await _antiforgery.ValidateRequestAsync(context.HttpContext);

                //var role = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
                ////if (Roles.Split(',').Any(x => x.Trim() == role))
                //if (role == "Admin")
                //    return;

                //context.Result = new UnauthorizedResult();
                //return;
            }
            catch (AntiforgeryValidationException exception)
            {
                //_logger.AntiforgeryTokenInvalid(exception.Message, exception);
                context.Result = new AntiforgeryValidationFailedResult();
            }


            //await _antiforgery.ValidateRequestAsync(context.HttpContext);

            //var res = await _antiforgery.IsRequestValidAsync(context.HttpContext);
            //if (res == false)
            //{
            //    context.Result = new UnauthorizedResult();
            //    return;
            //}
            //else
            //{
            //var role = context.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).FirstOrDefault().Value;
            ////if (Roles.Split(',').Any(x => x.Trim() == role))
            //if (role == "Admin")
            //    return;

            //context.Result = new UnauthorizedResult();
            //return;
            //}
            //});
        }
    }
}