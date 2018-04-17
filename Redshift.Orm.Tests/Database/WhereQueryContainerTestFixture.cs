#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WhereQueryContainerTestFixture.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.Tests.
//
//    Redshift.Orm.Tests is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm.Tests is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
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
    using HelperModel;
    using NUnit.Framework;

    using Redshift.Orm.Database;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// WhereQueryContainerTestFixture
    /// </summary>
    [TestFixture]
    public class WhereQueryContainerTestFixture : OrmBaseTestFixture
    {
        [Test]
        public void VerifyThatContainerConstructsString()
        {
            var container = new WhereQueryContainer()
                                {
                                    Comparer = "=",
                                    Property = typeof(Thing).GetProperty("ThingType"),
                                    Value = new List<object>() { "someVal12", "someval3", 4 , DateTime.UtcNow }
                                };

            var container2 = new WhereQueryContainer()
            {
                Comparer = "=",
                Property = typeof(Thing).GetProperty("ThingType"),
                Value = new List<object>() { DateTime.UtcNow }
            };

            Console.WriteLine(container.GetSqlString());
        }

        [Test]
        public void VerifyThatQueryContainersCanBeJoinedByORAND()
        {
            var template = new User();

            DatabaseSession.Instance.Connector.CreateTableWithColumns(template);

            var user1 = new User()
            {
                Uuid = Guid.NewGuid(),
                Usergroup_Id = 4,
                Name = "Name1"
            };

            user1.Save();

            var user2 = new User()
            {
                Uuid = Guid.NewGuid(),
                Usergroup_Id = 5,
                Name = "Name2"
            };

            user2.Save();

            var whereQuery1 = new WhereQueryContainer()
            {
                Comparer = "=",
                Property = typeof(User).GetProperty("Name"),
                Value = new List<object>() { "Name1" }
            };

            var whereQuery2 = new WhereQueryContainer()
            {
                Comparer = "=",
                Property = typeof(User).GetProperty("Usergroup_Id"),
                Value = new List<object>() { 5 }
            };

            Assert.AreEqual(0, User.Where(new List<IWhereQueryContainer>() { whereQuery1, whereQuery2}, true).Count);
            Assert.AreEqual(2, User.Where(new List<IWhereQueryContainer>() { whereQuery1, whereQuery2 }, false).Count);

            Assert.AreEqual(0, User.CountWhere(new List<IWhereQueryContainer>() { whereQuery1, whereQuery2 }, true));
            Assert.AreEqual(2, User.CountWhere(new List<IWhereQueryContainer>() { whereQuery1, whereQuery2 }, false));
        }

        [Test]
        public void VerifyThatQueriesAreCorrect()
        {
            var template = new DatedThing();

            DatabaseSession.Instance.Connector.CreateTableWithColumns(template);

            var object1 = new DatedThing()
            {
                Uuid = Guid.NewGuid(),
                Date = new DateTime(2018, 2, 3)
            };

            object1.Save();

            var object2 = new DatedThing()
            {
                Uuid = Guid.NewGuid(),
                Date = new DateTime(2018, 2, 4)
            };

            object2.Save();

            var object3 = new DatedThing()
            {
                Uuid = Guid.NewGuid(),
                Date = new DateTime(2018, 2, 5)
            };

            object3.Save();

            var returned = DatedThing.Where(new List<IWhereQueryContainer>()
            {
                new WhereQueryContainer
                {
                    Comparer = ">=",
                    Property = typeof(DatedThing).GetProperty("Date"),
                    Value = { new DateTime(2018, 2, 3) }
                },
                new WhereQueryContainer
                {
                    Comparer = "<",
                    Property = typeof(DatedThing).GetProperty("Date"),
                    Value = { new DateTime(2018, 2, 5) }
                }
            });

            Assert.AreEqual(2, returned.Count);
        }
    }
}