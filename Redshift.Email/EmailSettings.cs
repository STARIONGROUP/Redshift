#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailSettings.cs" company="RHEA System S.A.">
//    Copyright (c) 2018 RHEA System S.A.
//
//    Author: Alex Vorobiev
//
//    This file is part of Redshift.Email.
//
//    Redshift.Email is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Lesser General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    Redshift.Email is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Lesser General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with Redshift.Email.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Redshift.Email
{
    /// <summary>
    /// Defines email SMTP server settings settings.
    /// </summary>
    public class EmailSettings
    {
        /// <summary>
        /// Gets or sets the SMTP host name.
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// Gets or sets the SMTP host port.
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the SMTP server requires authentication.
        /// </summary>
        public bool NeedsAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the SMTP account username.
        /// </summary>
        public string SmtpUsername { get; set; }

        /// <summary>
        /// Gets or sets the SMTP account password.
        /// </summary>
        public string SmtpPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SSL is required by the server.
        /// </summary>
        public bool SmtpSslRequired { get; set; }

        /// <summary>
        /// Gets or sets email address to be used in the "from" field.
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets a prefix to be used by all emails in their subject fields.
        /// </summary>
        public string SubjectPrefix { get; set; }
    }
}
