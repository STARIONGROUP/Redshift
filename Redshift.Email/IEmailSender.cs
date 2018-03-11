#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEmailSender.cs" company="RHEA System S.A.">
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
    using System.Threading.Tasks;

    /// <summary>
    /// The public interface for an email sending framework.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Initializes the calss with proper settings.
        /// </summary>
        /// <param name="settings">A fully initialized settings file.</param>
        void Initialize(EmailSettings settings);

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="toAddress">The address to send the email to.</param>
        /// <param name="template">The template to use for the email.</param>
        /// <returns>The async task.</returns>
        Task SendEmailAsync(string toAddress, IEmailTemplate template);

        /// <summary>
        /// Sends an email to a specified address using a specified template.
        /// </summary>
        /// <param name="toAddress">The address to send the email to.</param>
        /// <param name="template">The template to use for the email.</param>
        void SendEmail(string toAddress, IEmailTemplate template);
    }
}
