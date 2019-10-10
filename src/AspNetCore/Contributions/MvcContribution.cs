using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Rocket.Surgery.AspNetCore.Mvc.Conventions;
using Rocket.Surgery.AspNetCore.Mvc.Views;
using Rocket.Surgery.Conventions;
using Rocket.Surgery.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Rocket.Surgery.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;

[assembly: Convention(typeof(AspNetCoreConvention))]

namespace Rocket.Surgery.AspNetCore.Mvc.Conventions
{
    /// <summary>
    /// Class MvcConvention.
    /// </summary>
    /// <seealso cref="IServiceConvention" />
    /// TODO Edit XML Comment Template for MvcConvention
    public class AspNetCoreConvention : IServiceConvention
    {
        /// <summary>
        /// Registers the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// TODO Edit XML Comment Template for Register
        public void Register(IServiceConventionContext context)
        {
            context.Services.Configure<RazorViewEngineOptions>(options =>
            {
                // {0} - Action Name
                // {1} - Controller Name
                // {2} - Area Name
                // {3} - Feature Name
                // Replace normal view location entirely
                for (var i = Locations.Length - 1; i >= 0; i--)
                {
                    options.AreaViewLocationFormats.Insert(0, $"/Areas/{{2}}{Locations[i]}");
                }

                for (var i = Locations.Length - 1; i >= 0; i--)
                {
                    options.ViewLocationFormats.Insert(0, Locations[i]);
                }

                options.ViewLocationExpanders.Add(new FeatureViewLocationExpander());
            });

            context.Services.Configure<MvcOptions>(options =>
            {
                options.Conventions.Add(new FeatureConvention());
                options.Filters.Add<NotFoundExceptionFilter>();
            });
        }

        /// <summary>
        /// The locations
        /// </summary>
        private static readonly string[] Locations = {
            "/{3}/{1}/{0}.cshtml",
            "/{3}/{0}.cshtml",
            "/{3}/{1}.cshtml",
            "/Shared/{0}.cshtml",
            "/Views/{0}.cshtml",
            "/Views/{1}.cshtml",
        };
    }
}
