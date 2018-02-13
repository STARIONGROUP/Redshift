#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsergroupResolver.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Seed.
//
//    Redshift.Seed is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Seed is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Seed.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Seed.Model
{
    using System;
    using System.Collections.Generic;
    using Api.Json;
    using Newtonsoft.Json.Linq;

    public static class UsergroupResolver
    {
        /// <summary>
        /// Map of property names and types.
        /// </summary>
        public static readonly Dictionary<string, Type> PropertyMap = new Dictionary<string, Type>
        {
            {
                "uuid", typeof(Guid)
            },
            {
                "modifiedOn", typeof(DateTime)
            },
            {
                "createdOn", typeof(DateTime)
            },
            {
                "name", typeof(string)
            },
            {
                "permissions", typeof(string)
            }
        };

        /// <summary>
        /// Instantiate and deserialize the properties of a <paramref name="Usergroup"/>
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/> containing the data</param>
        /// <returns>The <see cref="User"/> to instantiate</returns>
        public static Usergroup FromJsonObject(JObject jObject)
        {
            var iid = jObject["uuid"].ToObject<Guid>();

            var usergroup = new Usergroup
            {
                Uuid = iid
            };

            if (!jObject["modifiedOn"].IsNullOrEmpty())
            {
                usergroup.ModifiedOn = jObject["modifiedOn"].ToObject<DateTime>();
            }

            if (!jObject["createdOn"].IsNullOrEmpty())
            {
                usergroup.CreatedOn = jObject["createdOn"].ToObject<DateTime>();
            }

            if (!jObject["name"].IsNullOrEmpty())
            {
                usergroup.Name = jObject["name"].ToObject<string>();
            }

            if (!jObject["permissions"].IsNullOrEmpty())
            {
                usergroup.Permissions.AddRange(jObject["permissions"].ToObject<IEnumerable<string>>());
            }

            After(usergroup, jObject);
            return usergroup;
        }

        /// <summary>
        /// Fills the object with the appropriate extra properties.
        /// </summary>
        /// <param name="usergroup">The user object.</param>
        /// <param name="jObject">The json object to deserialize from.</param>
        public static void After(Usergroup usergroup, JObject jObject)
        {

        }
    }
}
