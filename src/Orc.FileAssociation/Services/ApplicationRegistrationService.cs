namespace Orc.FileAssociation;

using System;
using Catel.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;

public class ApplicationRegistrationService : IApplicationRegistrationService
{
    private const string ClassesRegistryKeyName = "Software\\Classes";
    private const string RegisteredApplicationRegistryKeyName = "Software\\RegisteredApplications";

    private static readonly ILogger Logger = LogManager.GetLogger(typeof(ApplicationRegistrationService));

    public virtual bool IsApplicationRegistered(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Checking if application '{ApplicationName}' is registered", applicationInfo.Name);

        if (!IsApplicationAddedToClassesRoot(applicationInfo))
        {
            Logger.LogDebug("Application not added to classes root");
            return false;
        }

        if (!IsFileAssociationCapabilitiesAdded(applicationInfo))
        {
            Logger.LogDebug("Application not added to file association capabilities");
            return false;
        }

        if (!IsAppAddedToRegisteredApps(applicationInfo))
        {
            Logger.LogDebug("Application not added to registered apps");
            return false;
        }

        Logger.LogDebug("Application '{ApplicationName}' is registered", applicationInfo.Name);

        return true;
    }

    public virtual void RegisterApplication(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Registering application '{ApplicationName}'", applicationInfo.Name);

        // Step 1: Create app in the classes root
        AddApplicationToClassesRoot(applicationInfo);

        // Step 2: Create app in registry with file association capabilities
        AddFileAssociationCapabilities(applicationInfo);

        // Step 3: Add registered app
        AddAppToRegisteredApps(applicationInfo);

        Logger.LogDebug("Registered application '{ApplicationName}'", applicationInfo.Name);
    }

    public virtual void UnregisterApplication(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Unregistering application '{ApplicationName}'", applicationInfo.Name);

        RemoveApplicationFromClassesRoot(applicationInfo);
        RemoveFileAssociationCapabilities(applicationInfo);
        RemoveAppFromRegisteredApps(applicationInfo);

        Logger.LogDebug("Unregistered application '{ApplicationName}'", applicationInfo.Name);
    }

    protected virtual bool IsApplicationAddedToClassesRoot(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        const RegistryHive registryHive = RegistryHive.CurrentUser;

        var registryKey = $"{ClassesRegistryKeyName}\\{applicationInfo.Name}";
        var keyExists = registryHive.IsRegistryKeyAvailable(registryKey);
        return keyExists;
    }

    protected virtual void AddApplicationToClassesRoot(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Adding application '{ApplicationName}' to classes root", applicationInfo.Name);

        const RegistryHive registryHive = RegistryHive.CurrentUser;

        //[HKEY_CURRENT_USER\Software\Classes\MyAppHTML]
        //@="MyApp HTML Document"
        registryHive.SetRegistryValue($"{ClassesRegistryKeyName}\\{applicationInfo.Name}", string.Empty, applicationInfo.Title);

        //[HKEY_CURRENT_USER\Software\Classes\MyAppHTML\Application]
        //"ApplicationCompany"="Fictional Software Inc."
        registryHive.SetRegistryValue($"{ClassesRegistryKeyName}\\{applicationInfo.Name}\\Application", "ApplicationCompany", applicationInfo.Company);

        //[HKEY_CURRENT_USER\Software\Classes\MyAppHTML\shell]
        //@="open"
        registryHive.SetRegistryValue($"{ClassesRegistryKeyName}\\{applicationInfo.Name}\\shell", string.Empty, "open");

        //[HKEY_CURRENT_USER\Software\Classes\MyAppHTML\shell\open\command]
        //@="\"C:\\the app path\\testassoc.exe\""
        registryHive.SetRegistryValue($"{ClassesRegistryKeyName}\\{applicationInfo.Name}\\shell\\open\\command", string.Empty, string.Format("\"{0}\" \"%1\"", applicationInfo.Location));
    }

