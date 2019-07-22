using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Rocket.Surgery.AspNetCore.Mvc.Filters
{
    public class ValidationExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is ValidationException validationException)) return;

            context.ExceptionHandled = true;
            // TODO: UnprocessableEntityResult
            context.Result = new BadRequestObjectResult(new ValidationResult(validationException.Errors));
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
