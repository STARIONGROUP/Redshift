#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FkDeleteBehaviorKind.cs" company="RHEA System S.A.">
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
    /// Assertion describing the possible behavior for foreign keys
    /// </summary>
    public enum FkDeleteBehaviorKind
    {
        /// <summary>
        /// NO ACTION means that if any referencing rows still exist when the constraint is checked, an error is raised
        /// </summary>
        NO_ACTION,

        /// <summary>
        /// CASCADE specifies that when a referenced row is deleted, row(s) referencing it should be automatically deleted as well
        /// </summary>
        CASCADE,

        /// <summary>
        /// RESTRICT prevents deletion of a referenced row
        /// </summary>
        RESTRICT,

        /// <summary>
        /// Set the column to null when the reference is deleted
        /// </summary>
        SET_NULL,

        /// <summary>
        /// Set the column to the default value when the reference is deleted
        /// </summary>
        SET_DEFAULT
    }
}