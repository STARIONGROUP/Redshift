#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WhereQueryContainer.cs" company="RHEA System S.A.">
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
    /// WhereQueryContainer maintains a structured way to process WHERE queries.
    /// </summary>
    public class WhereQueryContainer : IWhereQueryContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhereQueryContainer"/> class. 
        /// </summary>
        public WhereQueryContainer()
        {
            this.Value = new List<object>();
            this.IsUsingAndConditionBetweenValues = false;
        }

        /// <summary>
        /// Gets or sets the property
        /// </summary>
        public PropertyInfo Property { get; set; }

        /// <summary>
        /// Gets or sets the comparative string
        /// </summary>
        public string Comparer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an AND or and OR condition should be used between comparative expressions.
        /// </summary>
        public bool IsUsingAndConditionBetweenValues { get; set; }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public List<object> Value { get; set; }

        /// <summary>
        /// Returns the sql string of the where query
        /// </summary>
        /// <returns>The sql string representing this container.</returns>
        public string GetSqlString()
        {
            var queryList = new List<string>();

            var columnName = EntityHelper.GetColumnNameFromProperty(this.Property);

            for (int i = 0; i < this.Value.Count; i++)
            {
                queryList.Add($"{columnName.MakePostgreSqlSafe()} {this.Comparer} @{columnName}{this.GetHashCode()}{i}");
            }

            return $"({string.Join(this.IsUsingAndConditionBetweenValues? " AND " : " OR ", queryList)})";
        }

        /// <summary>
        /// Inserts the appropriate values into the command.
        /// </summary>
        /// <param name="cmd">The command.</param>
        public void InsertParameterValues(ref NpgsqlCommand cmd)
        {
            for (int i = 0; i < this.Value.Count; i++)
            {
                var val = this.Value[i];

                if (val is DateTime time)
                {
                    cmd.Parameters.AddWithValue($"{EntityHelper.GetColumnNameFromProperty(this.Property)}{this.GetHashCode()}{i}", NpgsqlDbType.Timestamp, time.ToUniversalTime());
                }
                else
                {
                    cmd.Parameters.AddWithValue($"{EntityHelper.GetColumnNameFromProperty(this.Property)}{this.GetHashCode()}{i}", val.GetType().GetTypeInfo().IsEnum ? val.ToString() : val);
                }
            }
        }
    }
}