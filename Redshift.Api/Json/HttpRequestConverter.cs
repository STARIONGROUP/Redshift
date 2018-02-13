#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRequestConverter.cs" company="RHEA System S.A.">
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
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Redshift.Orm.EntityObject;

    /// <summary>
    /// Complex HTTP request converter
    /// </summary>
    public class HttpRequestConverter : JsonConverter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestConverter"/> class.
        /// </summary>
        /// <param name="entityMap">
        /// The entity map.
        /// </param>
        public HttpRequestConverter(ConcurrentDictionary<string, Func<JObject, IEntityObject>> entityMap)
        {
            this.EntityMap = entityMap;
        }

        /// <summary>
        /// Gets a value indicating whether this converter supports JSON read.
        /// </summary>
        public override bool CanRead => true;

        /// <summary>
        /// Gets a value indicating whether this converter supports JSON write.
        /// </summary>
        public override bool CanWrite => false;

        /// <summary>
        /// Gets the entity map.
        /// </summary>
        public ConcurrentDictionary<string, Func<JObject, IEntityObject>> EntityMap { get; }

        /// <summary>
        /// Override of the can convert type check.
        /// </summary>
        /// <param name="objectType">
        /// The object type.
        /// </param>
        /// <returns>
        /// true if this converter is to be used.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return typeof(HttpRequest).IsAssignableFrom(objectType);
        }

        /// <summary>
        /// Write JSON.
        /// </summary>
        /// <param name="writer">
        /// The JSON writer.
        /// </param>
        /// <param name="value">
        /// The value object.
        /// </param>
        /// <param name="serializer">
        /// The JSON serializer.
        /// </param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Override of the Read JSON method.
        /// </summary>
        /// <param name="reader">
        /// The JSON reader.
        /// </param>
        /// <param name="objectType">
        /// The type information of the object.
        /// </param>
        /// <param name="existingValue">
        /// The existing object value.
        /// </param>
        /// <param name="serializer">
        /// The JSON serializer.
        /// </param>
        /// <returns>
        /// A deserialized instance.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // load object from stream
            var jsonObject = JObject.Load(reader);

            if (jsonObject == null)
            {
                throw new InvalidCastException("The data in the JSON array does not cast to JObject and thus cannot be parsed.");
            }

            var httpResponse = new HttpRequest();

            JToken dataObject;
            if (jsonObject.TryGetValue("data", out dataObject))
            {
                httpResponse.Data = ((JObject)dataObject).ToEntities(this.EntityMap).ToArray();
            }
            
            return httpResponse;
        }
    }
}