#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiIgnoreMethodAttribute.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Api.
//
//    Redshift.Api is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Api is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
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
    /// Ignore the a combination of REST method in API.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class ApiIgnoreMethodAttribute : Attribute
    {
        /// <summary>
        /// The backing field for <see cref="IgnoredMethods"/>.
        /// </summary>
        private RestMethods ignoredMethods;

        /// <summary>
        /// The backing field for <see cref="Message"/>
        /// </summary>
        private string message;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiIgnoreMethodAttribute"/> class.
        /// </summary>
        /// <param name="ignoredMethods">The ignored methods.</param>
        /// <param name="message">The message to be displayed.</param>
        public ApiIgnoreMethodAttribute(RestMethods ignoredMethods, string message)
        {
            this.ignoredMethods = ignoredMethods;
            this.message = message;
        }

        /// <summary>
        /// Gets the <see cref="message"/>.
        /// </summary>
        public string Message => this.message;

        /// <summary>
        /// Gets the ignored methods.
        /// </summary>
        public RestMethods IgnoredMethods => this.ignoredMethods;
    }
}
