#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationEngineTestFixture.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using HelperModel.Migrations;
    using NUnit.Framework;
    using Redshift.Orm.Database;

    [TestFixture]
    public class MigrationEngineTestFixture :OrmBaseTestFixture
    {
        [Test]
        public void VerifyThatMigrationEngineWorks()
        {
            // Reset all migrations
            MigrationEngine.Reset();

            Assert.IsFalse(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
            Assert.Throws<Exception>(() => MigrationEngine.Migrate());
            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
            Assert.Throws<Exception>(() => MigrationEngine.Migrate(new List<string>() {"BadMigration"}));
            Assert.Throws<Exception>(() => MigrationEngine.Migrate(excludes: new List<string>() { "BadMigration", "BadSeed" }));
            MigrationEngine.Migrate(excludes: new List<string>() { "BadMigration", "BadSeed", "BadSave", "CreateMigrationTable" });
            Assert.Throws<Exception>(() => MigrationEngine.Reset("BadReverse"));
            MigrationEngine.Reset(hard: true);
            Assert.IsFalse(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
        }

        [Test]
        public void VerifyThatMigrationVersionsWork()
        {
            MigrationEngine.Reset();

            MigrationEngine.Migrate(excludes: new List<string>() { "BadMigration", "BadSeed", "BadSave", "BadReverse" });

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
            Assert.AreEqual(4, MigrationRecord.All().Count);

            MigrationEngine.ResetTo(new Version(1,0,0));

            Assert.AreEqual(3, MigrationRecord.All().Count);

            MigrationEngine.Reset();

            MigrationEngine.Migrate(excludes: new List<string>() { "BadMigration", "BadSeed", "BadSave" });

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
            Assert.AreEqual(5, MigrationRecord.All().Count);

            var migrationDeleteBase = new BadMigration();

            Assert.Throws<InvalidDataException>(() => migrationDeleteBase.Delete());

            Assert.Throws<Exception>(() => MigrationEngine.ResetTo(new Version(0, 0, 0)));

            MigrationEngine.Reset(hard: true);
            Assert.IsFalse(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
        }

        [Test]
        public void VerifyThatMigrationVersionsWorkSoftReset()
        {
            MigrationEngine.Reset();

            MigrationEngine.Migrate(excludes: new List<string>() { "BadMigration", "BadSeed", "BadSave", "BadReverse" });

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
            Assert.AreEqual(4, MigrationRecord.All().Count);

            MigrationEngine.ResetTo(new Version(1, 0, 0), true);

            Assert.AreEqual(3, MigrationRecord.All().Count);

            MigrationEngine.Reset();

            MigrationEngine.Migrate(excludes: new List<string>() { "BadMigration", "BadSeed", "BadSave" });

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
            Assert.AreEqual(5, MigrationRecord.All().Count);

            var migrationDeleteBase = new BadMigration();

            Assert.Throws<InvalidDataException>(() => migrationDeleteBase.Delete());

            Assert.DoesNotThrow(() => MigrationEngine.ResetTo(new Version(0, 0, 0),true));

            MigrationEngine.Reset(hard: true);
            Assert.IsFalse(DatabaseSession.Instance.Connector.CheckTableExists(new MigrationRecord()));
        }
    }
}
