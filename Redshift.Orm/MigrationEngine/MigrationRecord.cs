#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationRecord.cs" company="RHEA System S.A.">
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
    using EntityObject;

    /// <summary>
    /// The purpose of the migration class is to aid in the creation of the migrations table in the
    /// database. NO OTHER USE!
    /// </summary>
    public class MigrationRecord : EntityObject<MigrationRecord>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationRecord"/> class.
        /// </summary>
        public MigrationRecord()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationRecord"/> class.
        /// </summary>
        /// <param name="migration">
        /// The migration from which to fill this object.
        /// </param>
        public MigrationRecord(IMigration migration)
        {
            this.Uuid = migration.Uuid;
            this.Name = migration.Name;
            this.FullName = migration.FullName;
            this.Description = migration.Description;
            this.Version = migration.Version.ToString();
            this.AppliedOn = DateTime.UtcNow;
            this.MigrationExecutionTime = migration.MigrationExecutionTime;
            this.SeedExecutionTime = migration.SeedExecutionTime;
        }

        /// <summary>
        /// Gets or sets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public Guid Uuid { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="IMigration"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full name of the migration. The full name of a migration is the <see cref="Name"/> property
        /// prepended with a long timestamp. This is done for sorting purposes.
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the version that the migration belongs to.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the datetime that the Migration was applied on.
        /// </summary>
        public DateTime AppliedOn { get; set; }

        /// <summary>
        /// Gets or sets the migration execution time in ms.
        /// </summary>
        public long MigrationExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the seed execution time in ms.
        /// </summary>
        public long SeedExecutionTime { get; set; }
    }
}
