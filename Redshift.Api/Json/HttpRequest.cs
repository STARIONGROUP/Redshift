#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpRequest.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.Json
{
    using Redshift.Orm.EntityObject;

    /// <summary>
    /// The complex http request
    /// </summary>
    public class HttpRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest"/> class
        /// </summary>
        public HttpRequest()
        {
            this.Data = new IEntityObject[0];
        }

        /// <summary>
        /// Gets or sets the status of the response
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the data returned
        /// </summary>
        public IEntityObject[] Data { get; set; }
    }
}