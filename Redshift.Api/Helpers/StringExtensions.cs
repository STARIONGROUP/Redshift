#region Copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="RHEA System S.A.">
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

namespace Redshift.Api.Helpers
{
    using System;

    /// <summary>
    /// Useful string extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Convert the string to camel case.
        /// </summary>
        /// <param name="stringToConvert">
        /// The string To Convert.
        /// </param>
        /// <remarks>
        /// <a href="http://csharphelper.com/blog/2014/10/convert-between-pascal-case-camel-case-and-proper-case-in-c/"/>
        /// </remarks>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToCamelCase(this string stringToConvert)
        {
            // If there are 0, just return the string.
            if (stringToConvert == null)
            {
                return null;
            }

            return char.ToLowerInvariant(stringToConvert[0]) + stringToConvert.Substring(1);
        }

        /// <summary>
        /// Converts strings to title case
        /// </summary>
        /// <param name="stringToConvert">
        /// The string To Convert.
        /// </param>
        /// <remarks>
        /// <a href="http://stackoverflow.com/questions/38360688/built-in-method-to-convert-a-string-to-title-case"/>
        /// </remarks>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ToTitleCase(this string stringToConvert)
        {
            var tokens = stringToConvert.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token.Substring(0, 1).ToUpper() + token.Substring(1);
            }

            return string.Join(" ", tokens);
        }

        /// <summary>
        /// Makes the string postgreSQL safe.
        /// </summary>
        /// <param name="stringToPad">The string to pad.</param>
        /// <returns>A string padded with quotes.</returns>
        public static string MakePostgreSqlSafe(this string stringToPad)
        {
            return $"\"{stringToPad}\"";
        }
    }
}
