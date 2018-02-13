#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonProcessor.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Api.
//
//    Redshift.Api is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Api is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Api.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Api.Json
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Nancy;
    using Nancy.Responses;
    using Nancy.Responses.Negotiation;

    /// <summary>
    /// The Json request processor
    /// </summary>
    public class JsonProcessor : IResponseProcessor
    {
        /// <summary>
        /// Extension mappings.
        /// </summary>
        private static readonly IEnumerable<Tuple<string, MediaRange>> ExMappings =
            new[] { new Tuple<string, MediaRange>("json", new MediaRange("application/json")) };

        /// <summary>
        /// The public serializer.
        /// </summary>
        private readonly ISerializer publicSerializer;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonProcessor"/> class,
        /// with the provided <paramref name="serializers"/>.
        /// </summary>
        /// <param name="serializers">The serializes that the processor will use to process the request.</param>
        public JsonProcessor(IEnumerable<ISerializer> serializers)
        {
            var serializationList = serializers.ToList();

            this.publicSerializer = serializationList.FirstOrDefault(x => x is JsonApiSerializer);
        }

        /// <summary>
        /// Check whether the request can be processed.
        /// </summary>
        /// <param name="requestedMediaRange">The requested media range.</param>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        /// <returns>True if can process.</returns>
        public ProcessorMatch CanProcess(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            if (IsExactJsonContentType(requestedMediaRange))
            {
                return new ProcessorMatch
                {
                    ModelResult = MatchResult.DontCare,
                    RequestedContentTypeResult = MatchResult.ExactMatch
                };
            }

            if (IsWildcardJsonContentType(requestedMediaRange))
            {
                return new ProcessorMatch
                {
                    ModelResult = MatchResult.DontCare,
                    RequestedContentTypeResult = MatchResult.NonExactMatch
                };
            }

            return new ProcessorMatch
            {
                ModelResult = MatchResult.DontCare,
                RequestedContentTypeResult = MatchResult.NoMatch
            };
        }

        /// <summary>
        /// Process the response.
        /// </summary>
        /// <param name="requestedMediaRange">The requested medi range.</param>
        /// <param name="model">The model.</param>
        /// <param name="context">The context.</param>
        /// <returns>The response.</returns>
        public Response Process(MediaRange requestedMediaRange, dynamic model, NancyContext context)
        {
            return new JsonResponse(model, this.publicSerializer, context.Environment);
        }

        /// <summary>
        /// Gets extension mappings.
        /// </summary>
        public IEnumerable<Tuple<string, MediaRange>> ExtensionMappings
        {
            get
            {
                return ExMappings;
            }
        }

        /// <summary>
        /// Get if the json content type is exact.
        /// </summary>
        /// <param name="requestedContentType">The media range.</param>
        /// <returns>True if exact match.</returns>
        private static bool IsExactJsonContentType(MediaRange requestedContentType)
        {
            if (requestedContentType.Type.IsWildcard && requestedContentType.Subtype.IsWildcard)
            {
                return true;
            }

            return requestedContentType.Matches("application/json") || requestedContentType.Matches("text/json");
        }

        /// <summary>
        /// Get if the json content type is wildcarted.
        /// </summary>
        /// <param name="requestedContentType">The media range.</param>
        /// <returns>True if wildcard match.</returns>
        private static bool IsWildcardJsonContentType(MediaRange requestedContentType)
        {
            if (!requestedContentType.Type.IsWildcard && !string.Equals("application", requestedContentType.Type, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (requestedContentType.Subtype.IsWildcard)
            {
                return true;
            }

            var subtypeString = requestedContentType.Subtype.ToString();

            return subtypeString.EndsWith("+json", StringComparison.OrdinalIgnoreCase);
        }
    }
}
