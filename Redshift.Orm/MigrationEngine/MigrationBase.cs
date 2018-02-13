#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationBase.cs" company="RHEA System S.A.">
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
    using System.IO;

    /// <summary>
    /// The base class for all migrations that implements some common methods.
    /// </summary>
    public abstract class MigrationBase : IMigration
    {
        /// <summary>
        /// Gets the description.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Gets the full name of the migration. The full name of a migration is the <see cref="IMigration.Name"/> property
        /// prepended with a long timestamp. This is done for sorting purposes.
        /// </summary>
        public abstract string FullName { get; }

        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public abstract Guid Uuid { get; }

        /// <summary>
        /// Gets the name of the <see cref="IMigration"/>.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the version that this migration belongs to.
        /// </summary>
        public abstract Version Version { get; }

        /// <summary>
        /// Gets or sets the migration execution time in ms.
        /// </summary>
        public long MigrationExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the seed execution time in ms.
        /// </summary>
        public long SeedExecutionTime { get; set; }

        /// <summary>
        /// Deletes the record from the migration table.
        /// </summary>
        public virtual void Delete()
        {
            var migrationRecord = MigrationRecord.Find(this.Uuid);

            if (migrationRecord == null)
            {
                throw new InvalidDataException(string.Format("The migration {0} cannot be deleted from the migration table.", this.Name));
            }

            migrationRecord.Delete();
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public abstract void Migrate();

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        public abstract void Reverse();

        /// <summary>
        /// The method that executes if a migration fails.
        /// </summary>
        /// <remarks>To be used in case of migration failure. If the migration is fully transactioned, then this can be overriden and left blank. If it is not, then you can safely run this.Reverse().</remarks>
        public virtual void MigrationReset()
        {
            this.Reverse();
        }

        /// <summary>
        /// Saves the migration information to the migration table
        /// </summary>
        public virtual void Save()
        {
            var migration = new MigrationRecord(this);
            migration.Save();
        }

        /// <summary>
        /// The seeds the database if needed. This method can be left empty.
        /// </summary>
        public virtual void Seed()
        {
        }

        /// <summary>
        /// Checks whether the migration should execute.
        /// </summary>
        /// <returns>True if the migration should run.</returns>
        public virtual bool ShouldMigrate()
        {
            return MigrationRecord.Find(this.Uuid) == null;
        }
    }
}
