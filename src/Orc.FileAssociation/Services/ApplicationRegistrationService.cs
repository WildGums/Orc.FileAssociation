namespace Orc.FileAssociation
{
    using System;
    using Catel.Logging;
    using Microsoft.Win32;

    public class ApplicationRegistrationService : IApplicationRegistrationService
    {
        private const string ClassesRegistryKeyName = "Software\\Classes";
        private const string RegisteredApplicationRegistryKeyName = "Software\\RegisteredApplications";

        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public virtual bool IsApplicationRegistered(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Checking if application '{0}' is registered", applicationInfo.Name);

            if (!IsApplicationAddedToClassesRoot(applicationInfo))
            {
                Log.Debug("Application not added to classes root");
                return false;
            }

            if (!IsFileAssociationCapabilitiesAdded(applicationInfo))
            {
                Log.Debug("Application not added to file association capabilities");
                return false;
            }

            if (!IsAppAddedToRegisteredApps(applicationInfo))
            {
                Log.Debug("Application not added to registered apps");
                return false;
            }

            Log.Debug("Application '{0}' is registered", applicationInfo.Name);

            return true;
        }

        public virtual void RegisterApplication(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Registering application '{0}'", applicationInfo.Name);

            // Step 1: Create app in the classes root
            AddApplicationToClassesRoot(applicationInfo);

            // Step 2: Create app in registry with file association capabilities
            AddFileAssociationCapabilities(applicationInfo);

            // Step 3: Add registered app
            AddAppToRegisteredApps(applicationInfo);

            Log.Debug("Registered application '{0}'", applicationInfo.Name);
        }

        public virtual void UnregisterApplication(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Unregistering application '{0}'", applicationInfo.Name);

            RemoveApplicationFromClassesRoot(applicationInfo);
            RemoveFileAssociationCapabilities(applicationInfo);
            RemoveAppFromRegisteredApps(applicationInfo);

            Log.Debug("Unregistered application '{0}'", applicationInfo.Name);
        }

        protected virtual bool IsApplicationAddedToClassesRoot(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            var registryHive = RegistryHive.CurrentUser;

            var registryKey = string.Format("{0}\\{1}", ClassesRegistryKeyName, applicationInfo.Name);
            var keyExists = registryHive.IsRegistryKeyAvailable(registryKey);
            return keyExists;
        }

        protected virtual void AddApplicationToClassesRoot(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Adding application '{0}' to classes root", applicationInfo.Name);

            var registryHive = RegistryHive.CurrentUser;

            //[HKEY_CURRENT_USER\Software\Classes\MyAppHTML]
            //@="MyApp HTML Document"
            registryHive.SetRegistryValue(string.Format("{0}\\{1}", ClassesRegistryKeyName, applicationInfo.Name), "", applicationInfo.Title);

            //[HKEY_CURRENT_USER\Software\Classes\MyAppHTML\Application]
            //"ApplicationCompany"="Fictional Software Inc."
            registryHive.SetRegistryValue(string.Format("{0}\\{1}\\Application", ClassesRegistryKeyName, applicationInfo.Name), "ApplicationCompany", applicationInfo.Company);

            //[HKEY_CURRENT_USER\Software\Classes\MyAppHTML\shell]
            //@="open"
            registryHive.SetRegistryValue(string.Format("{0}\\{1}\\shell", ClassesRegistryKeyName, applicationInfo.Name), "", "open");

            //[HKEY_CURRENT_USER\Software\Classes\MyAppHTML\shell\open\command]
            //@="\"C:\\the app path\\testassoc.exe\""
            registryHive.SetRegistryValue(string.Format("{0}\\{1}\\shell\\open\\command", ClassesRegistryKeyName, applicationInfo.Name), "", string.Format("\"{0}\" \"%1\"", applicationInfo.Location));
        }

        protected virtual void RemoveApplicationFromClassesRoot(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Removing application '{0}' from classes root", applicationInfo.Name);

            var registryHive = RegistryHive.CurrentUser;

            //[HKEY_CURRENT_USER\Software\Classes]
            registryHive.RemoveRegistryKey(string.Format("{0}\\{1}", ClassesRegistryKeyName, applicationInfo.Name));
        }

        protected virtual bool IsFileAssociationCapabilitiesAdded(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            var registryHive = RegistryHive.CurrentUser;

            var softwareKey = GetCurrentUserSoftwareKeyName(applicationInfo);
            var keyExists = registryHive.IsRegistryKeyAvailable(softwareKey);
            return keyExists;
        }

        protected virtual void AddFileAssociationCapabilities(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Adding file association capabilities '{0}' to current user", applicationInfo.Name);

            var registryHive = RegistryHive.CurrentUser;

            var softwareKey = GetCurrentUserSoftwareKeyName(applicationInfo);

            //[HKEY_CURRENT_USER\Software\FictionalSoftware\MyApp\Capabilities]
            //"ApplicationDescription" = "My Fictional Application"
            registryHive.SetRegistryValue(string.Format("{0}\\Capabilities", softwareKey), "ApplicationDescription", applicationInfo.Title);

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

                Log.Debug("Adding file association capability for extension '{0}'", finalExtension);

                registryHive.SetRegistryValue(string.Format("{0}\\Capabilities\\FileAssociations", softwareKey), finalExtension, applicationInfo.Name);
            }
        }

        protected virtual void RemoveFileAssociationCapabilities(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Removing file association capabilities '{0}' from current user", applicationInfo.Name);

            var registryHive = RegistryHive.CurrentUser;

            //[HKEY_CURRENT_USER\Software\FictionalSoftware\MyApp]
            var softwareKey = GetCurrentUserSoftwareKeyName(applicationInfo);
            registryHive.RemoveRegistryKey(softwareKey);
        }

        protected virtual bool IsAppAddedToRegisteredApps(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            var registryHive = RegistryHive.CurrentUser;

            var keyExists = registryHive.IsRegisteryValueAvailable(RegisteredApplicationRegistryKeyName, applicationInfo.Name);
            return keyExists;
        }

        protected virtual void AddAppToRegisteredApps(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Adding app {0}' to registered apps", applicationInfo.Name);

            var registryHive = RegistryHive.CurrentUser;

            //[HKEY_CURRENT_USER\Software\RegisteredApplications]
            //"MyApp" ="Software\\FictionalSoftware\\MyApp\\Capabilities"
            registryHive.SetRegistryValue(RegisteredApplicationRegistryKeyName, applicationInfo.Name,
                string.Format("{0}\\Capabilities", GetCurrentUserSoftwareKeyName(applicationInfo)));
        }

        protected virtual void RemoveAppFromRegisteredApps(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            Log.Debug("Removing app {0}' from registered apps", applicationInfo.Name);

            var registryHive = RegistryHive.CurrentUser;

            registryHive.RemoveRegistryValue(RegisteredApplicationRegistryKeyName, applicationInfo.Name);
        }

        protected virtual string GetCurrentUserSoftwareKeyName(ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationInfo);

            return string.Format("Software\\{0}\\{1}", applicationInfo.Company, applicationInfo.Name);
        }
    }
}
