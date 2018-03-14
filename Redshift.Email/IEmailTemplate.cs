#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEmailTemplate.cs" company="RHEA System S.A.">
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
    /// Public interface for an email template.
    /// </summary>
    public interface IEmailTemplate
    {
        /// <summary>
        /// The subject of the message.
        /// </summary>
        string Subject { get; }

        /// <summary>
        /// Get or set the dotliquid file of the plaintext.
        /// </summary>
        string TemplatePlainFile { get; }

        /// <summary>
        /// Gets the plain text email body text.
        /// </summary>
        /// <returns>The plain text body text.</returns>
        string GetPlainText();
    }
}
