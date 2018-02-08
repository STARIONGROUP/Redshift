#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SeedResolverRegistry.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Seed.
//
//    Redshift.Seed is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Seed is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
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
    using Api;

    /// <summary>
    /// Registry of the entity maps.
    /// </summary>
    public static class SeedResolverRegistry
    {
        /// <summary>
        /// Register the various types.
        /// </summary>
        public static void Register()
        {
            // abstract entity resolver
            EntityResolverMap.AbstractToConcreteMap.TryAdd("Thing", new List<Type> { typeof(User), typeof(Usergroup) });

            // type property resolver
            EntityResolverMap.TypeToPropertyResolverMap.TryAdd(typeof(User), UserResolver.PropertyMap);
            EntityResolverMap.TypeToPropertyResolverMap.TryAdd(typeof(Usergroup), UsergroupResolver.PropertyMap);

            // type property resolver
            EntityResolverMap.ApiRouteToPropertyResolverMap.TryAdd("User", UserResolver.PropertyMap);
            EntityResolverMap.ApiRouteToPropertyResolverMap.TryAdd("Usergroup", UsergroupResolver.PropertyMap);

            // deserialization resolver
            EntityResolverMap.DeserializationMap.TryAdd("user", UserResolver.FromJsonObject);
            EntityResolverMap.DeserializationMap.TryAdd("usergroup", UsergroupResolver.FromJsonObject);
        }
    }
}
