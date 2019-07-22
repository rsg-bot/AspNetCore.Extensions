using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Rocket.Surgery.AspNetCore.Mvc.Conventions;
using Rocket.Surgery.AspNetCore.Mvc.Conventions;
using Rocket.Surgery.AspNetCore.Mvc.Views;
using Rocket.Surgery.Core;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.Autofac;
using Rocket.Surgery.Extensions.DependencyInjection;

[assembly: Convention(typeof(MvcConvention))]

namespace Rocket.Surgery.AspNetCore.Mvc.Conventions
{
    /// <summary>
    /// Class MvcConvention.
    /// </summary>
    /// <seealso cref="IServiceConvention" />
    /// TODO Edit XML Comment Template for MvcConvention
    public class MvcConvention : IAutofacConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// TODO Edit XML Comment Template for Register
        public void Register(IAutofacConventionContext context)
        {
            context.WithMvc().AddOptions();
        }
    }
}
