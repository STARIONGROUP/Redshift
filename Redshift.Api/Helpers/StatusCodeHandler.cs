#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusCodeHandler.cs" company="RHEA System S.A.">
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

    using Nancy;
    using Nancy.ErrorHandling;
    using Nancy.Responses.Negotiation;
    using Nancy.ViewEngines;

    using Redshift.Api.Json;

    /// <summary>
    /// Handles HTTP status codes to display a custom error page.
    /// </summary>
    /// <remarks><a href="https://blog.tommyparnell.com/custom-error-pages-in-nancy/"/></remarks>
    public class StatusCodeHandler : IStatusCodeHandler
    {
        /// <summary>
        /// The collection of tracked status codes
        /// </summary>
        private static IEnumerable<int> checks = new List<int>();

        /// <summary>
        /// Response negotiator.
        /// </summary>
        private readonly IResponseNegotiator responseNegotiator;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCodeHandler"/> class. 
        /// </summary>
        /// <param name="viewRenderer">
        /// The view renderer.
        /// </param>
        /// <param name="negotiator">
        /// The response negotiator.
        /// </param>
        public StatusCodeHandler(IViewRenderer viewRenderer, IResponseNegotiator negotiator)
        {
            this.responseNegotiator = negotiator;
        }

        /// <summary>
        /// Add status <paramref name="code"/> to tracking.
        /// </summary>
        /// <param name="code">The code to add.</param>
        public static void AddCode(int code)
        {
            AddCode(new List<int> { code });
        }

        /// <summary>
        /// Add codes to tracking.
        /// </summary>
        /// <param name="code">The codes to add.</param>
        public static void AddCode(IEnumerable<int> code)
        {
            checks = checks.Union(code);
        }

        /// <summary>
        /// Remove <paramref name="code"/> from tracking.
        /// </summary>
        /// <param name="code">The code to remove.</param>
        public static void RemoveCode(int code)
        {
            RemoveCode(new List<int> { code });
        }

        /// <summary>
        /// Remove <paramref name="code"/> from tracking.
        /// </summary>
        /// <param name="code">The code to remove.</param>
        public static void RemoveCode(IEnumerable<int> code)
        {
            checks = checks.Except(code);
        }

        /// <summary>
        /// Refresh the list.
        /// </summary>
        public static void Disable()
        {
            checks = new List<int>();
        }

        /// <summary>
        /// Returns <see langword="true"/> if the status code is monitored.
        /// </summary>
        /// <param name="statusCode">The status code to check.</param>
        /// <param name="context">The <see cref="NancyContext"/> that comes in.</param>
        /// <returns>True if the status code is tracked.</returns>
        public bool HandlesStatusCode(HttpStatusCode statusCode, NancyContext context)
        {
            return checks.Any(x => x == (int)statusCode);
        }

        /// <summary>
        /// Handle the code that comes in and display the correct page.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="context">The <see cref="NancyContext"/> that the code rides on.</param>
        public void Handle(HttpStatusCode statusCode, NancyContext context)
        {
            try
            {
                var jsendObject = new JSendObject
                {
                    Status = StatusMessage.fail,
                    StatusCode = statusCode,
                    Route = context.Request.Url,
                    Method = context.Request.Method,
                    Data = statusCode.ToString()
                };
                
                context.NegotiationContext = new NegotiationContext();

                var negotiator = new Negotiator(context)
                    .WithStatusCode(statusCode)
                    .WithModel(jsendObject)
                    .WithAllowedMediaRange("application/json")
                    .WithAllowedMediaRange("text/html");

                context.Response = this.responseNegotiator.NegotiateResponse(negotiator, context);
            }
            catch (Exception)
            {
                RemoveCode((int)statusCode);
                context.Response.StatusCode = statusCode;
            }
        }
    }
}
