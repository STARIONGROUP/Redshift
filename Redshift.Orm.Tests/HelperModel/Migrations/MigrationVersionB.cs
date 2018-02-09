#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MigrationVersionB.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Tests.HelperModel.Migrations
{
    using System;
    using Orm.Database;
    using Redshift.Orm.Tests.HelperModel;

    public class MigrationVersionB : MigrationBase
    {
        public override string Description
        {
            get { return "A migration in version 1 second go"; }
        }

        public override Version Version
        {
            get { return new Version(1, 0, 0); }
        }

        public override string FullName
        {
            get
            {
                return string.Format("{0}_{1}", "201509281002", this.Name);
            }
        }

        public override Guid Uuid
        {
            get { return Guid.Parse("A2C5F191-C90A-4703-B274-8085EBCE65C7"); }
        }

        public override string Name
        {
            get { return this.GetType().Name; }
        }

        public override void Migrate()
        {
            // do nothing
        }

        public override void Reverse()
        {

        }
    }
}
