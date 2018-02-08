#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Thing.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Seed.
//
//    Redshift.Seed is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Seed is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Seed.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Seed.Model
{
    using System;
    using Orm.EntityObject;

    public abstract class Thing<T> : EntityObject<T>, ISeedObject where T : IEntityObject
    {
        /// <summary>
        /// Gets or sets the unique identifier
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or sets the date this object was modified
        /// </summary>
        public DateTime ModifiedOn { get; set; }

        /// <summary>
        /// Gets or sets the date this object was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
