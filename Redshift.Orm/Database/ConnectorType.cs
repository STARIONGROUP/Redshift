#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectorType.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Orm.
//
//    Redshift.Orm is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Orm is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Orm.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Orm.Database
{
    /// <summary>
    /// The supported connector types.
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>
        /// The connector that allows connections to a PosgreSQL database
        /// </summary>
        Postgresql,

        /// <summary>
        /// The connector provides a an in memory cache of objects.
        /// </summary>
        InMemory
    }
}