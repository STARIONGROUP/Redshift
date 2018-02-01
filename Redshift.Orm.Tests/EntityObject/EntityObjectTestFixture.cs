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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Redshift.Orm.Tests.EntityObject
{
    using System.Reflection;

    using NUnit.Framework;

    using Redshift.Orm.Attributes;
    using Redshift.Orm.Database;
    using Redshift.Orm.EntityObject;

    public abstract class Thing<T> : EntityObject<T> where T: IEntityObject
    {
        public Guid Uuid { get; set; }
    }

    public class User : Thing<User>
    {
        public User(Guid iid)
        {
            this.Uuid = iid;
        }

        public string Username { get; set; }

        //[ApiSerializeNull]
        public string Password { get; set; }
    }

    public class AddressRegion : Thing<AddressRegion>
    {
        public AddressRegion(Guid iid)
        {
            this.Uuid = iid;
        }

        public string CodeStatus { get; set; }
    }

    //[ApiIgnoreMethod(RestMethods.POST | RestMethods.PATCH | RestMethods.DELETE, "Log entities cannot be created, modified or deleted.")]
    public class Log : Thing<Log>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="iid">
        /// The unique identifier.
        /// </param>
        public Log(Guid iid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        public Log()
        {
        }


        ///<summary>
        /// The thing that was created, modified or deleted.
        ///</summary>
        //[ApiDescription("The thing that was created, modified or deleted.")]
        public Guid Thing { get; set; }
    }

    [TestFixture]
    public class EntityObjectTestFixture : InMemoryDatabaseSessionBaseTestFixture
    {
        [Test]
        public void VerifyThatSubsetReturns()
        {
            Assert.Inconclusive();
            // Assert.IsNotEmpty(User.Subset(1, 0, typeof(User).GetProperty("Username"), true));
            // Assert.AreEqual(5, AddressRegion.Subset(5, 0, typeof(AddressRegion).GetProperty("Uuid"), true).Count);
        }

        [Test]
        public void VerifyThatSaveWorks()
        {
            Assert.Inconclusive();
            var all = AddressRegion.All();

            // existing record
            //var testConcept = all.First();
            //var count = all.Count;

            //testConcept.CodeStatus = "someother";

            //Assert.DoesNotThrow(() => testConcept.Save());
            //Assert.AreEqual("someother", AddressRegion.Find(testConcept.Uuid).CodeStatus);

            // new record
            //var newConcept = new AddressRegion(Guid.NewGuid()) { CodeStatus = "blabla" };
            //Assert.DoesNotThrow(() => newConcept.Save());
            //Assert.AreEqual("blabla", AddressRegion.Find(newConcept.Uuid).CodeStatus);
            //Assert.AreEqual(count + 1, AddressRegion.All().Count);

            // new record
            //var mignewConcept = new MigrationRecord();
            //Assert.DoesNotThrow(() => mignewConcept.Save());
            //Assert.AreEqual(1, MigrationRecord.All().Count);

            //mignewConcept.FullName = "somename";
            //Assert.DoesNotThrow(() => mignewConcept.Save());
        }

        [Test]
        public void VerifyThatDeleteWorks()
        {
            Assert.Inconclusive();
            //var all = AddressRegion.All();

            //var count = all.Count;

            //// new record

            //var newConcept = new AddressRegion(Guid.NewGuid()) { CodeStatus = "someother" };
            //Assert.DoesNotThrow(() => newConcept.Save());
            //Assert.AreEqual(count + 1, AddressRegion.All().Count);
            //Assert.DoesNotThrow(() => newConcept.Delete());
            //Assert.AreEqual(count, AddressRegion.All().Count);
        }
    }
}
