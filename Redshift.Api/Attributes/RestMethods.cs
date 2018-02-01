#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RestMethods.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Api.
//
//    Redshift.Api is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Api is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Api.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Api.Attributes
{
    using System;

    /// <summary>
    /// Available REST methods.
    /// </summary>
    [Flags]
    public enum RestMethods
    {
        /// <summary>
        /// The GET, for reading values.
        /// </summary>
        NONE = 0x0,

        /// <summary>
        /// The POST is used for creating concepts.
        /// </summary>
        POST = 0x1,

        /// <summary>
        /// The DELETE is used to delete concepts.
        /// </summary>
        DELETE = 0x2,

        /// <summary>
        /// The PATCH is used to delta updates to concepts.
        /// </summary>
        PATCH = 0x4,

        /// <summary>
        /// The PUT is used to update whole concepts.
        /// </summary>
        PUT = 0x8,

        /// <summary>
        /// The GET is used to fetch concepts.
        /// </summary>
        GET = 0x10
    }
}