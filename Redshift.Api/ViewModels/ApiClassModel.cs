#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiClassModel.cs" company="RHEA System S.A.">
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
    /// Defines a Class API descriptor.
    /// </summary>
    public class ApiClassModel
    {
        /// <summary>
        /// The root path of the API route.
        /// </summary>
        private readonly string apiRootPath = "api/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClassModel"/> class. 
        /// </summary>
        /// <param name="type">
        /// The <see cref="Type"/> of the entity.
        /// </param>
        public ApiClassModel(Type type)
        {
            this.Type = type;

            this.ResolveInformation();
            this.ResolveProperties();
        }

        /// <summary>
        /// Gets the type.
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
        /// Gets or sets a value indicating whether an alert should be printed.
        /// </summary>
        public bool HasAlert { get; set; }

        /// <summary>
        /// Gets or sets the alert.
        /// </summary>
        public string Alert { get; set; }

        /// <summary>
        /// Gets the route
        /// </summary>
        public string Route => string.Format("{0}{2}{1}", "/", this.Name, this.apiRootPath);

        /// <summary>
        /// Gets the route to the entity
        /// </summary>
        public string RouteEntity => string.Format("{0}{2}{1}/{{uuid}}", "/", this.Name, this.apiRootPath);

        /// <summary>
        /// Gets the fully qualified route to the entity
        /// </summary>
        public string FullRouteEntity
        {
            get
            {
                const string Root = "http://localhost:5000/";

                return string.Format("{1}{2}{0}/{{id}}", this.Name, Root, this.apiRootPath);
            }
        }

        /// <summary>
        /// Gets the link.
        /// </summary>
        public string Link => string.Format("<a href=\"{0}\">{0}</a>", this.Route);

        /// <summary>
        /// Gets the link to the entity.
        /// </summary>
        public string LinkEntity => string.Format("{0}/{{uuid}}", this.Route);

        /// <summary>
        /// Gets or sets the list of properties
        /// </summary>
        public string PropertiesTable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this route supports a GET
        /// </summary>
        public bool HasGet { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this route supports a POST
        /// </summary>
        public bool HasPost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this route supports a PATCH
        /// </summary>
        public bool HasPatch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this route supports a DELETE
        /// </summary>
        public bool HasDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this route has ignored methods
        /// </summary>
        public bool HasIgnoredMethods { get; set; }

        /// <summary>
        /// Gets or sets the message to display in case of ignored methods.
        /// </summary>
        public string IgnoredMethods { get; set; }

        /// <summary>
        /// Gets the edit claim name.
        /// </summary>
        //public string EditClaim => PermissionService.GetEditClaimNameFromEntity(this.Type);

        /// <summary>
        /// Gets the delete claim name.
        /// </summary>
        //public string DeleteClaim => PermissionService.GetDeleteClaimNameFromEntity(this.Type);

        /// <summary>
        /// Resolves the information for the class.
        /// </summary>
        private void ResolveInformation()
        {
            this.Name = this.Type.Name;
            var attributes = this.Type.GetTypeInfo().GetCustomAttributes(true).ToList();

            this.Description = attributes.OfType<ApiDescriptionAttribute>().FirstOrDefault()?.Description ?? "No description available at this time.";
            this.Alert = string.Join(" ", attributes.OfType<ApiWarningAttribute>().Select(a => a.Message));

            if (!string.IsNullOrEmpty(this.Alert))
            {
                this.HasAlert = true;
            }

            this.HasGet = true;
            this.HasPost = true;
            this.HasPatch = true;
            this.HasDelete = true;

            var ignoredMethods = attributes.OfType<ApiIgnoreMethodAttribute>().FirstOrDefault();
            if (ignoredMethods != null)
            {
                this.HasGet = !ignoredMethods.IgnoredMethods.HasFlag(RestMethods.GET);
                this.HasPost = !ignoredMethods.IgnoredMethods.HasFlag(RestMethods.POST);
                this.HasPatch = !ignoredMethods.IgnoredMethods.HasFlag(RestMethods.PATCH);
                this.HasDelete = !ignoredMethods.IgnoredMethods.HasFlag(RestMethods.DELETE);

                this.HasIgnoredMethods = true;
                this.IgnoredMethods = ignoredMethods.Message;
            }
        }

        /// <summary>
        /// Resolves the properties for the class.
        /// </summary>
        private void ResolveProperties()
        {
            this.PropertiesTable = ApiHelper.ConstructPropertyTable(this.Type);
        }
    }
}