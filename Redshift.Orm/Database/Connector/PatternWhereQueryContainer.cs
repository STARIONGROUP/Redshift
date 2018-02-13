#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PatternWhereQueryContainer.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Database
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Npgsql;

    using NpgsqlTypes;

    using EntityObject;
    using Helpers;

    /// <summary>
    /// WhereQueryContainer maintains a structured way to process patterned WHERE queries.
    /// </summary>
    public class PatternWhereQueryContainer : IWhereQueryContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatternWhereQueryContainer"/> class. 
        /// </summary>
        public PatternWhereQueryContainer()
        {
            this.Properties = new List<PropertyInfo>();
        }

        /// <summary>
        /// Gets or sets the property
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// Gets or sets the property
        /// </summary>
        public List<PropertyInfo> Properties { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Returns the sql string of the where query
        /// </summary>
        /// <returns>The sql string representing this container.</returns>
        public string GetSqlString()
        {
            var queryList = new List<string>();
            foreach (var property in this.Properties)
            {
                var columnName = EntityHelper.GetColumnNameFromProperty(property);

                queryList.Add($"{columnName.MakePostgreSqlSafe()} ILIKE @{columnName}");
            }

            return $"({string.Join(" OR ", queryList)})";
        }

        /// <summary>
        /// Inserts the appropriate values into the command.
        /// </summary>
        /// <param name="cmd">The command.</param>
        public void InsertParameterValues(ref NpgsqlCommand cmd)
        {
            foreach (var property in this.Properties)
            {
                var val = this.Value;

                if (val is DateTime)
                {
                    cmd.Parameters.AddWithValue($"{EntityHelper.GetColumnNameFromProperty(property)}", NpgsqlDbType.Timestamp, ((DateTime)val).ToUniversalTime());
                }
                else
                {
                    cmd.Parameters.AddWithValue($"{EntityHelper.GetColumnNameFromProperty(property)}", val.GetType().GetTypeInfo().IsEnum ? val.ToString() : val);
                }
            }
        }
    }
}