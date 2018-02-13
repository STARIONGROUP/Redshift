#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityColumnNameOverrideAttribute.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.
//
//    Redshift.Orm is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Orm.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Orm.Attributes
{
    using System;

    /// <summary>
    /// Facilitates possibility to override the name of the column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class EntityColumnNameOverrideAttribute : Attribute
    {
        /// <summary>
        /// Backing field for <see cref="Name"/>
        /// </summary>
        private string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityColumnNameOverrideAttribute"/> class. 
        /// </summary>
        /// <param name="name">
        /// The name to use as override.
        /// </param>
        public EntityColumnNameOverrideAttribute(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Gets the <see cref="name"/> of the column.
        /// </summary>
        public string Name => this.name;

        /// <summary>
        /// Gets the <see cref="name"/> with the escape back ticks removed
        /// </summary>
        public string CleanName => this.Name.Replace("`", string.Empty);
    }
}
