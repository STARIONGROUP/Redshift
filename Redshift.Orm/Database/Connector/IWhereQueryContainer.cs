#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWhereQueryContainer.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Database
{
    using Npgsql;

    /// <summary>
    /// IWhereQueryContainer is the interface for various types of query containers.
    /// </summary>
    public interface IWhereQueryContainer
    {
        /// <summary>
        /// Gets the sql string representing the where statements.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string GetSqlString();

        /// <summary>
        /// Inserts the appropriate values into the command.
        /// </summary>
        /// <param name="cmd">The command.</param>
        void InsertParameterValues(ref NpgsqlCommand cmd);
    }
}