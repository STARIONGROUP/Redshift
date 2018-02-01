#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMigration.cs" company="RHEA System S.A.">
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
    using System;

    /// <summary>
    /// The MigrationRecord interface.
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        Guid Uuid { get; }

        /// <summary>
        /// Gets the version that this migration belongs to.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Gets or sets the migration execution time in ms.
        /// </summary>
        long MigrationExecutionTime { get; set; }

        /// <summary>
        /// Gets or sets the seed execution time in ms.
        /// </summary>
        long SeedExecutionTime { get; set; }

        /// <summary>
        /// Gets the name of the <see cref="IMigration"/>.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the full name of the migration. The full name of a migration is the <see cref="Name"/> property
        /// prepended with a long timestamp. This is done for sorting purposes.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        void Migrate();

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        void Reverse();

        /// <summary>
        /// The method that executes if a migration fails.
        /// </summary>
        /// <remarks>To be used in case of migration failure. If the migration is fully transactioned, then this can be overriden and left blank. If it is not, then you can safely run this.Reverse().</remarks>
        void MigrationReset();

        /// <summary>
        /// Checks whether the migration should execute.
        /// </summary>
        /// <returns>True if the migration should run.</returns>
        bool ShouldMigrate();

        /// <summary>
        /// Saves the migration information to the migration table
        /// </summary>
        void Save();

        /// <summary>
        /// Deletes the record from the migration table.
        /// </summary>
        void Delete();

        /// <summary>
        /// The seeds the database if needed. This method can be left empty.
        /// </summary>
        void Seed();
    }
}
