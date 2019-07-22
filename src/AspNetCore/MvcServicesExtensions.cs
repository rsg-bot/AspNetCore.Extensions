using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NodaTime;
using Newtonsoft.Json.Converters;
using NodaTime.Serialization.JsonNet;
using Rocket.Surgery.AspNetCore.Mvc.Builders;
using Rocket.Surgery.AspNetCore.Mvc.Conventions;
using Rocket.Surgery.AspNetCore.Mvc.Views;
using Rocket.Surgery.Extensions.Autofac;
using FluentValidation.AspNetCore;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.Options;
using Rocket.Surgery.AspNetCore;
using Rocket.Surgery.AspNetCore.Mvc.Filters;
using Rocket.Surgery.Extensions.Serilog;
using System;

// ReSharper disable once CheckNamespace
namespace Rocket.Surgery.Core
{
    /// <summary>
    /// Class MediatRServicesExtensions.
    /// </summary>
    public static class MvcServicesExtensions
    {
        /// <summary>
        /// Withes the mediat r.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns>MediatRBuilder.</returns>
        public static MvcBuilder WithMvc(this IAutofacConventionContext parent)
        {
            return new MvcBuilder(parent);
        }

        /// <summary>
        /// The locations
        /// </summary>
        /// TODO Edit XML Comment Template for Locations
        private static readonly string[] Locations = {
            "/{3}/{1}/{0}.cshtml",
            "/{3}/{0}.cshtml",
            "/{3}/{1}.cshtml",
            "/Shared/{0}.cshtml",
            "/Views/{0}.cshtml",
            "/Views/{1}.cshtml",
        };

        /// <summary>
        /// Adds the options.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>MvcBuilder.</returns>
        /// TODO Edit XML Comment Template for AddOptions
        public static MvcBuilder AddOptions(this MvcBuilder builder)
        {
            builder.Parent.Services.Configure<MvcJsonOptions>(options =>
            {
                options.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                options.SerializerSettings.Converters.Add(new StringEnumConverter(true));
            });

            builder.Parent.Services.AddSingleton(_ =>
                _.GetRequiredService<IOptions<MvcJsonOptions>>().Value.SerializerSettings);

            builder.Parent.Services.AddRouting(options =>
            {
                options.AppendTrailingSlash = true;
                options.LowercaseUrls = true;
            });

            builder.Parent.Services.AddTransient<ISerilogDiagnosticListener, MvcDiagnosticsListener>();

            builder.Parent.Services
                .AddMvc(options =>
                {
                    options.Conventions.Add(new FeatureConvention());
                    options.Filters.Add<NotFoundExceptionFilter>();
                    options.Filters.Add<ValidationExceptionFilter>();
                })
                .AddFluentValidation(c => {
                    c.ValidatorFactoryType = builder.Parent.Services
                        .FirstOrDefault(x => x.ServiceType == typeof(IValidatorFactory))?.ImplementationType;
                })
                .AddRazorOptions(options =>
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
            return builder;
        }
    }
}
