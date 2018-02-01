#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostgresConstraintKind.cs" company="RHEA System S.A.">
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
    /// <summary>
    /// Defines the available constraints for PostgreSQL
    /// </summary>
    public enum PostgresConstraintKind
    {
        /// <summary>
        /// The column can be set to NULL if required. This
        /// constraint is not widely used as all columns are NULL on default setting.
        /// </summary>
        NULL,

        /// <summary>
        /// The column cannot be set to NULL.
        /// </summary>
        NOTNULL,

        /// <summary>
        /// The column, or combination of columns has to be unique across all records.
        /// </summary>
        UNIQUE
    }
}