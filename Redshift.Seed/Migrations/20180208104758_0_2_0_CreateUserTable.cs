#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="20180208104758_0_2_0_CreateUserTable.cs" company="RHEA System S.A.">
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

namespace Redshift.Seed.Migrations
{
    using System;
    using Model;
    using Redshift.Orm.Database;

    /// <summary>
    /// The purpose of the <see cref="CreateUserTable"/> migration is to create user table
    /// </summary>
    internal class CreateUserTable : MigrationBase
    {
        /// <summary>
        /// Gets the unique <see cref="Guid"/> of the <see cref="IMigration"/>. This must be generated pre-compile time.
        /// </summary>
        public override Guid Uuid 
        {
            get 
            { 
                return Guid.Parse("81710ff3-a03a-462e-bdb3-4103d110426a"); 
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
                return string.Format("{0}_{1}", "20180208104758", this.Name);
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description
        {
            get 
            { 
                return "Creates a user table"; 
            }
        }

        /// <summary>
        /// Gets the version this migration belongs to
        /// </summary>
        public override Version Version
        {
            get
            {
                return new Version(0, 2, 0);
            }
        }

        /// <summary>
        /// The method that executes to apply a migration.
        /// </summary>
        public override void Migrate()
        {
            var template = User.Template();

            var transaction = DatabaseSession.Instance.CreateTransaction();

            DatabaseSession.Instance.Connector.CreateTable(template, transaction);

            DatabaseSession.Instance.Connector.CreateColumn(typeof(User).GetProperty("Uuid"), template, transaction);
            DatabaseSession.Instance.Connector.CreateColumn(typeof(User).GetProperty("ModifiedOn"), template, transaction);
            DatabaseSession.Instance.Connector.CreateColumn(typeof(User).GetProperty("CreatedOn"), template, transaction);
            DatabaseSession.Instance.Connector.CreateColumn(typeof(User).GetProperty("Username"), template, transaction);
            DatabaseSession.Instance.Connector.CreateColumn(typeof(User).GetProperty("Email"), template, transaction);
            DatabaseSession.Instance.Connector.CreateColumn(typeof(User).GetProperty("Password"), template, transaction);
            DatabaseSession.Instance.Connector.CreateColumn(typeof(User).GetProperty("Salt"), template, transaction);
            DatabaseSession.Instance.Connector.CreateColumn(typeof(User).GetProperty("Usergroup"), template, transaction);

            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(template, transaction);

            // create foreign key contraint to usergroup
            var usergroupTemplate = Usergroup.Template();
            DatabaseSession.Instance.Connector.CreateForeignKeyConstraint(typeof(User).GetProperty("Usergroup"), template, typeof(Usergroup).GetProperty("Uuid"), usergroupTemplate, FkDeleteBehaviorKind.RESTRICT, transaction: transaction);

            DatabaseSession.Instance.CommitTransaction(transaction);
        }

        /// <summary>
        /// The method that executes if a migration fails.
        /// </summary>
        /// <remarks>To be used in case of migration failure. If the migration is fully transactioned, then this can be overriden and left blank. If it is not, then you can safely run this.Reverse().</remarks>
        public override void MigrationReset()
        {
            // what to do in case of migration fail
        }

        /// <summary>
        /// The method that executes if a migration needs to be rolled back.
        /// </summary>
        public override void Reverse()
        {
            var transaction = DatabaseSession.Instance.CreateTransaction();

            var UsergroupReferencePropertyInfo = typeof(User).GetProperty("Usergroup");
            var usergroupIdPropertyInfo = typeof(Usergroup).GetProperty("Uuid");
            DatabaseSession.Instance.Connector.DeleteForeignKeyConstraint(UsergroupReferencePropertyInfo, User.Template(), usergroupIdPropertyInfo, Usergroup.Template(), transaction: transaction);
            DatabaseSession.Instance.CommitTransaction(transaction);
        }

        /// <summary>
        /// The seeds the database if needed. This method can be left empty.
        /// </summary>
        public override void Seed()
        {
            // any required seeding
#if DEBUG

#endif
        }

        /// <summary>
        /// Checks whether the migration should execute.
        /// </summary>
        /// <returns>True if the migration should run.</returns>
        public override bool ShouldMigrate()
        {
            // migration condition
            return base.ShouldMigrate();
        }
    }
}
