#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiModel.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using Helpers;

    /// <summary>
    /// The model for the API.
    /// </summary>
    public class ApiModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiModel"/> class.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <param name="nameSpace">
        /// The name space of the model.
        /// </param>
        public ApiModel(Assembly assembly, string nameSpace)
        {
            this.Entities = new List<ApiClassModel>();
            this.Enums = new List<ApiEnumModel>();

            this.PopulateModel(assembly, nameSpace);
            this.ParseLinks();
        }

        /// <summary>
        /// Gets or sets he version of the API.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets the dictionary for entity linking.
        /// </summary>
        public Dictionary<string, string> LinkDictionary { get; private set; }

        /// <summary>
        /// Gets the list of API entities.
        /// </summary>
        public List<ApiClassModel> Entities { get; }

        /// <summary>
        /// Gets the list of API enumerations.
        /// </summary>
        public List<ApiEnumModel> Enums { get; }

        /// <summary>
        /// Parses the entire API model to substitute links.
        /// </summary>
        private void ParseLinks()
        {
            this.PopulateLinkDictionary();

            foreach (var apiClassModel in this.Entities)
            {
                foreach (var word in this.LinkDictionary.Keys)
                {
                    apiClassModel.Description = Regex.Replace(apiClassModel.Description, word, this.LinkDictionary[word]);
                }
            }
        }

        /// <summary>
        /// Populates the self-linking dictionary.
        /// </summary>
        private void PopulateLinkDictionary()
        {
            this.LinkDictionary = new Dictionary<string, string>();

            foreach (var apiClassModel in this.Entities)
            {
                this.LinkDictionary.Add(apiClassModel.Name, string.Format("<a href=\"#{0}\">{0}</a>", apiClassModel.Name));
            }

            foreach (var apiEnumModel in this.Enums)
            {
                this.LinkDictionary.Add(apiEnumModel.Name, string.Format("<a href=\"#{0}\">{0}</a>", apiEnumModel.Name));
            }
        }

        /// <summary>
        /// Resolves the API entities with reflection.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <param name="nameSpace">
        /// The name space.
        /// </param>
        private void PopulateModel(Assembly assembly, string nameSpace)
        {
            var types = ApiHelper.GetAllEntityTypesFromNamespace(assembly, nameSpace);

            foreach (var type in types.OrderBy(t => t.Name))
            {
                this.Entities.Add(new ApiClassModel(type));
            }

            var enums = ApiHelper.GetAllEnumsFromNamespace(assembly, nameSpace);

            foreach (var enumer in enums.OrderBy(t => t.Name))
            {
                this.Enums.Add(new ApiEnumModel(enumer));
            }
        }
    }
}
