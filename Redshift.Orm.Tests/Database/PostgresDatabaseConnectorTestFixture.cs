#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PostgresDatabaseConnectorTestFixture.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Tests.Database
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Threading.Tasks;
    using HelperModel;
    using Npgsql;

    using NUnit.Framework;

    using Redshift.Orm.Database;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// PostgresDatabaseConnectorTestFixture
    /// </summary>
    [TestFixture]
    public class PostgresDatabaseConnectorTestFixture : OrmBaseTestFixture
    {
        [Test]
        public void TestQueryDecomposition()
        {
            var conn = new NpgsqlConnection();
            var command = conn.CreateCommand();

            var sql = $"SELECT * FROM log";

            var connector = new PostgresDatabaseConnector();

            var thing = new Thing();

            var queries = new List<IWhereQueryContainer>();
            
            queries.Add(new WhereQueryContainer() { Comparer = "=", Property = thing.GetPropertyInfoFromName("Uuid"), Value = new List<object>() { Guid.NewGuid(), Guid.NewGuid() } });
            queries.Add(new WhereQueryContainer() { Comparer = "=", Property = thing.GetPropertyInfoFromName("ThingType"), Value = new List<object>() { Guid.NewGuid(), Guid.NewGuid() } });
            queries.Add(new PatternWhereQueryContainer() { Properties = new List<PropertyInfo>() { thing.GetPropertyInfoFromName("ThingType"), thing.GetPropertyInfoFromName("Uuid") }, Value = "dsf"});
            connector.DecomposeWhereStatements(ref conn, ref sql, queries, 10, 2, null, false);

            Console.WriteLine(sql);
        }

        [Test]
        public void VerifyThatTableNameOverridesWork()
        {
            var template = new NameOverridesPostgres();

            DatabaseSession.Instance.Connector.CreateTableWithColumns(template);

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(template));
            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckColumnExists(template.GetType().GetProperty("SomeString"), template));

            DatabaseSession.Instance.Connector.DeleteTable(template);
        }

        [Test]
        public void VerifyThatAConnectionCanBeOpened()
        {
            Assert.AreEqual(ConnectorType.Postgresql, DatabaseSession.Instance.Connector.ConnectorType);

            var con = DatabaseSession.Instance.CreateConnection() as NpgsqlConnection;

            Assert.NotNull(con);

            con.Close();
        }

        [Test]
        public void VerifyThatTableCanBeCreatedAndRemoved()
        {
            var allTypeThing = new AllTypeThing();

            DatabaseSession.Instance.Connector.CreateTable(allTypeThing);
            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(allTypeThing));
            Assert.Throws<InvalidDataException>(() => DatabaseSession.Instance.Connector.CreateTable(allTypeThing));

            DatabaseSession.Instance.Connector.DeleteTable(allTypeThing);
        }

        [Test]
        public void VerifyThatWhereReturns()
        {
            var allTypeThing = new AllTypeThing();

            DatabaseSession.Instance.Connector.CreateTableWithColumnsAndPrimaryKey(allTypeThing);

            var saveItem1 = new AllTypeThing
            {
                Uuid = Guid.NewGuid(),
                SomeString = string.Empty,
                SomeDate = DateTime.Now,
                SomeEnum = ConnectorType.Postgresql
            };

            var saveItem2 = new AllTypeThing
            {
                Uuid = Guid.NewGuid(),
                SomeString = string.Empty,
                SomeInt = 13
            };

            var saveItem3 = new AllTypeThing
            {
                Uuid = Guid.NewGuid(),
                SomeString = string.Empty,
                SomeInt = 13
            };


            saveItem1.Save();
            saveItem2.Save();
            saveItem3.Save();

            Assert.AreEqual(2, AllTypeThing.Where(saveItem3.GetType().GetProperty("SomeInt"), "=", 13).Count);
            Assert.AreEqual(1, AllTypeThing.Where(saveItem3.GetType().GetProperty("Uuid"), "=", saveItem3.Uuid).Count);

            DatabaseSession.Instance.Connector.DeleteTable(allTypeThing);
        }

        [Test]
        public void VerifyThatTableCanBeCreatedFullyAndRemoved()
        {
            var allTypeThing = new AllTypeThing();

            DatabaseSession.Instance.Connector.CreateTableWithColumnsAndPrimaryKey(allTypeThing);
            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(allTypeThing));


            Assert.Throws<InvalidDataException>(() => DatabaseSession.Instance.Connector.CreateTable(allTypeThing));

            DatabaseSession.Instance.Connector.DeleteTable(allTypeThing);
        }

        [Test]
        public void VerifyThatConnectorIsPostgres()
        {
            Assert.IsTrue(DatabaseSession.Instance.Connector.ConnectorType == ConnectorType.Postgresql);
        }

        [Test]
        public void VerifyThatColumnsOfAllTypesCanBeCreatedAndRemoved()
        {
            var template = new AllTypeThing();

            DatabaseSession.Instance.Connector.CreateTable(template);

            var propertyNotExists = template.GetType().GetProperties().First();
            Assert.Throws<InvalidDataException>(() => DatabaseSession.Instance.Connector.DeleteColumn(propertyNotExists, template));
            Assert.Throws<InvalidDataException>(() => DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(template));

            foreach (var property in template.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(IgnoreDataMemberAttribute))).ToList())
            {
                DatabaseSession.Instance.Connector.CreateColumn(property, template);
            }

            foreach (var property in template.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(IgnoreDataMemberAttribute))).ToList())
            {
                Assert.Throws<InvalidDataException>(() => DatabaseSession.Instance.Connector.CreateColumn(property, template));
            }

            foreach (var property in template.GetType().GetProperties().Where(p => !Attribute.IsDefined(p, typeof(IgnoreDataMemberAttribute))).ToList())
            {
                var transaction = DatabaseSession.Instance.CreateTransaction();
                DatabaseSession.Instance.Connector.DeleteColumn(property, template, transaction);
                DatabaseSession.Instance.CommitTransaction(transaction);
            }

            DatabaseSession.Instance.Connector.DeleteTable(new AllTypeThing());
        }

        [Test]
        public void VerifyThatConnectionThrowsOnBadCredentials()
        {
            Assert.Throws<System.Security.Authentication.AuthenticationException>(() => DatabaseSession.Instance.Connector.CreateConnection(new DatabaseCredentials() { Port = 121}));
            Assert.Throws<System.Security.Authentication.AuthenticationException>(() => DatabaseSession.Instance.Connector.CreateTransaction(new DatabaseCredentials() { Port = 24}));
        }

        [Test]
        public void VerifyThrowOnWrongTransaction()
        {
            var badtransaction = 4;

            IEntityObject obj = null;

            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.CreateTable(obj, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.DeleteTable(obj, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.CreateColumn(null, null, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.DeleteColumn(null, obj, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(null, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.DeletePrimaryKeyConstraint(null, null, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.CreateForeignKeyConstraint(null, obj, null, null, FkDeleteBehaviorKind.CASCADE, true, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.DeleteForeignKeyConstraint(null, obj, null, null, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.CreateRecord<AllTypeThing>(null, false, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.UpdateRecord<AllTypeThing>(null, false, badtransaction));
            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.DeleteRecord(null, badtransaction));

            Assert.Throws<ArgumentException>(() => DatabaseSession.Instance.CommitTransaction(badtransaction));
        }

        [Test]
        public void VerifyThatTableWithColumnsCanBeCreatedAndRemoved()
        {
            var template = new AllTypeThing();

            Assert.Throws<InvalidDataException>(() => AllTypeThing.All());

            DatabaseSession.Instance.Connector.CreateTableWithColumns(template);
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(template);

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(template));

            var saveItem1 = new AllTypeThing
            {
                Uuid = Guid.NewGuid(),
                SomeString = string.Empty,
                SomeDate = DateTime.Now,
                SomeEnum = ConnectorType.Postgresql
            };

            saveItem1.SomeGuidList.Add(Guid.NewGuid());
            saveItem1.SomeGuidList.Add(Guid.NewGuid());
            saveItem1.SomeGuidList.Add(Guid.NewGuid());

            saveItem1.SomeList.Add("Aleds");
            saveItem1.SomeList.Add("Aleds2");
            saveItem1.SomeList.Add("Aleds3");

            var saveItem2 = new AllTypeThing
            {
                Uuid = Guid.NewGuid(),
                SomeString = string.Empty
            };

            saveItem2.SomeGuidList.Add(Guid.NewGuid());
            saveItem2.SomeGuidList.Add(Guid.NewGuid());
            saveItem2.SomeList.Add("Aleds");
            saveItem2.SomeList.Add("Aleds2");
            saveItem2.SomeList.Add("Aleds3");

            saveItem1.Save();
            saveItem2.Save();

            Assert.AreEqual(2, DatabaseSession.Instance.Connector.ReadRecords<AllTypeThing>().ToList().Count);
            Assert.AreEqual(1, DatabaseSession.Instance.Connector.ReadRecords<AllTypeThing>(limit: 1).ToList().Count);
            Assert.AreEqual(1, DatabaseSession.Instance.Connector.ReadRecords<AllTypeThing>(limit: 20, offset: 1, orderBy: template.GetType().GetProperty(template.PrimaryKey)).ToList().Count);
            Assert.AreEqual(1, DatabaseSession.Instance.Connector.ReadRecords<AllTypeThing>(offset: 1).ToList().Count);

            var newString = "IsSome";

            saveItem1.SomeString = newString;

            saveItem1.Save(ignoreNull: true);

            var getSave1 = AllTypeThing.Find(saveItem1.Uuid);
            Assert.AreEqual(newString, getSave1.SomeString);

            saveItem1.Delete();
            saveItem2.Delete();

            saveItem1.Save();
            saveItem2.Save();

            var transaction = DatabaseSession.Instance.CreateTransaction();
            saveItem1.Save(transaction: transaction);
            saveItem2.Save(transaction: transaction);
            DatabaseSession.Instance.CommitTransaction(transaction);

            transaction = DatabaseSession.Instance.CreateTransaction();
            saveItem1.Delete(transaction);
            saveItem2.Delete(transaction);
            DatabaseSession.Instance.CommitTransaction(transaction);
            DatabaseSession.Instance.Connector.DeletePrimaryKeyConstraint(template.GetType().GetProperty(template.PrimaryKey), template);
            Assert.Throws<InvalidDataException>(
                () =>
                    DatabaseSession.Instance.Connector.DeletePrimaryKeyConstraint(
                        template.GetType().GetProperty(template.PrimaryKey), template));

            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(template);
            transaction = DatabaseSession.Instance.CreateTransaction();
            DatabaseSession.Instance.Connector.DeletePrimaryKeyConstraint(template.GetType().GetProperty(template.PrimaryKey), template, transaction);
            DatabaseSession.Instance.CommitTransaction(transaction);

            Assert.IsEmpty(DatabaseSession.Instance.Connector.ReadRecords<AllTypeThing>().ToList());

            DatabaseSession.Instance.Connector.DeleteTable(new AllTypeThing());
        }

        [Test]
        public void VerifyThatChecksWork()
        {
            var template = new AllTypeThing();

            DatabaseSession.Instance.Connector.CreateTableWithColumns(template);

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(template));
            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckColumnExists(template.GetType().GetProperty("SomeFloat"), template));
            Assert.IsFalse(DatabaseSession.Instance.Connector.CheckColumnExists(template.GetType().GetProperty("SomeHidden"), template));

            DatabaseSession.Instance.Connector.DeleteTable(new AllTypeThing());
        }

        [Test]
        public void VerifyThatOverridesAndForeignKeyConstraintsWork()
        {
            var fromObject = new User
            {
                Uuid = Guid.NewGuid(),
                Name = "John Doe"
            };

            var toObject = new Usergroup
            {
                Iid = 1,
                Name = "Admin"
            };

            fromObject.Usergroup_Id = toObject.Iid;

            DatabaseSession.Instance.Connector.CreateTableWithColumns(toObject);
            DatabaseSession.Instance.Connector.CreateTableWithColumns(fromObject);

            Assert.Throws<InvalidDataException>(() => DatabaseSession.Instance.Connector.CreateForeignKeyConstraint(fromObject.GetType().GetProperty("Usergroup_Id"), fromObject, toObject.GetType().GetProperty(toObject.PrimaryKey), toObject));

            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(toObject);
            Assert.Throws<InvalidDataException>(
                () =>
                    DatabaseSession.Instance.Connector.DeleteForeignKeyConstraint(
                        fromObject.GetType().GetProperty("Usergroup_Id"), fromObject,
                        toObject.GetType().GetProperty(toObject.PrimaryKey), toObject));

            Assert.DoesNotThrow(() => DatabaseSession.Instance.Connector.CreateForeignKeyConstraint(fromObject.GetType().GetProperty("Usergroup_Id"), fromObject, toObject.GetType().GetProperty(toObject.PrimaryKey), toObject));

            //Assert.Throws<InvalidDataException>(() => fromObject.Save());

            toObject.Save();

            Assert.DoesNotThrow(() => fromObject.Save());

            //Assert.Throws<InvalidDataException>(() => toObject.Delete());

            DatabaseSession.Instance.Connector.DeleteForeignKeyConstraint(fromObject.GetType().GetProperty("Usergroup_Id"), fromObject, toObject.GetType().GetProperty(toObject.PrimaryKey), toObject);

            Assert.DoesNotThrow(() => toObject.Delete());
            DatabaseSession.Instance.Connector.DeleteTable(new User());
            DatabaseSession.Instance.Connector.DeleteTable(new Usergroup());
        }

        [Test]
        public void VerifyThatUniquenessConstraintWorks()
        {
            var fromObject = new User
            {
                Uuid = Guid.NewGuid(),
                Name = "John Doe"
            };

            fromObject.Usergroup_Id = 3;

            DatabaseSession.Instance.Connector.CreateTableWithColumns(fromObject);

            Assert.Throws<InvalidDataException>(
                () =>
                    DatabaseSession.Instance.Connector.DeleteUniquenessConstraint(new[] { fromObject.GetType().GetProperty("Usergroup_Id") }, fromObject));

            Assert.Throws<ArgumentException>(
                () => DatabaseSession.Instance.Connector.CreateUniquenessConstraint(null, fromObject));

            Assert.Throws<ArgumentException>(
                () => DatabaseSession.Instance.Connector.DeleteUniquenessConstraint(null, fromObject));

            DatabaseSession.Instance.Connector.CreateUniquenessConstraint(new[] { fromObject.GetType().GetProperty("Usergroup_Id") }, fromObject);
            Assert.Throws<InvalidDataException>(
                () => DatabaseSession.Instance.Connector.CreateUniquenessConstraint(new[] { fromObject.GetType().GetProperty("Usergroup_Id") }, fromObject));

            Assert.DoesNotThrow(() => DatabaseSession.Instance.Connector.DeleteUniquenessConstraint(new[] { fromObject.GetType().GetProperty("Usergroup_Id") }, fromObject));

            Assert.DoesNotThrow(() => fromObject.Save());

            DatabaseSession.Instance.Connector.DeleteTable(new User());
        }

        [Test]
        public void VerifyThatDefaultWorks()
        {
            var fromObject = new User
            {
                Uuid = Guid.NewGuid(),
                Name = null
            };

            fromObject.Usergroup_Id = 3;

            var testname = "James";

            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.SetDefault(null, testname, fromObject));

            Assert.Throws<ArgumentNullException>(() => DatabaseSession.Instance.Connector.DeleteDefault(null, fromObject));

            DatabaseSession.Instance.Connector.CreateTableWithColumns(fromObject);

            DatabaseSession.Instance.Connector.SetDefault(fromObject.GetType().GetProperty("Name"), testname, fromObject);

            // save some names
            fromObject.Save(ignoreNull: true);

            var object2 = new User
            {
                Uuid = Guid.NewGuid(),
                Name = null
            };

            object2.Save(ignoreNull: true);

            Assert.AreEqual(2, User.All().Where(x => x.Name == testname).ToList().Count);

            DatabaseSession.Instance.Connector.DeleteDefault(fromObject.GetType().GetProperty("Name"), fromObject);

            var object3 = new User
            {
                Uuid = Guid.NewGuid(),
                Name = null
            };

            object3.Save(ignoreNull: true);

            Assert.AreEqual(2, User.All().Where(x => x.Name == testname).ToList().Count);
            Assert.AreEqual(3, User.All().ToList().Count);

            Assert.AreEqual(2, User.Subset(2, 1, fromObject.GetType().GetProperty("Name")).ToList().Count);

            DatabaseSession.Instance.Connector.DeleteTable(new User());
        }

        [Test]
        public void VerifyThatNotNullConstraintWorks()
        {
            var fromObject = new User
            {
                Uuid = Guid.NewGuid(),
                Name = "John Doe"
            };

            fromObject.Usergroup_Id = 3;

            DatabaseSession.Instance.Connector.CreateTableWithColumns(fromObject);
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(fromObject);

            Assert.Throws<ArgumentNullException>(
                () => DatabaseSession.Instance.Connector.CreateNotNullConstraint(null, fromObject));

            Assert.Throws<ArgumentNullException>(
                () => DatabaseSession.Instance.Connector.DeleteNotNullConstraint(null, fromObject));

            fromObject.Name = null;
            fromObject.Save();

            Assert.Throws<InvalidDataException>(
                () => DatabaseSession.Instance.Connector.CreateNotNullConstraint(fromObject.GetType().GetProperty("Name"), fromObject));

            fromObject.Name = "somename";
            fromObject.Save();

            DatabaseSession.Instance.Connector.CreateNotNullConstraint(fromObject.GetType().GetProperty("Name"), fromObject);

            fromObject.Name = null;

            Assert.Throws<InvalidDataException>(() => fromObject.Save());

            Assert.DoesNotThrow(() => DatabaseSession.Instance.Connector.DeleteNotNullConstraint(fromObject.GetType().GetProperty("Name"), fromObject));

            fromObject.Name = null;

            Assert.DoesNotThrow(() => fromObject.Save());

            DatabaseSession.Instance.Connector.DeleteTable(new User());
        }

        [Test]
        public void VerifyThatBadTypeThrows()
        {
            Assert.Throws<InvalidDataException>(() => DatabaseSession.Instance.Connector.CreateTableWithColumns(new BadTypes()));
            DatabaseSession.Instance.Connector.DeleteTable(new BadTypes());
        }

        [Test]
        public void VerifyThatTransactionSafeCreateTableWorks()
        {
            var fromObject = new User
            {
                Uuid = Guid.NewGuid(),
                Name = "John Doe"
            };

            var transaction = DatabaseSession.Instance.CreateTransaction();
            var prop = fromObject.GetType().GetProperties().First();
            DatabaseSession.Instance.Connector.CreateTable(fromObject, transaction);
            DatabaseSession.Instance.Connector.CreateColumn(prop, fromObject, transaction);
            DatabaseSession.Instance.CommitTransaction(transaction);

            DatabaseSession.Instance.Connector.DeleteTable(fromObject);

            var allTypes = new AllTypeThing();

            DatabaseSession.Instance.Connector.CreateTable(allTypes);

            transaction = DatabaseSession.Instance.CreateTransaction();
            DatabaseSession.Instance.Connector.CreateTable(fromObject, transaction);
            Assert.Throws<InvalidDataException>(() => allTypes.Save(transaction: transaction));
            DatabaseSession.Instance.CommitTransaction(transaction);

            // TODO: Unclear why but transactions stopped working all of a sudden
            DatabaseSession.Instance.Connector.DeleteTable(allTypes);
            DatabaseSession.Instance.Connector.DeleteTable(fromObject);

            Assert.IsFalse(DatabaseSession.Instance.Connector.CheckTableExists(fromObject));
        }

        [Test]
        public void VerifyThatTransactionSafeDeleteTableWorks()
        {
            var fromObject = new User
            {
                Uuid = Guid.NewGuid(),
                Name = "John Doe"
            };

            var toObject = new Usergroup();

            Assert.Throws<InvalidDataException>(() => DatabaseSession.Instance.Connector.DeleteTable(fromObject));

            DatabaseSession.Instance.Connector.CreateTableWithColumns(fromObject);
            DatabaseSession.Instance.Connector.CreateTableWithColumns(toObject);
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(fromObject);
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(toObject);

            var transaction = DatabaseSession.Instance.CreateTransaction();

            DatabaseSession.Instance.Connector.CreateForeignKeyConstraint(fromObject.GetType().GetProperty("Usergroup_Id"), fromObject, toObject.GetType().GetProperty(toObject.PrimaryKey), toObject, FkDeleteBehaviorKind.CASCADE, true, transaction);
            DatabaseSession.Instance.Connector.DeleteForeignKeyConstraint(fromObject.GetType().GetProperty("Usergroup_Id"), fromObject, toObject.GetType().GetProperty(toObject.PrimaryKey), toObject, transaction);
            DatabaseSession.Instance.Connector.DeleteTable(fromObject, transaction);

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(fromObject));

            DatabaseSession.Instance.Connector.DeleteTable(toObject, transaction);

            Assert.IsTrue(DatabaseSession.Instance.Connector.CheckTableExists(toObject));

            DatabaseSession.Instance.CommitTransaction(transaction);

            Assert.IsFalse(DatabaseSession.Instance.Connector.CheckTableExists(fromObject));
            Assert.IsFalse(DatabaseSession.Instance.Connector.CheckTableExists(toObject));
        }

        [Test]
        public void VerifyVariousTransactionColumnManipulations()
        {
            var fromObject = new User
            {
                Uuid = Guid.NewGuid(),
                Name = "John Doe"
            };

            DatabaseSession.Instance.Connector.CreateTableWithColumns(fromObject);

            var transaction = DatabaseSession.Instance.CreateTransaction();
            DatabaseSession.Instance.Connector.CreatePrimaryKeyConstraint(fromObject, transaction);
            DatabaseSession.Instance.CommitTransaction(transaction);

            DatabaseSession.Instance.Connector.DeleteTable(fromObject);
        }
    }
}