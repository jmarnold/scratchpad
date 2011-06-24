using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FubuCore.Reflection;
using FubuMVC.Core;
using FubuMVC.Core.Registration.DSL;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;
using Scratchpad.Web.Configuration.Policies;
using Scratchpad.Web.Endpoints;

namespace Scratchpad.Web.Configuration
{
    public static class FubuExtensions
    {
        private static readonly HashSet<string> HttpVerbs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { "GET", "POST", "PUT", "HEAD" };

        public static void ApplyEndpointConventions(this FubuRegistry registry)
        {
            registry
                .Actions
                .IncludeEndpoints(typeof(EndpointMarker));

            registry
                .Routes
                .UrlPolicy<EndpointUrlPolicy>();

            registry
                .Routes
                .ConstraintEndpointMethods();
        }

        public static void ConstraintEndpointMethods(this RouteConventionExpression routes)
        {
            HttpVerbs
                .Each(verb => routes.ConstrainToHttpMethod(action => action.Method.Name.Equals(verb, StringComparison.InvariantCultureIgnoreCase), verb));
        }

        public static ActionCallCandidateExpression IncludeEndpoints(this ActionCallCandidateExpression actions, params Type[] markerTypes)
        {
            var markers = new List<Type>(markerTypes);
            markers.Each(markerType => actions.IncludeTypes(t => t.Namespace.StartsWith(markerType.Namespace) && t.Name.EndsWith(EndpointUrlPolicy.ENDPOINT) && !t.IsAbstract));
            return actions.IncludeMethods(action => HttpVerbs.Contains(action.Method.Name));
        }

        public static void AddRouteInput<T>(this IRouteDefinition route, Expression<Func<T, object>> expression, bool appendToUrl)
        {
            Accessor accessor = ReflectionHelper.GetAccessor(expression);
            route.Input.AddRouteInput(new RouteParameter(accessor), appendToUrl);
        }

        public static void ApplyRouteInputAttributes(this IRouteDefinition routeDefinition, ActionCall call)
        {
            if (call.HasInput)
            {
                call
                    .InputType()
                    .PropertiesWhere(p => p.HasAttribute<RouteInputAttribute>())
                    .Each(p => routeDefinition.Input.AddRouteInput(new RouteParameter(new SingleProperty(p)), true));
            }
        }

        public static void ApplyQueryStringAttributes(this IRouteDefinition routeDefinition, ActionCall call)
        {
            if (call.HasInput)
            {
                call
                    .InputType()
                    .PropertiesWhere(p => p.HasAttribute<QueryStringAttribute>())
                    .Each(routeDefinition.Input.AddQueryInput);
            }
        }

        public static IEnumerable<PropertyInfo> PropertiesWhere(this Type type, Func<PropertyInfo, bool> predicate)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            return properties.Where(predicate);
        }    
    }
}