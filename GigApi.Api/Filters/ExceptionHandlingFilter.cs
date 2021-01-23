using GigApi.Api.V1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.Filters
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var errorResponse = new ErrorResponse();

            errorResponse.Errors.Add(context.Exception.Message);

            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }
}
