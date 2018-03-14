#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailSender.cs" company="RHEA System S.A.">
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
    using MailKit.Net.Smtp;
    using MimeKit;

    /// <summary>
    /// The email sender.
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Gets or sets the settings for the email sender.
        /// </summary>
        public EmailSettings Settings { get; set; }

        /// <summary>
        /// Initializes the calss with proper settings.
        /// </summary>
        /// <param name="settings">A fully initialized settings file.</param>
        public void Initialize(EmailSettings settings)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="toAddress">The address to send the email to.</param>
        /// <param name="template">The template to use for the email.</param>
        /// <returns>The async task.</returns>
        public Task SendEmailAsync(string toAddress, IEmailTemplate template)
        {
            return Task.Run(() => this.SendEmail(toAddress, template));
        }

        /// <summary>
        /// Sends an email to a specified address using a specified template.
        /// </summary>
        /// <param name="toAddress">The address to send the email to.</param>
        /// <param name="template">The template to use for the email.</param>
        public void SendEmail(string toAddress, IEmailTemplate template)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(this.Settings.FromEmail));
            message.To.Add(new MailboxAddress(toAddress));

            message.Subject = $"{this.Settings.SubjectPrefix} {template.Subject}";

            message.Body = new TextPart("plain")
            {
                Text = template.GetPlainText()
            };

            using (var client = new SmtpClient())
            {
                // accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(this.Settings.SmtpHost, this.Settings.SmtpPort, this.Settings.SmtpSslRequired);

                if(this.Settings.NeedsAuthentication)
                {
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(this.Settings.SmtpUsername, this.Settings.SmtpPassword);
                }

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
