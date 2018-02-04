#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityResolverMap.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Api.
//
//    Redshift.Orm is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Orm.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Api
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// Utility class that is responsible for instantiating a <see cref="Thing"/>.
    /// TODO: Create model meta data registration mechanism.
    /// </summary>
    public static class EntityResolverMap
    {
        /// <summary>
        /// The map to resolve concrete types from abstract.
        /// </summary>
        public static readonly ConcurrentDictionary<string, List<Type>> AbstractToConcreteMap = new ConcurrentDictionary<string, List<Type>>();

        /// <summary>
        /// The map to resolve types to property maps
        /// </summary>
        public static readonly ConcurrentDictionary<Type, Dictionary<string, Type>> TypeToPropertyResolverMap = new ConcurrentDictionary<Type, Dictionary<string, Type>>();

        /// <summary>
        /// The map to resolve entityroutes to property maps
        /// </summary>
        public static readonly ConcurrentDictionary<string, Dictionary<string, Type>> ApiRouteToPropertyResolverMap = new ConcurrentDictionary<string, Dictionary<string, Type>>();

        /// <summary>
        /// The concrete entity resolver
        /// </summary>
        public static readonly ConcurrentDictionary<string, Func<JObject, IEntityObject>> DeserializationMap = new ConcurrentDictionary<string, Func<JObject, IEntityObject>>();
    }
}
