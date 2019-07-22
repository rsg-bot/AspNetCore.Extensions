using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DiagnosticAdapter;
using Rocket.Surgery.Extensions.Serilog;
using Serilog.Context;

namespace Rocket.Surgery.AspNetCore
{
    /// <summary>
    /// <see cref="ISerilogDiagnosticListener"/> implementation that listens for evens specific to AspNetCore Mvc layer
    /// </summary>
    public class MvcDiagnosticsListener : ISerilogDiagnosticListener
    {
        private static readonly AsyncLocal<Queue<IDisposable>> ActionDisposable = new AsyncLocal<Queue<IDisposable>>();

        static Queue<IDisposable> GetOrCreateActionQueue()
        {
            var enrichers = ActionDisposable.Value;
            if (enrichers == null)
            {
                enrichers = ActionDisposable.Value = new Queue<IDisposable>();
            }
            return enrichers;
        }

        /// <inheritdoc />
        public string ListenerName { get; } = "Microsoft.AspNetCore";

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.BeforeAction' event
        /// </summary>
        [DiagnosticName("Microsoft.AspNetCore.Mvc.BeforeAction")]
        public void OnBeforeAction(HttpContext httpContext, RouteData routeData)
        {
            var list = GetOrCreateActionQueue();
            var name = GetNameFromRouteContext(list, routeData);
            name = httpContext.Request.Method + " " + name;
            list.Enqueue(LogContext.PushProperty("RequestName", name));
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.BeforeAction' event
        /// </summary>
        [DiagnosticName("Microsoft.AspNetCore.Mvc.AfterAction")]
        public void OnAfterAction()
        {
            var list = GetOrCreateActionQueue();
            while (list.Count > 0)
            {
                var item = list.Dequeue();
                item.Dispose();
            }
        }


        private static readonly AsyncLocal<Queue<IDisposable>> ViewDisposable = new AsyncLocal<Queue<IDisposable>>();

        static Queue<IDisposable> GetOrCreateViewQueue()
        {
            var enrichers = ViewDisposable.Value;
            if (enrichers == null)
            {
                enrichers = ViewDisposable.Value = new Queue<IDisposable>();
            }
            return enrichers;
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.BeforeView' event
        /// </summary>
        [DiagnosticName("Microsoft.AspNetCore.Mvc.BeforeView")]
        public void OnBeforeView(IView view)
        {
            var list = GetOrCreateViewQueue();
            list.Enqueue(LogContext.PushProperty("View", view.Path));
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.AfterView' event
        /// </summary>
        [DiagnosticName("Microsoft.AspNetCore.Mvc.AfterView")]
        public void OnAfterView()
        {
            var list = GetOrCreateViewQueue();
            while (list.Count > 0)
            {
                var item = list.Dequeue();
                item.Dispose();
            }
        }


        private static readonly AsyncLocal<Queue<IDisposable>> HandlerDisposable = new AsyncLocal<Queue<IDisposable>>();

        static Queue<IDisposable> GetOrCreateHandlerQueue()
        {
            var enrichers = HandlerDisposable.Value;
            if (enrichers == null)
            {
                enrichers = HandlerDisposable.Value = new Queue<IDisposable>();
            }
            return enrichers;
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.BeforeHandlerMethod' event
        /// </summary>
        [DiagnosticName("Microsoft.AspNetCore.Mvc.BeforeHandlerMethod")]
        public void OnBeforeHandlerMethod(ActionContext actionContext)
        {
            var list = GetOrCreateHandlerQueue();

            var name = GetNameFromRouteContext(list, actionContext.RouteData);
            name = actionContext.HttpContext.Request.Method + " " + name;
            list.Enqueue(LogContext.PushProperty("RequestName", name));
            list.Enqueue(LogContext.PushProperty("RequestDisplayName", actionContext.ActionDescriptor.DisplayName));
        }

        /// <summary>
        /// Diagnostic event handler method for 'Microsoft.AspNetCore.Mvc.AfterHandlerMethod' event
        /// </summary>
        [DiagnosticName("Microsoft.AspNetCore.Mvc.AfterHandlerMethod")]
        public void OnAfterHandlerMethod()
        {
            var list = GetOrCreateHandlerQueue();
            while (list.Count > 0)
            {
                var item = list.Dequeue();
                item.Dispose();
            }
        }

        private static string QueueRouteValue(Queue<IDisposable> queue, IDictionary<string, object> routeValues, string routeValueKey, string name)
        {
            routeValues.TryGetValue(routeValueKey, out var controller);
            var controllerString = controller?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(controllerString))
                QueueValue(queue, name, controllerString);
            return controllerString;
        }

        private static void QueueValue(Queue<IDisposable> queue, string name, object value)
        {
            queue.Enqueue(LogContext.PushProperty(name, value));
        }

        private static string GetNameFromRouteContext(Queue<IDisposable> queue, RouteData routeData)
        {
            string name = null;

            if (routeData.Values.Count > 0)
            {
                var routeValues = routeData.Values;

                var controllerString = QueueRouteValue(queue, routeValues, "controller", "Controller");
                var actionString = QueueRouteValue(queue, routeValues, "action", "Action");
                QueueRouteValue(queue, routeValues, "feature", "Feature");
                QueueRouteValue(queue, routeValues, "area", "Area");

                if (!string.IsNullOrEmpty(controllerString))
                {
                    name = controllerString;

                    if (!string.IsNullOrEmpty(actionString))
                    {
                        name += "/" + actionString;
                    }

                    if (routeValues.Keys.Count > 2)
                    {
                        // Add parameters
                        var sortedKeys = routeValues.Keys
                            .Where(key =>
                                !string.Equals(key, "controller", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "action", StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(key, "!__route_group", StringComparison.OrdinalIgnoreCase))
                            .OrderBy(key => key, StringComparer.OrdinalIgnoreCase)
                            .ToArray();

                        if (sortedKeys.Length <= 0) return name;
                        var arguments = string.Join(@"/", sortedKeys);
                        name += " [" + arguments + "]";
                    }
                }
            }

            return name;
        }
    }
}
