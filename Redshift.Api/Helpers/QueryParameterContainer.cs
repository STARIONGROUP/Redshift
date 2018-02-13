#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QueryParameterContainer.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using Nancy;
    using Orm.Database;
    using Orm.EntityObject;

    /// <summary>
    /// QueryParameterContainer resolves and stores query parameters
    /// </summary>
    public class QueryParameterContainer
    {
        /// <summary>
        /// Default pagination limit.
        /// </summary> 
        private const int DefaultLimit = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParameterContainer"/> class. 
        /// </summary>
        public QueryParameterContainer()
        {
            this.FilterList = new List<IWhereQueryContainer>();
            this.Limit = -1;
        }

        /// <summary>
        /// Gets or sets the limit.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is limited.
        /// </summary>
        public bool IsLimited { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is paginated.
        /// </summary>
        public bool IsPaginated { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the order is descending.
        /// </summary>
        public bool IsDescending { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the count is expected in response.
        /// </summary>
        public bool IsCountExpected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether query is ordered.
        /// </summary>
        public bool IsOrdered { get; set; }

        /// <summary>
        /// Gets or sets the order property.
        /// </summary>
        public PropertyInfo OrderProperty { get; set; }

        /// <summary>
        /// Gets or sets the filter list.
        /// </summary>
        public List<IWhereQueryContainer> FilterList { get; set; }

        /// <summary>
        /// Gets a value indicating whether is filtered.
        /// </summary>
        public bool IsFiltered => this.FilterList.Any();

        /// <summary>
        /// Processes the request to get the correct set up for this container.
        /// </summary>
        /// <param name="request">
        /// The nancy Request to process.
        /// </param>
        /// <param name="entityType">
        /// The entity Type.
        /// </param>
        public void Process(Request request, Type entityType)
        {
            // process query parameters
            var limit = request.Query["limit"];
            var page = request.Query["page"];
            var order = request.Query["order"];
            var desc = request.Query["desc"];
            var count = request.Query["count"];

            int pageNumber, limitNumber;
            bool countExpected;

            this.IsPaginated = int.TryParse(page, out pageNumber);
            this.IsLimited = int.TryParse(limit, out limitNumber);

            var isCountProvided = bool.TryParse(count, out countExpected);

            if (isCountProvided)
            {
                this.IsCountExpected = countExpected;
            }

            if (this.IsPaginated)
            {
                this.PageNumber = pageNumber;

                if (!this.IsLimited)
                {
                    this.Limit = DefaultLimit;
                }
                else
                {
                    this.Limit = limitNumber;
                }

                this.Offset = (this.PageNumber - 1) * this.Limit;
            }

            var primaryKey = !(Activator.CreateInstance(entityType) is IEntityObject instance) ? "Uuid" : instance.PrimaryKey;

            this.OrderProperty = entityType.GetProperty(primaryKey);
            this.IsDescending = true;

            if (desc != null)
            {
                // parse descending
                bool descBool;

                var canDescParse = bool.TryParse(desc.ToString(), out descBool);
                if (canDescParse)
                {
                    this.IsDescending = descBool;
                }
            }

            // get the ordering property
            if (!string.IsNullOrEmpty(order))
            {
                var orderString = order.ToString() as string;

                var overrideOrderProperty = entityType.GetProperty(orderString.ToTitleCase());
                if (overrideOrderProperty != null)
                {
                    this.OrderProperty = overrideOrderProperty;
                    this.IsOrdered = true;
                }
            }

            // get filtering properties
            var map = EntityResolverMap.TypeToPropertyResolverMap[entityType];

            foreach (var property in map)
            {
                var val = request.Query[property.Key];

                if (val != null)
                {
                    var valueString = val.ToString() as string;

                    if (valueString == null)
                    {
                        continue;
                    }

                    var whereContainer = new WhereQueryContainer
                    {
                        Comparer = "=",
                        Property =
                            entityType.GetProperty(
                                property.Key.ToTitleCase())
                    };

                    // split by ;
                    var values = valueString.Split(new[] { ';' });

                    foreach (var value in values)
                    {
                        // parse the value into correct type
                        var typeConverter = TypeDescriptor.GetConverter(property.Value);
                        var parsedValue = typeConverter.ConvertFromString(value);

                        whereContainer.Value.Add(parsedValue);
                    }

                    this.FilterList.Add(whereContainer);
                }
            }
        }
    }
}
