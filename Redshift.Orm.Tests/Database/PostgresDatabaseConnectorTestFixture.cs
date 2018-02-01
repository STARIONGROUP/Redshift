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
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    using Npgsql;

    using NUnit.Framework;

    using Redshift.Orm.Database;
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// PostgresDatabaseConnectorTestFixture
    /// </summary>
    [TestFixture]
    public class PostgresDatabaseConnectorTestFixture
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
            queries.Add(new PatternWhereQueryContainer() { Property = new List<PropertyInfo>() { thing.GetPropertyInfoFromName("ThingType"), thing.GetPropertyInfoFromName("Uuid") }, Value = "dsf"});
            connector.DecomposeWhereStatements(ref conn, ref sql, queries, 10, 2, null, false);

            Console.WriteLine(sql);
        }
    }
}