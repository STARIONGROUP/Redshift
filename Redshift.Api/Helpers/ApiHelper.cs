#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiHelper.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Api.
//
//    Redshift.Api is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Api is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Api.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Api.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.Serialization;
    using System.Text;

    using Attributes;

    using Json;

    using Nancy;
    using Nancy.Responses.Negotiation;

    using NLog;

    using Redshift.Orm.EntityObject;

    /// <summary>
    /// Set of helper methods to make work with the API easier.
    /// </summary>
    public static class ApiHelper
    {
        /// <summary>
        /// The Logger
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets the type of the entity based on the string <paramref name="name"/>.
        /// </summary>
        /// <param name="nameSpace">
        /// The name Space.
        /// </param>
        /// <param name="name">
        /// The name of the entity.
        /// </param>
        /// <param name="assemblyName">
        /// The assembly Name.
        /// </param>
        /// <returns>
        /// The <see cref="Type"/> derived from the name. Null if not found.
        /// </returns>
        public static Type GetEntityTypeFromName(string nameSpace, string name, string assemblyName)
        {
            return Type.GetType($"{nameSpace}.{name}, {assemblyName}", throwOnError: false, ignoreCase: true);
        }

        /// <summary>
        /// Returns all concrete and relevant types in the data model.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <param name="nameSpace">
        /// The name space to retrieve from.
        /// </param>
        /// <returns>
        /// The list of types.
        /// </returns>
        public static IEnumerable<Type> GetAllEntityTypesFromNamespace(Assembly assembly, string nameSpace)
        {
            // get all concrete types
            return assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsClass
                    && !t.GetTypeInfo().IsAbstract
                    && t.Namespace == nameSpace
                    && !t.GetTypeInfo().GetCustomAttributes(true).Any(a => a is ApiIgnoreAttribute)
                    && !t.GetTypeInfo().GetCustomAttributes(true).Any(a => a is ApiIgnoreAttribute)
                    && !t.GetTypeInfo().GetCustomAttributes(true).Any(a => a is CompilerGeneratedAttribute));
        }

        /// <summary>
        /// Returns all relevant data model enumerations.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <param name="nameSpace">
        /// The name space to retrieve from.
        /// </param>
        /// <returns>
        /// The list of enumeration types.
        /// </returns>
        public static IEnumerable<Type> GetAllEnumsFromNamespace(Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes()
                .Where(t => t.GetTypeInfo().IsEnum
                            && t.Namespace == nameSpace);
        }

        /// <summary>
        /// Gets the identifier from string.
        /// </summary>
        /// <param name="idString">The identifier string.</param>
        /// <returns>The id in it's correct type.</returns>
        public static object GetIdFromString(string idString)
        {
            object id;

            int intId;

            if (!int.TryParse(idString, out intId))
            {
                // intercept string ids that could parse for Guid but are not such as for Token
                if (!string.IsNullOrEmpty(idString) && !idString.Contains("-"))
                {
                    // must be string
                    id = idString;
                }
                else
                {
                    // must be Guid
                    Guid guidId;
                    if (!Guid.TryParse(idString, out guidId))
                    {
                        return null;
                    }

                    id = guidId;
                }
            }
            else
            {
                id = intId;
            }

            return id;
        }

        /// <summary>
        /// Gets the errors connected to parsing the <paramref name="type"/>.
        /// </summary>
        /// <param name="negotiator">The negotiator.</param>
        /// <param name="entity">The entity name.</param>
        /// <param name="type">The entity type.</param>
        /// <param name="attributes">The found attributes.</param>
        /// <param name="context">The request context.</param>
        /// <returns>Null if no errors are found.</returns>
        public static Negotiator GetTypeErrors(Negotiator negotiator, string entity, Type type, out List<Attribute> attributes, NancyContext context)
        {
            if (type == null)
            {
                attributes = null;
                return ConstructFailResponse(
                    negotiator,
                    "type",
                    string.Format("The requested type {0} does not exist.", entity),
                    context,
                    HttpStatusCode.BadRequest);
            }

            attributes = type.GetTypeInfo().GetCustomAttributes().ToList();

            if (attributes.Any(a => a is ApiIgnoreAttribute))
            {
                return ConstructFailResponse(
                    negotiator,
                    "type",
                    string.Format("The requested type {0} does not expose an API.", entity),
                    context,
                    HttpStatusCode.BadRequest);
            }

            var contextMethod = context.Request.Method;

            RestMethods method;

            try
            {
                method = (RestMethods)Enum.Parse(typeof(RestMethods), contextMethod.ToUpper());
            }
            catch (Exception ex)
            {
                return ConstructFailResponse(
                    negotiator,
                    "type",
                    string.Format("The request method {1} for entity {0} is not recognized.", entity, contextMethod),
                    context,
                    HttpStatusCode.BadRequest,
                    ex);
            }

            if (attributes.Any(a => a is ApiIgnoreMethodAttribute)
                && type.GetTypeInfo()
                    .GetCustomAttributes(true)
                    .OfType<ApiIgnoreMethodAttribute>()
                    .First()
                    .IgnoredMethods.HasFlag(method))
            {
                return ConstructFailResponse(
                    negotiator,
                    "type",
                    string.Format("The requested type {0} does not support the method {1}.", entity, method),
                    context,
                    HttpStatusCode.BadRequest);
            }

            return null;
        }

        /// <summary>
        /// Gets the API <see cref="Type"/> errors.
        /// </summary>
        /// <param name="negotiator">The <see cref="Negotiator"/>.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="type">The type.</param>
        /// <param name="method">The method.</param>
        /// <param name="context">The request context.</param>
        /// <returns>Null if no errors are located in connection to the <see cref="Type"/>.</returns>
        public static Negotiator GetApiTypeErrors(Negotiator negotiator, string entity, Type type, RestMethods method, NancyContext context)
        {
            if (type == null)
            {
                return ConstructFailResponse(
                    negotiator,
                    "type",
                    string.Format("The requested type {0} does not expose an API.", entity),
                    context,
                    HttpStatusCode.BadRequest);
            }

            var attributes = type.GetTypeInfo().GetCustomAttributes().ToList();

            if (attributes.Any(a => a is ApiIgnoreMethodAttribute)
                && type.GetTypeInfo()
                    .GetCustomAttributes(true)
                    .OfType<ApiIgnoreMethodAttribute>()
                    .First()
                    .IgnoredMethods.HasFlag(method))
            {
                return ConstructFailResponse(
                    negotiator,
                    "type",
                    string.Format("The requested type {0} does not support the method {1}.", entity, method),
                    context,
                    HttpStatusCode.BadRequest);
            }

            return null;
        }

        /// <summary>
        /// Constructs the fail response.
        /// </summary>
        /// <param name="negotiator">
        /// The <see cref="Negotiator"/>.
        /// </param>
        /// <param name="wrapperName">
        /// Name of the wrapper.
        /// </param>
        /// <param name="packageMessage">
        /// The package message.
        /// </param>
        /// <param name="context">
        /// The request context.
        /// </param>
        /// <param name="code">
        /// The status code.
        /// </param>
        /// <param name="e">
        /// The optional exception used for logging.
        /// </param>
        /// <returns>
        /// The complete JSON response.
        /// </returns>
        public static Negotiator ConstructFailResponse(Negotiator negotiator, string wrapperName, string packageMessage, NancyContext context, HttpStatusCode code = HttpStatusCode.NotFound, Exception e = null)
        {
            if (e != null)
            {
                Logger.Error(e, "Fail response initiated.");
            }

            var wrapper = new Dictionary<string, string> { { wrapperName, packageMessage } };

            var jsendObject = new JSendObject
            {
                Status = StatusMessage.fail,
                StatusCode = code,
                Route = context.Request.Url,
                Method = context.Request.Method,
                Data = wrapper
            };

            return negotiator
                .WithModel(jsendObject)
                .WithAllowedMediaRange("application/json")
                .WithStatusCode(code)
                .WithAllowedMediaRange("text/html");
        }

        /// <summary>
        /// Constructs the error response.
        /// </summary>
        /// <param name="negotiator">The <see cref="Negotiator"/>.</param>
        /// <param name="e">The exception.</param>
        /// <param name="context">The request context.</param>
        /// <returns>The complete JSON response.</returns>
        public static Negotiator ConstructErrorResponse(Negotiator negotiator, Exception e, NancyContext context)
        {
            var innerExceptionMessage = e.InnerException == null ? string.Empty : e.InnerException.Message;

            // do not expose the exception in the return. Log it and notify the user to contact the administrator.
            var logMessage = string.Format(
                "Server error. Exception: {1} Error: {0}. {2} ",
                e.Message,
                e.GetType().Name,
                innerExceptionMessage);

            // log the message
            Logger.Error(e, logMessage);

            var message = "Server error. Consult the server logs for details.";

            var jsendObject = new JSendErrorObject
                                  {
                                      Message = message,
                                      StatusCode = HttpStatusCode.InternalServerError,
                                      Method = context.Request.Method,
                                      Route = context.Request.Url
                                  };

            return negotiator
                .WithModel(jsendObject)
                .WithAllowedMediaRange("application/json")
                .WithAllowedMediaRange("text/html")
                .WithStatusCode(HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// Constructs the success response.
        /// </summary>
        /// <param name="negotiator">The <see cref="Negotiator"/>.</param>
        /// <param name="container">The response container.</param>
        /// <param name="context">The context of the request.</param>
        /// <param name="statusCode">The status code to provide with this response. 200 OK is default and this is usually perfectly fine, but more specific codes can be provided.</param>
        /// <returns>The complete JSON response</returns>
        public static Negotiator ConstructSuccessResponse(Negotiator negotiator, ResponseContainer container, NancyContext context, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var jsendObject = new JSendObject
            {
                Status = StatusMessage.success,
                StatusCode = statusCode,
                Route = context.Request.Url,
                Method = context.Request.Method,
                Data = container
            };

            return negotiator
                .WithModel(jsendObject)
                .WithAllowedMediaRange("application/json")
                .WithAllowedMediaRange("text/html")
                .WithStatusCode(statusCode);
        }

        /// <summary>
        /// Constructs the property table to display in the API page.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to construct the table for.</param>
        /// <returns>The html representation of the table.</returns>
        public static string ConstructPropertyTable(Type type)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<table class=\"table table-striped\" id=\"newspaper-a\">");
            sb.AppendLine("<thead><tr><th>Parameter</th><th>Type</th><th>Description</th></tr><thead><tbody>");

            foreach (var property in type.GetApiProperties())
            {
                sb.AppendLine($"<tr><td><code>{property.Name.ToCamelCase()}<code></td><td>{GetApiPropertyTypeDescription(property.PropertyType)}</td><td>{GetApiDescriptionFromProperty(property)}</td></tr>");
            }

            sb.AppendLine("</tbody></table>");

            return sb.ToString();
        }

        /// <summary>
        /// Constructs the enumeration value table to display in the API page.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to construct the table for.</param>
        /// <returns>The html representation of the table.</returns>
        public static string ConstructEnumTable(Type type)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<table class=\"table table-striped\" id=\"newspaper-a\">");
            sb.AppendLine("<thead><tr><th>Value</th><th>Description</th></tr><thead><tbody>");

            foreach (var value in Enum.GetValues(type))
            {
                sb.AppendLine($"<tr><td><code>{value}<code></td><td>{string.Join(" ", value.GetAttributeOfType<ApiDescriptionAttribute>().Select(d => d.Description))}</td></tr>");
            }

            sb.AppendLine("</tbody></table>");

            return sb.ToString();
        }

        /// <summary>
        /// Gets all the API serializable properties of the supplied <see cref="Type"/>.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The list of all properties serializable by the API.
        /// </returns>
        public static IEnumerable<PropertyInfo> GetApiProperties(this Type type)
        {
            return type.GetProperties().Where(p => !p.IsDefined(typeof(IgnoreDataMemberAttribute)) && !p.IsDefined(typeof(ApiIgnoreAttribute)));
        }

        /// <summary>
        /// Gets the API property type description.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>The type descriptor as a string.</returns>
        public static string GetApiPropertyTypeDescription(Type propertyType)
        {
            var name = propertyType.Name;

            if (propertyType.GetTypeInfo().IsGenericType &&
               propertyType.GetTypeInfo().GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                name = $"{propertyType.GenericTypeArguments.First().Name}?";
            }

            if (propertyType.IsArray
                || (propertyType.GetTypeInfo().IsGenericType
                    && propertyType.GetTypeInfo().GetGenericTypeDefinition() == typeof(List<>)))
            {
                name = $"List<{propertyType.GenericTypeArguments.First().Name}>";
            }

            return $"<b>{name}</b>";
        }

        /// <summary>
        /// Gets the API description from <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>The full description from any number of API description attributes placed on a Property.</returns>
        public static string GetApiDescriptionFromProperty(PropertyInfo property)
        {
            return property.GetCustomAttributes().OfType<ApiDescriptionAttribute>().FirstOrDefault()?.Description;
        }

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType{DescriptionAttribute}().Description;</example>
        public static List<T> GetAttributeOfType<T>(this object enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false).ToList();
            return (attributes.Count > 0) ? attributes.OfType<T>().ToList() : null;
        }

        /// <summary>
        /// Culls the API <see langword="null"/> values.
        /// </summary>
        /// <param name="resp">The response collection.</param>
        public static void CullApiNullValues(List<IEntityObject> resp)
        {
            foreach (var entity in resp)
            {
                CullApiNullValues(entity);
            }
        }

        /// <summary>
        /// Culls the API <see langword="null"/> values.
        /// </summary>
        /// <param name="resp">The response object.</param>
        public static void CullApiNullValues(IEntityObject resp)
        {
            var properties = resp.GetType().GetProperties().Where(p => p.IsDefined(typeof(ApiSerializeNullAttribute)));
            foreach (var property in properties)
            {
                property.SetValue(resp, null);
            }
        }
    }
}