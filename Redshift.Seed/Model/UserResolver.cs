#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserResolver.cs" company="RHEA System S.A.">
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

    public static class UserResolver
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
                "username", typeof(string)
            },
            {
                "email", typeof(string)
            },
            {
                "password", typeof(string)
            },
            {
                "usergroup", typeof(Guid)
            }
        };

        /// <summary>
        /// Instantiate and deserialize the properties of a <paramref name="User"/>
        /// </summary>
        /// <param name="jObject">The <see cref="JObject"/> containing the data</param>
        /// <returns>The <see cref="User"/> to instantiate</returns>
        public static User FromJsonObject(JObject jObject)
        {
            var iid = jObject["uuid"].ToObject<Guid>();

            var user = new User
            {
                Uuid = iid
            };

            if (!jObject["modifiedOn"].IsNullOrEmpty())
            {
                user.ModifiedOn = jObject["modifiedOn"].ToObject<DateTime>();
            }

            if (!jObject["createdOn"].IsNullOrEmpty())
            {
                user.CreatedOn = jObject["createdOn"].ToObject<DateTime>();
            }

            if (!jObject["password"].IsNullOrEmpty())
            {
                user.Password = jObject["password"].ToObject<string>();
            }

            if (!jObject["username"].IsNullOrEmpty())
            {
                user.Username = jObject["username"].ToObject<string>();
            }

            if (!jObject["email"].IsNullOrEmpty())
            {
                user.Email = jObject["email"].ToObject<string>();
            }

            if (!jObject["usergroup"].IsNullOrEmpty())
            {
                user.Usergroup = jObject["usergroup"].ToObject<Guid>();
            }

            After(user, jObject);
            return user;
        }

        /// <summary>
        /// Fills the object with the appropriate extra properties.
        /// </summary>
        /// <param name="user">The user object.</param>
        /// <param name="jObject">The json object to deserialize from.</param>
        public static void After(User user, JObject jObject)
        {

        }
    }
}
