using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Rocket.Surgery.AspNetCore.Mvc.Filters
{
    public class NotFoundExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is NotFoundException)) return;

            context.ExceptionHandled = true;
            context.Result = new NotFoundResult();
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
