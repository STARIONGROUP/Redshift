#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityObjectTestFixture.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Tests
{
    using System;
    using System.Collections.Generic;
    using HelperModel;
    using NUnit.Framework;
    using Orm.Database;

    [TestFixture]
    public class EntityObjectTestFixture : OrmBaseTestFixture
    {
        [Test]
        public void VerifyCountWorks()
        {
            var temp = User.Template();

            DatabaseSession.Instance.Connector.CreateTableWithColumnsAndPrimaryKey(temp);

            Assert.AreEqual(0, User.Count());

            temp.Uuid = Guid.NewGuid();
            temp.Name = "dksd";
            temp.Usergroup_Id = 4;

            temp.Save();

            Assert.AreEqual(1, User.Count());

            var temp2 = new User
            {
                Name = "eee",
                Usergroup_Id = 4,
                Uuid = Guid.NewGuid()
            };

            temp2.Save();

            Assert.AreEqual(2, User.Count());

            Assert.AreEqual(1, User.CountWhere(new List<IWhereQueryContainer> {new WhereQueryContainer() {Comparer = "=", Property = typeof(User).GetProperty("Name"), Value = new List<object> {temp.Name}} }));
        }

        [Test]
        public void VerifyWhereWorks()
        {
            var temp = User.Template();

            DatabaseSession.Instance.Connector.CreateTableWithColumnsAndPrimaryKey(temp);

            Assert.AreEqual(0, User.Count());

            temp.Uuid = Guid.NewGuid();
            temp.Name = "dksd";
            temp.Usergroup_Id = 4;

            temp.Save();

            var temp2 = new User
            {
                Name = "eee",
                Usergroup_Id = 4,
                Uuid = Guid.NewGuid()
            };

            temp2.Save();

            var ret = User.Where(new List<IWhereQueryContainer>
            {
                new WhereQueryContainer()
                {
                    Comparer = "=",
                    Property = typeof(User).GetProperty("Name"),
                    Value = new List<object> {temp.Name}
                }
            }, limit: 5, offset: 0);

            Assert.AreEqual(1, ret.Count);

            Assert.AreEqual(temp.Name, ret[0].Name);

            var temp3 = new User
            {
                Name = temp.Name,
                Usergroup_Id = 4,
                Uuid = Guid.NewGuid()
            };

            temp3.Save();

            ret = User.Where(new List<IWhereQueryContainer>
            {
                new WhereQueryContainer()
                {
                    Comparer = "=",
                    Property = typeof(User).GetProperty("Name"),
                    Value = new List<object> {temp.Name}
                }
            }, limit: 5, offset: 0);

            Assert.AreEqual(2, ret.Count);

            Assert.AreEqual(temp.Name, ret[1].Name);
        }
    }
}
