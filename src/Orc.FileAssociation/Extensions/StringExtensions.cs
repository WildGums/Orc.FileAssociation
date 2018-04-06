// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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
                applicationName = entryAssembly.GetName().Name;
            }

            return applicationName;
        }
    }
}