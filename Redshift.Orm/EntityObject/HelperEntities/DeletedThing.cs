#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeletedThing.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.EntityObject
{
    using System;

    /// <summary>
    /// A helper entity to assist on working with the table of deleted things.
    /// </summary>
    public class DeletedThing : EntityObject<DeletedThing>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeletedThing"/> class.
        /// </summary>
        public DeletedThing()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeletedThing"/> class.
        /// </summary>
        /// <param name="uuid">
        /// The uuid.
        /// </param>
        public DeletedThing(Guid uuid)
        {
            this.Uuid = uuid;
        }

        /// <summary>
        /// Gets or sets the unique identifier for this persisted thing.
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or sets the date this object was last modified
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Creates or update an instance of this object in the database. All properties are written.
        /// </summary>
        /// <param name="userUuid">
        /// The uuid of the user that performed the save.
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
        public override IEntityObject Save(Guid? userUuid = null, bool ignoreNull = false, object transaction = null)
        {
            throw new InvalidOperationException("Deleted things cannot be saved.");
        }

        /// <summary>
        /// The deletes this object from the database.
        /// </summary>
        /// <param name="transaction">The transaction object.</param>
        public override void Delete(object transaction = null)
        {
            throw new InvalidOperationException("Deleted things cannot be deleted.");
        }
    }
}
