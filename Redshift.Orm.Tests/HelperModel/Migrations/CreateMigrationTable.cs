#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CreateMigrationTable.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.Tests.
//
//    Redshift.Orm.Tests is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm.Tests is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Orm.Tests.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace DotNORM.HelperModel.Migrations
{
    using System;
    using System.Linq;
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
            get { return Guid.Parse("B0C4E191-C90A-4703-B274-8085EBCE64C6"); }
        }

        /// <summary>
        /// Gets the name of the <see cref="IMigration"/>.
        /// </summary>
        public override string Name
        {
            get { return this.GetType().Name; }
        }

        /// <summary>
        /// Gets the full name of the migration. The full name of a migration is the <see cref="IMigration.Name"/> property
        /// prepended with a long date. This is done for sorting purposes.
        /// </summary>
        public override string FullName
        {
            get
            {
                return string.Format("{0}_{1}", "201508171001", this.Name);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description 
        {
            get { return "Creates the Migration table so that the application can write migration patch logs."; }
        }

        /// <summary>
        /// Gets the version this migration belongs to
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version(0,0,0);
            }
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public override void Migrate()
        {
            var migrationTableTemplate = new MigrationRecord();
            DatabaseSession.Instance.Connector.CreateTable(migrationTableTemplate);

            foreach (var property in migrationTableTemplate.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(IgnoreDataMemberAttribute))).ToList())
            {
                DatabaseSession.Instance.Connector.CreateColumn(property, migrationTableTemplate);
            }

            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(migrationTableTemplate);
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