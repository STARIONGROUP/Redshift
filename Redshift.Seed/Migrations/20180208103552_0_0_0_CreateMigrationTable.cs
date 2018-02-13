#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="20180208103552_0_0_0_CreateMigrationTable.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Seed.
//
//    Redshift.Seed is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Seed is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Seed.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Seed.Migrations
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;

    using Redshift.Orm.Database;

    /// <summary>
    /// Creates migration table.
    /// </summary>
    internal class CreateMigrationTable : MigrationBase
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public override Guid Uuid 
        {
            get 
            { 
                return Guid.Parse("8a5ec392-3712-4715-9663-85e050f983cc"); 
            }
        }

        /// <summary>
        /// Gets the name of the <see cref="IMigration"/>.
        /// </summary>
        public override string Name
        {
            get 
            { 
                return this.GetType().Name; 
            }
        }

        /// <summary>
        /// Gets the full name of the migration. The full name of a migration is the <see cref="IMigration.Name"/> property
        /// prepended with a long date. This is done for sorting purposes.
        /// </summary>
        public override string FullName
        {
            get
            {
                return string.Format("{0}_{1}", "20180208103552", this.Name);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description 
        {
            get 
            { 
                return "Creates the Migration table so that the application can write migration patch logs."; 
            }
        }

        /// <summary>
        /// Gets the version this migration belongs to
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version(0, 0, 0);
            }
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public override void Migrate()
        {
            var migrationTableTemplate = new MigrationRecord();
            DatabaseSession.Instance.Connector.CreateTable(migrationTableTemplate);

            foreach (var property in migrationTableTemplate.GetType().GetProperties().Where(p => !CustomAttributeExtensions.IsDefined(p, typeof(IgnoreDataMemberAttribute))).ToList())
            {
                DatabaseSession.Instance.Connector.CreateColumn(property, migrationTableTemplate);
            }

            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(migrationTableTemplate);
        }

        /// <summary>
        /// The method that executes if a migration fails.
        /// </summary>
        /// <remarks>To be used in case of migration failure. If the migration is fully transactioned, then this can be overriden and left blank. If it is not, then you can safely run this.Reverse().</remarks>
        public override void MigrationReset()
        {

        }

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        public override void Reverse()
        {
            var migrationTableTemplate = new MigrationRecord();
            DatabaseSession.Instance.Connector.DeleteTable(migrationTableTemplate);
        }

        /// <summary>
        /// Deletes the record from the migration table.
        /// </summary>
        public override void Delete()
        {
            // nothing needed
        }

        /// <summary>
        /// Checks whether the migration should execute.
        /// </summary>
        /// <returns>True if the migration should run.</returns>
        public override bool ShouldMigrate()
        {
            var migrationTableTemplate = new MigrationRecord();
            return !DatabaseSession.Instance.Connector.CheckTableExists(migrationTableTemplate);
        }
    }
}
