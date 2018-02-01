#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EntityColumnNameOverrideAttributeTestFixture.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Tests.Attributes
{
    using System.Linq;
    using System.Reflection;
    using NUnit.Compatibility;
    using NUnit.Framework;
    using Redshift.Orm.Attributes;

    [TestFixture]
    public class EntityColumnNameOverrideAttributeTestFixture
    {
        [Test]
        public void VerifyThatSettingEntityColumnNameOverrideAttributeWorks()
        {
            var type = typeof(EntityColumnNameOverrideAttributeTestClass);
            var property = type.GetTypeInfo().GetProperty("Property1");
            var property2 = type.GetTypeInfo().GetProperty("Property2");

            Assert.IsTrue(property.IsDefined(typeof(EntityColumnNameOverrideAttribute)));
            Assert.IsFalse(property2.IsDefined(typeof(EntityColumnNameOverrideAttribute)));

            Assert.AreEqual("`nana`", ((EntityColumnNameOverrideAttribute)property.GetCustomAttributes(typeof(EntityColumnNameOverrideAttribute)).Single()).Name);
            Assert.AreEqual("nana", ((EntityColumnNameOverrideAttribute)property.GetCustomAttributes(typeof(EntityColumnNameOverrideAttribute)).Single()).CleanName);
        }
    }

    internal class EntityColumnNameOverrideAttributeTestClass
    {
        [EntityColumnNameOverride("`nana`")]
        public int Property1 { get; set; }

        public int Property2 { get; set; }
    }
}
