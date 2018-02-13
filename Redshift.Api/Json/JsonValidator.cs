#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonValidator.cs" company="RHEA System S.A.">
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

    using Newtonsoft.Json.Linq;

    using Redshift.Orm.EntityObject;

    /// <summary>
    /// Validates JSON objects against the types.
    /// </summary>
    public class JsonValidator
    {
        /// <summary>
        /// The serializer
        /// </summary>
        private JsonApiSerializer serializer = new JsonApiSerializer();

        /// <summary>
        /// Processes the complex POST body for a list of entities/ 
        /// </summary>
        /// <param name="body">
        /// The complex post body.
        /// </param>
        /// <param name="entityMap">
        /// The entity Map.
        /// </param>
        /// <returns>
        /// A HTTP request object with all the entities.
        /// </returns>
        public HttpRequest Process(string body, ConcurrentDictionary<string, Func<JObject, IEntityObject>> entityMap)
        {
            this.serializer.DeserializationMap = entityMap;

            var result = this.serializer.Deserialize(body);

            return result;
        }
    }
}
