using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Rocket.Surgery.AspNetCore.Mvc.Views
{
    /// <summary>
    /// Class FeatureViewLocationExpander.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander" />
    /// TODO Edit XML Comment Template for FeatureViewLocationExpander
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        /// <summary>
        /// Invoked by a <see cref="T:Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine" /> to determine the values that would be consumed by this instance
        /// of <see cref="T:Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander" />. The calculated values are used to determine if the view location
        /// has changed since the last time it was located.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Razor.ViewLocationExpanderContext" /> for the current view location
        /// expansion operation.</param>
        /// TODO Edit XML Comment Template for PopulateValues
        public void PopulateValues(ViewLocationExpanderContext context) { }

        /// <summary>
        /// Invoked by a <see cref="T:Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine" /> to determine potential locations for a view.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Razor.ViewLocationExpanderContext" /> for the current view location
        /// expansion operation.</param>
        /// <param name="viewLocations">The sequence of view locations to expand.</param>
        /// <returns>A list of expanded view locations.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// context
        /// or
        /// viewLocations
        /// </exception>
        /// <exception cref="System.NullReferenceException">ControllerActionDescriptor cannot be null.</exception>
        /// TODO Edit XML Comment Template for ExpandViewLocations
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (viewLocations == null) throw new ArgumentNullException(nameof(viewLocations));

            // Error checking removed for brevity
            if (!(context.ActionContext.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor))
            {
                throw new NullReferenceException("ControllerActionDescriptor cannot be null.");
            }

            var featureName = controllerActionDescriptor.Properties["feature"] as string;
            foreach (var location in viewLocations)
            {
                yield return location.Replace("{3}", featureName);
            }
        }
    }
}
