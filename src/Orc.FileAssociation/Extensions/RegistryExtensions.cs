namespace Orc.FileAssociation;

using System;
using System.Linq;
using Catel.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

public static class RegistryExtensions
{
    private static readonly ILogger Logger = LogManager.GetLogger(typeof(RegistryExtensions));

    public static void SetRegistryValue(this RegistryHive registryHive, string key, string valueName, string value)
    {
        Logger.LogDebug("Setting registry value '{RegistryHive}\\{Key}' => '{ValueName}' = '{Value}'", registryHive, key, valueName, value);

        using var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default);
        using var registryKey = registry.CreateSubKey(key);
        registryKey.SetValue(valueName, value);
    }

    public static bool IsRegistryKeyAvailable(this RegistryHive registryHive, string key)
    {
        using var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default);
        using var registryKey = registry.OpenSubKey(key);
        return registryKey is not null;
    }

    public static bool IsRegistryValueAvailable(this RegistryHive registryHive, string key, string valueName)
    {
        using var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default);
        using var registryKey = registry.OpenSubKey(key);
        if (registryKey is null)
        {
            return false;
        }

        var valueExists = registryKey.GetValueNames().Any(x => string.Equals(valueName, x, StringComparison.OrdinalIgnoreCase));
        return valueExists;
    }

    public static void RemoveRegistryKey(this RegistryHive registryHive, string key)
    {
        Logger.LogDebug("Removing registry key '{RegistryHive}\\{Key}'", registryHive, key);

        using var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default);
        registry.DeleteSubKeyTree(key);
    }

    public static void RemoveRegistryValue(this RegistryHive registryHive, string key, string valueName)
    {
        Logger.LogDebug("Removing registry key value '{RegistryHive}\\{Key}' => '{ValueName}'", registryHive, key, valueName);

        using var registry = RegistryKey.OpenBaseKey(registryHive, RegistryView.Default);
        using var registryKey = registry?.CreateSubKey(key);
        registryKey?.DeleteValue(valueName);
    }
}
