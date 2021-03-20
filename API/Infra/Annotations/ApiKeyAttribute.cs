using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Infra.Annotations
{
    public class ApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string ApiKeyName = "x-apikey";
        private const string ApiKey = "Batman Batman Batman";

        /// <summary>
        /// Annotation para validar se no request esta retornando no header o valor da chave de segurança ApiKey em x-apikey
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var apikey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Sem autorização"
                };
                return;
            }


            if (!ApiKey.Equals(apikey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403,
                    Content = "Sem permissão"
                };
                return;
            }

            await next();
        }
    }
}
