#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JSendObject.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.Json
{
    using System.Runtime.Serialization;
    using Attributes;

    using Newtonsoft.Json;

    /// <summary>
    /// JSend structure object.
    /// </summary>
    [ApiIgnore]
    public class JSendObject : JSendObjectBase
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        [JsonProperty]
        public new StatusMessage Status { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        [JsonProperty]
        public object Data { get; set; }

        /// <summary>
        /// Gets the data payload in JSON form.
        /// </summary>
        [IgnoreDataMember]
        public string SerializedData
        {
            get
            {
                // TODO: This should serialize the custom serializer
                return JsonConvert.SerializeObject(this.Data).Trim();
            }
        }

        /// <summary>
        /// Gets the full serialized object for display in the textual response.
        /// </summary>
        [IgnoreDataMember]
        public string SerializedFull
        {
            get
            {
                // TODO: This should serialize the custom serializer
                return JsonConvert.SerializeObject(this).Trim();
            }
        }
    }
}