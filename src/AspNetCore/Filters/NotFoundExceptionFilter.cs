using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Rocket.Surgery.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Not found exception that catches not found messages that might have been thrown by calling code.
    /// </summary>
    public class NotFoundExceptionFilter : IExceptionFilter, IAsyncExceptionFilter
    {
        /// <inheritdoc />
        public void OnException(ExceptionContext context)
        {
            if (!(context.Exception is NotFoundException)) return;

            context.ExceptionHandled = true;
            context.Result = new NotFoundResult();
        }

        /// <inheritdoc />
        public Task OnExceptionAsync(ExceptionContext context)
        {
            OnException(context);
            return Task.CompletedTask;
        }
    }
}
