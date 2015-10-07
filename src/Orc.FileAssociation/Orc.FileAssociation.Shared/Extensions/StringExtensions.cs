// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    using Catel.Reflection;

    public static class StringExtensions
    {
        public static string GetApplicationName(this string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                var entryAssembly = AssemblyHelper.GetEntryAssembly();
                applicationName = entryAssembly.Title();
            }

            return applicationName;
        }
    }
}