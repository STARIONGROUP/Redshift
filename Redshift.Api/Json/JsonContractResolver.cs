#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JsonContractResolver.cs" company="RHEA System S.A.">
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
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    using Redshift.Api.Attributes;

    /// <summary>
    /// The Json Contract resolver.
    /// </summary>
    public class JsonContractResolver : CamelCasePropertyNamesContractResolver
    {
        /// <summary>
        /// Gets the pascal case of a string
        /// </summary>
        /// <param name="name">The string to change.</param>
        /// <returns>The string converted to PascalCase.</returns>
        public static string GetPascalCase(string name)
        {
            return Regex.Replace(name, @"^\w|_\w", match => match.Value.Replace("_", string.Empty).ToUpper());
        }

        /// <summary>
        /// Cull the properties that are 
        /// </summary>
        /// <param name="type">The type to resolve</param>
        /// <param name="memberSerialization">The member specialization to be used in the converter.</param>
        /// <returns>The list of Json properties to serialize.</returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            properties =
                properties.Where(p =>
                        type.GetTypeInfo().GetProperty(GetPascalCase(p.PropertyName)).GetCustomAttributes(true).Any(a => a is ApiIgnoreAttribute) == false)
                    .ToList();

            return properties;
        }
    }
}