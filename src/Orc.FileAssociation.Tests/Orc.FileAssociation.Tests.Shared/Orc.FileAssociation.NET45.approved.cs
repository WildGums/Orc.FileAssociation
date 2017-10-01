[assembly: System.Resources.NeutralResourcesLanguageAttribute("en-US")]
[assembly: System.Runtime.InteropServices.ComVisibleAttribute(false)]
[assembly: System.Runtime.Versioning.TargetFrameworkAttribute(".NETFramework,Version=v4.5", FrameworkDisplayName=".NET Framework 4.5")]


public class static ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.FileAssociation
{
    
    public class ApplicationInfo
    {
        public ApplicationInfo(string company, string name, string title, string location) { }
        public ApplicationInfo(System.Reflection.Assembly assembly) { }
        public string Company { get; }
        public string Location { get; }
        public string Name { get; }
        public System.Collections.Generic.List<string> SupportedExtensions { get; }
        public string Title { get; }
    }
    public class ApplicationRegistrationService : Orc.FileAssociation.IApplicationRegistrationService
    {
        public ApplicationRegistrationService() { }
        protected virtual void AddApplicationToClassesRoot(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual void AddAppToRegisteredApps(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual void AddFileAssociationCapabilities(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual string GetCurrentUserSoftwareKeyName(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual bool IsAppAddedToRegisteredApps(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual bool IsApplicationAddedToClassesRoot(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        public virtual bool IsApplicationRegistered(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual bool IsFileAssociationCapabilitiesAdded(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        public virtual void RegisterApplication(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual void RemoveAppFromRegisteredApps(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual void RemoveApplicationFromClassesRoot(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual void RemoveFileAssociationCapabilities(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        public virtual void UnregisterApplication(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
    }
    public class FileAssociationService : Orc.FileAssociation.IFileAssociationService
    {
        public FileAssociationService() { }
        public void AssociateFilesWithApplication(string applicationName = null) { }
        [System.Runtime.InteropServices.GuidAttribute("1968106d-f3b5-44cf-890e-116fcb9ecef1")]
        public class ApplicationAssociationRegistrationUI
        {
            public ApplicationAssociationRegistrationUI() { }
        }
        [System.Runtime.InteropServices.GuidAttribute("1f76a169-f994-40ac-8fc8-0959e8874710")]
        [System.Runtime.InteropServices.InterfaceTypeAttribute(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
        public interface IApplicationAssociationRegistrationUI
        {
            int LaunchAdvancedAssociationUI(string pszAppRegName);
        }
    }
    public interface IApplicationRegistrationService
    {
        bool IsApplicationRegistered(Orc.FileAssociation.ApplicationInfo applicationInfo);
        void RegisterApplication(Orc.FileAssociation.ApplicationInfo applicationInfo);
        void UnregisterApplication(Orc.FileAssociation.ApplicationInfo applicationInfo);
    }
    public class static IApplicationRegistrationServiceExtensions
    {
        public static void UpdateRegistration(this Orc.FileAssociation.IApplicationRegistrationService applicationRegistrationService, Orc.FileAssociation.ApplicationInfo applicationInfo) { }
    }
    public interface IFileAssociationService
    {
        void AssociateFilesWithApplication(string applicationName = null);
    }
    public class static RegistryExtensions
    {
        public static bool IsRegisteryValueAvailable(this Microsoft.Win32.RegistryHive registryHive, string key, string valueName) { }
        public static bool IsRegistryKeyAvailable(this Microsoft.Win32.RegistryHive registryHive, string key) { }
        public static void RemoveRegistryKey(this Microsoft.Win32.RegistryHive registryHive, string key) { }
        public static void RemoveRegistryValue(this Microsoft.Win32.RegistryHive registryHive, string key, string valueName) { }
        public static void SetRegistryValue(this Microsoft.Win32.RegistryHive registryHive, string key, string valueName, string value) { }
    }
    public class static StringExtensions
    {
        public static string GetApplicationName(this string applicationName) { }
    }
}