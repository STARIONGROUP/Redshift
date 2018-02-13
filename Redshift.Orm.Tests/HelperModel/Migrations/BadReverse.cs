#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BadReverse.cs" company="RHEA System S.A.">
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

namespace Redshift.Orm.Tests.HelperModel.Migrations
{
    using System;
    using Orm.Database;
    using Redshift.Orm.Tests.HelperModel;

    public class BadReverse : MigrationBase
    {
        public override string Description
        {
            get { return "A bad save script."; }
        }

        public override Version Version
        {
            get
            {
                return new Version(1,0,0);
            }
        }

        public override string FullName
        {
            get
            {
                return string.Format("{0}_{1}", "20150902184304", this.Name);
            }
        }

        public override Guid Uuid
        {
            get { return Guid.Parse("A0B4F191-C90A-4703-A274-8085EBCE64C6"); }
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
            throw new Exception("Some bad reverse.");
        }
    }
}
