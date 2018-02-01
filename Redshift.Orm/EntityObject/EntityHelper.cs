#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityHelper.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.
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

namespace Redshift.Orm.EntityObject
{
    using System.Reflection;
    using Attributes;

    /// <summary>
    /// Helper methods for Entity objects.
    /// </summary>
    public static class EntityHelper
    {
        /// <summary>
        /// Gets the column name.
        /// </summary>
        /// <param name="x">The <see cref="PropertyInfo"/> to extract name from.</param>
        /// <param name="clean">Indicates whether or not to get the clean name.</param>
        /// <returns>The name of the column linked to this property.</returns>
        public static string GetColumnNameFromProperty(PropertyInfo x, bool clean = false)
        {
            if (x.IsDefined(typeof(EntityColumnNameOverrideAttribute)))
            {
                var nameOverrideAttribute = (EntityColumnNameOverrideAttribute)x.GetCustomAttribute(typeof(EntityColumnNameOverrideAttribute));

                return string.Format("{0}", clean ? nameOverrideAttribute.CleanName : nameOverrideAttribute.Name);
            }

            return x.Name.ToLower();
        }

        /// <summary>
        /// Reflects the <see cref="PropertyInfo"/> from a string name.
        /// </summary>
        /// <typeparam name="T">
        /// The type of entity object.
        /// </typeparam>
        /// <param name="thing">
        /// The thing to reflect from.
        /// </param>
        /// <param name="propertyName">
        /// The string name of the property to get.
        /// </param>
        /// <returns>
        /// The <see cref="PropertyInfo"/> resolved from a string.
        /// </returns>
        public static PropertyInfo GetPropertyInfoFromName<T>(this T thing, string propertyName) where T : IEntityObject
        {
            return thing.GetType().GetTypeInfo().GetProperty(propertyName);
        }
    }
}
