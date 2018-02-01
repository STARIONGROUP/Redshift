#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiDescriptionAttribute.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.Attributes
{
    using System;

    /// <summary>
    /// Description of the API object.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class ApiDescriptionAttribute : Attribute
    {
        /// <summary>
        /// The backing field for <see cref="Description"/>.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiDescriptionAttribute"/> class.
        /// </summary>
        /// <param name="description">The description.</param>
        public ApiDescriptionAttribute(string description)
        {
            this.description = description;
        }

        /// <summary>
        /// Gets the <see cref="description"/>.
        /// </summary>
        public string Description => this.description;
    }
}
