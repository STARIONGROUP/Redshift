#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonApiSerializer.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.Json
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;

    using Nancy;
    using Nancy.Json;
    using Nancy.Responses.Negotiation;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Linq;

    using Redshift.Orm.EntityObject;

    /// <summary>
    /// The JSON custom serializer.
    /// </summary>
    public class JsonApiSerializer : ISerializer
    {
        /// <summary>
        /// The backing field for the serializer.
        /// </summary>
        private readonly JsonSerializer serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonApiSerializer"/> class. 
        /// </summary>
        public JsonApiSerializer()
        {
            // set default newtonsoft settings
            JsonConvert.DefaultSettings = GetSerializerSettings;
            
            this.serializer = GetSerializer();
        }

        /// <summary>
        /// Gets or sets the entity map.
        /// </summary>
        public ConcurrentDictionary<string, Func<JObject, IEntityObject>> DeserializationMap { get; set; }

        /// <summary>
        /// Gets the list of extensions.
        /// </summary>
        public IEnumerable<string> Extensions
        {
            get
            {
                yield return "json";
            }
        }

        /// <summary>
        /// Gets the custom JsonSerializer
        /// </summary>
        /// <returns>The custom serializer.</returns>
        public static JsonSerializer GetSerializer()
        {
            var settings = GetSerializerSettings();

            var serializer = JsonSerializer.Create(settings);
            serializer.Converters.Add(new StringEnumConverter
            {
                AllowIntegerValues = false,
                CamelCaseText = true
            });

            return serializer;
        }

        /// <summary>
        /// Gets the json serialization settings.
        /// </summary>
        /// <returns>The settings.</returns>
        public static JsonSerializerSettings GetSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new JsonContractResolver()
            };
        }

        /// <summary>
        /// Deserialize the complex request.
        /// </summary>
        /// <param name="body">The post body.</param>
        /// <returns>The populated request object.</returns>
        public HttpRequest Deserialize(string body)
        {
            if (this.DeserializationMap == null)
            {
                throw new Exception("The entity map is not set for the deserializer.");
            }

            this.serializer.Converters.Add(new HttpRequestConverter(this.DeserializationMap));

            HttpRequest data;

            using (var stringReader = new StringReader(body))
            using (var jsonTextReader = new JsonTextReader(stringReader))
            {
                data = this.serializer.Deserialize<HttpRequest>(jsonTextReader);
            }

            return data;
        }

        /// <summary>
        /// Returns whether this serializer can serialize the content.
        /// </summary>
        /// <param name="mediaRange">
        /// The content type.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/> indicating whether serialization is possible.
        /// </returns>
        public bool CanSerialize(MediaRange mediaRange)
        {
            return Json.IsJsonContentType(mediaRange);
        }

        /// <summary>
        /// Serializes the content.
        /// </summary>
        /// <param name="mediaRange">
        /// The content type.
        /// </param>
        /// <param name="model">
        /// The model.
        /// </param>
        /// <param name="outputStream">
        /// The output stream.
        /// </param>
        /// <typeparam name="TModel">
        /// The type of the model to serialize.
        /// </typeparam>
        public void Serialize<TModel>(MediaRange mediaRange, TModel model, Stream outputStream)
        {
            using (var writer = new JsonTextWriter(new StreamWriter(outputStream))
                {
                    CloseOutput = true,
                    Formatting = Formatting.Indented
                })
            {
                this.serializer.Serialize(writer, model);
                writer.Flush();
            }
        }
    }
}
