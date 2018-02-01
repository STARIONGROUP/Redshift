#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntityObject.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.EntityObject
{
    using System;

    /// <summary>
    /// The interface of the ORM object
    /// </summary>
    public interface IEntityObject
    {
        /// <summary>
        /// Gets the SQL table name that contains this concept.
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Gets the literal string name of the property that is used as the primary unique key.
        /// </summary>
        string PrimaryKey { get; }

        /// <summary>
        /// Creates or update an instance of this object in the database. All properties are written.
        /// </summary>
        /// <param name="userUuid">
        /// The uuid of the user that has called this method.
        /// </param>
        /// <param name="ignoreNull">
        /// If set to true will not submit explicitly set values.
        /// </param>
        /// <param name="transaction">
        /// The transaction object.
        /// </param>
        /// <returns>
        /// The <see cref="IEntityObject"/> that was saved.
        /// </returns>
        IEntityObject Save(Guid? userUuid = null, bool ignoreNull = false, object transaction = null);

        /// <summary>
        /// The deletes this object from the database.
        /// </summary>
        /// <param name="transaction">The transaction object.</param>
        void Delete(object transaction = null);
    }
}