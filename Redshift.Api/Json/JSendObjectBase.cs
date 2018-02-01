#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JSendObjectBase.cs" company="RHEA System S.A.">
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
    using System.Runtime.Serialization;
    using Nancy;
    using Newtonsoft.Json;

    /// <summary>
    /// The base class for JSend objects.
    /// </summary>
    public abstract class JSendObjectBase : IJSendObject
    {
        /// <summary>
        /// Gets the status.
        /// </summary>
        [JsonProperty]
        public virtual StatusMessage Status
        {
            get
            {
                return StatusMessage.success;
            }
        }

        /// <summary>
        /// Gets or sets the HTTP status code. 
        /// </summary>
        [IgnoreDataMember]
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the route that this object is a response to.
        /// </summary>
        [IgnoreDataMember]
        public string Route { get; set; }

        /// <summary>
        /// Gets or sets the HTTP method.
        /// </summary>
        [IgnoreDataMember]
        public string Method { get; set; }
    }
}
