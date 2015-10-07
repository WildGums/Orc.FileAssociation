// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RegistryExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    using System;
    using System.Linq;
    using Catel.Logging;
    using Microsoft.Win32;

    public static class RegistryExtensions
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public static void SetRegistryValue(this RegistryHive registryHive, string key, string valueName, string value)
        {
            Log.Debug("Setting registry value '{0}\\{1}' => '{2}' = '{3}'", registryHive, key, valueName, value);

            using (var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default))
            {
                using (var registryKey = registry.CreateSubKey(key))
                {
                    registryKey.SetValue(valueName, value);
                }
            }
        }

        public static bool IsRegistryKeyAvailable(this RegistryHive registryHive, string key)
        {
            using (var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default))
            {
                var registryKey = registry.OpenSubKey(key);
                if (registryKey == null)
                {
                    return false;
                }

                return true;
            }
        }

        public static bool IsRegisteryValueAvailable(this RegistryHive registryHive, string key, string valueName)
        {
            using (var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default))
            {
                var registryKey = registry.OpenSubKey(key);
                if (registryKey == null)
                {
                    return false;
                }

                var valueExists = registryKey.GetValueNames().Any(x => string.Equals(valueName, x, StringComparison.OrdinalIgnoreCase));
                registryKey.Dispose();
                return valueExists;
            }
        }

        public static void RemoveRegistryKey(this RegistryHive registryHive, string key)
        {
            Log.Debug("Removing registry key '{0}\\{1}'", registryHive, key);

            using (var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default))
            {
                registry.DeleteSubKeyTree(key);
            }
        }

        public static void RemoveRegistryValue(this RegistryHive registryHive, string key, string valueName)
        {
            Log.Debug("Removing registry key value '{0}\\{1}' => '{2}'", registryHive, key, valueName);

            using (var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default))
            {
                using (var registryKey = registry.CreateSubKey(key))
                {
                    registryKey.DeleteValue(valueName);
                }
            }
        }
    }
}