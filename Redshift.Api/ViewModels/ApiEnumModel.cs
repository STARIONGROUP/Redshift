#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiEnumModel.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.ViewModels
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Redshift.Api.Attributes;
    using Redshift.Api.Helpers;

    /// <summary>
    /// Defines a Class API <see langword="enum"/> descriptor.
    /// </summary>
    public class ApiEnumModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiEnumModel"/> class. 
        /// </summary>
        /// <param name="type">
        /// The type of the enum.
        /// </param>
        public ApiEnumModel(Type type)
        {
            this.Type = type;

            this.ResolveInformation();
            this.ResolveLiterals();
        }

        /// <summary>
        /// Gets the type
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the list of properties
        /// </summary>
        public string LiteralsTable { get; private set; }

        /// <summary>
        /// Resolves the properties.
        /// </summary>
        private void ResolveInformation()
        {
            this.Name = this.Type.Name;
            var attributes = this.Type.GetTypeInfo().GetCustomAttributes(true).ToList();

            this.Description = attributes.OfType<ApiDescriptionAttribute>().FirstOrDefault()?.Description ?? "No description available at this time.";
        }

        /// <summary>
        /// Resolves the literal values.
        /// </summary>
        private void ResolveLiterals()
        {
            this.LiteralsTable = ApiHelper.ConstructEnumTable(this.Type);
        }
    }
}