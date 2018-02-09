#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllTypeThing.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Tests.HelperModel
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Orm.Database;
    using Orm.EntityObject;

    internal class AllTypeThing : EntityObject<AllTypeThing>
    {
        public string SomeString { get; set; }

        public ConnectorType SomeEnum { get; set; }

        public DateTime SomeDate { get; set; }

        public Guid Id { get; set; }

        public long SomeLong { get; set; }

        public int SomeInt { get; set; }

        public double SomeDouble { get; set; }

        public float SomeFloat { get; set; }

        public bool SomeBool { get; set; }

        public List<string> SomeList { get; set; }

        public int[] SomeArray { get; set; }

        public List<Guid>SomeGuidList { get; set; }

        [IgnoreDataMember]
        public string SomeHidden { get; set; }

        public Int16 SomeInt16 { get; set; }

        public Decimal SomeDecimal { get; set; }

        public TimeSpan SomeTimeSpan { get; set; }

        public AllTypeThing()
        {
            this.SomeList = new List<string>() {"some1","some2"};
            this.SomeArray = new[] {2, 34, 65};
            this.SomeGuidList = new List<Guid>() {Guid.NewGuid(), Guid.NewGuid()};
            this.SomeString = "skejfks";
            this.SomeLong = 2;
            this.SomeInt = 23;
            this.SomeDouble = 6.34d;
            this.SomeFloat = 5438.764f;
            this.SomeBool = true;
            this.SomeHidden = "fdes";
            this.SomeInt16 = 2;
            this.SomeDecimal = 3.5m;
            this.SomeTimeSpan = new TimeSpan(2,4,3);
        }
    }
}
