#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClasslessDTO.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// A classless Data Transfer Object is a wrapper for a <see cref="Dictionary{TKey,TValue}"/>.
    /// </summary>
    public class ClasslessDTO : Dictionary<string, object>
    {
        /// <summary>
        /// Populates this dictionary with the properties of the object.
        /// </summary>
        /// <typeparam name="T">The type of the thing to consider.</typeparam>
        /// <param name="thing">The thing to transform.</param>
        /// <returns>A classless DTO of this type.</returns>
        public static ClasslessDTO FromThing<T>(T thing)
        {
            var result = new ClasslessDTO();

            if (thing == null)
            {
                throw new ArgumentNullException("thing");
            }

            foreach (var property in thing.GetType().GetProperties())
            {
                if (property.GetCustomAttributes(typeof(IgnoreDataMemberAttribute), false).Length == 0)
                {
                    var propertyValue = property.GetValue(thing);
                    if (propertyValue == null)
                    {
                        propertyValue = string.Empty;
                    }

                    result.Add(property.Name, propertyValue);
                }
            }

            return result;
        }
    }
}