    protected virtual void RemoveApplicationFromClassesRoot(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Removing application '{ApplicationName}' from classes root", applicationInfo.Name);

        const RegistryHive registryHive = RegistryHive.CurrentUser;

        //[HKEY_CURRENT_USER\Software\Classes]
        registryHive.RemoveRegistryKey($"{ClassesRegistryKeyName}\\{applicationInfo.Name}");
    }

    protected virtual bool IsFileAssociationCapabilitiesAdded(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        const RegistryHive registryHive = RegistryHive.CurrentUser;

        var softwareKey = GetCurrentUserSoftwareKeyName(applicationInfo);
        var keyExists = registryHive.IsRegistryKeyAvailable(softwareKey);
        return keyExists;
    }

    protected virtual void AddFileAssociationCapabilities(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Adding file association capabilities '{ApplicationName}' to current user", applicationInfo.Name);

        const RegistryHive registryHive = RegistryHive.CurrentUser;

        var softwareKey = GetCurrentUserSoftwareKeyName(applicationInfo);

        //[HKEY_CURRENT_USER\Software\FictionalSoftware\MyApp\Capabilities]
        //"ApplicationDescription" = "My Fictional Application"
        registryHive.SetRegistryValue($"{softwareKey}\\Capabilities", "ApplicationDescription", applicationInfo.Title);

        //[HKEY_CURRENT_USER\Software\FictionalSoftware\MyApp\Capabilities\FileAssociations]
        //".htm" = "MyAppHTML"
        //".html" = "MyAppHTML"
        foreach (var extension in applicationInfo.SupportedExtensions)
        {
            var finalExtension = extension;
            if (!finalExtension.StartsWith("."))
            {
                finalExtension = "." + finalExtension;
            }

            Logger.LogDebug("Adding file association capability for extension '{Extension}'", finalExtension);

            registryHive.SetRegistryValue($"{softwareKey}\\Capabilities\\FileAssociations", finalExtension, applicationInfo.Name);
        }
    }

    protected virtual void RemoveFileAssociationCapabilities(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Removing file association capabilities '{ApplicationName}' from current user", applicationInfo.Name);

        var registryHive = RegistryHive.CurrentUser;

        //[HKEY_CURRENT_USER\Software\FictionalSoftware\MyApp]
        var softwareKey = GetCurrentUserSoftwareKeyName(applicationInfo);
        registryHive.RemoveRegistryKey(softwareKey);
    }

    protected virtual bool IsAppAddedToRegisteredApps(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        const RegistryHive registryHive = RegistryHive.CurrentUser;

        var keyExists = registryHive.IsRegistryValueAvailable(RegisteredApplicationRegistryKeyName, applicationInfo.Name);
        return keyExists;
    }

    protected virtual void AddAppToRegisteredApps(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Adding app '{ApplicationName}' to registered apps", applicationInfo.Name);

        const RegistryHive registryHive = RegistryHive.CurrentUser;

        //[HKEY_CURRENT_USER\Software\RegisteredApplications]
        //"MyApp" ="Software\\FictionalSoftware\\MyApp\\Capabilities"
        registryHive.SetRegistryValue(RegisteredApplicationRegistryKeyName, applicationInfo.Name,
            $"{GetCurrentUserSoftwareKeyName(applicationInfo)}\\Capabilities");
    }

    protected virtual void RemoveAppFromRegisteredApps(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        Logger.LogDebug("Removing app '{ApplicationName}' from registered apps", applicationInfo.Name);

        const RegistryHive registryHive = RegistryHive.CurrentUser;

        registryHive.RemoveRegistryValue(RegisteredApplicationRegistryKeyName, applicationInfo.Name);
    }

    protected virtual string GetCurrentUserSoftwareKeyName(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        return $"Software\\{applicationInfo.Company}\\{applicationInfo.Name}";
    }
}
