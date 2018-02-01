#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityFactory.cs" company="RHEA System S.A.">
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
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;

    using Redshift.Orm.EntityObject;

    /// <summary>
    /// Utility class that is responsible for instantiating a <see cref="EntityObject"/>s
    /// </summary>
    public static class EntityFactory
    {
        /// <summary>
        /// Creates a collection of <see cref="Thing"/> from a <see cref="JObject"/>
        /// </summary>
        /// <param name="dataObject">
        /// The <see cref="JObject"/> containing the data
        /// </param>
        /// <param name="entityConstructorMap">
        /// The entity Constructor Map.
        /// </param>
        /// <returns>
        /// The collection of <see cref="Thing"/>
        /// </returns>
        public static IEnumerable<IEntityObject> ToEntities(this JObject dataObject, Dictionary<string, Func<JObject, IEntityObject>> entityConstructorMap)
        {
            foreach (var property in dataObject.Properties())
            {
                Func<JObject, IEntityObject> constructor;
                if (!entityConstructorMap.TryGetValue(property.Name, out constructor))
                {
                    throw new InvalidOperationException(string.Format("The entity resolver was not found for {0}", property.Name));
                }

                foreach (JObject jsonObj in property.Value)
                {
                    yield return constructor(jsonObj);
                }
            }
        }
    }
}