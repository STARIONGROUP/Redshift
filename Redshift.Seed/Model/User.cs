#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="User.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Seed.
//
//    Redshift.Seed is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Seed is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Seed.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Seed.Model
{
    using System;
    using System.Runtime.Serialization;
    using Api.Attributes;
    using Orm.Attributes;

    [ApiDescription("A normal physical person.")]
    [ApiIgnoreMethod(RestMethods.DELETE, "Users cannot be DELETED.")]
    public class User : Thing<User>
    {
        [IgnoreDataMember]
        public override string TableName => "people";

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        [ApiDescription("A unique username used for authenticaltion.")]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password. Nulled by the api.
        /// </summary>
        [ApiSerializeNull]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the salt. Completely ignored by the api.
        /// </summary>
        [ApiIgnore]
        public string Salt { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [EntityColumnNameOverride("electronicmail")]
        [ApiWarning("The email should follow a specific regex pattern.")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user group uuid.
        /// </summary>
        public Guid Usergroup { get; set; }

        /// <summary>
        /// Gets the usergroup entity.
        /// </summary>
        [IgnoreDataMember]
        public Usergroup UsergroupEntity => Model.Usergroup.Find(this.Usergroup);
    }
}
