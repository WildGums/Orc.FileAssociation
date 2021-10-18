[assembly: System.Resources.NeutralResourcesLanguage("en-US")]
[assembly: System.Runtime.Versioning.TargetFramework(".NETCoreApp,Version=v5.0", FrameworkDisplayName="")]
public static class ModuleInitializer
{
    public static void Initialize() { }
}
namespace Orc.FileAssociation
{
    public class ApplicationInfo
    {
        public ApplicationInfo(System.Reflection.Assembly assembly) { }
        public ApplicationInfo(string company, string name, string title, string location) { }
        public string Company { get; }
        public string Location { get; }
        public string Name { get; }
        public System.Collections.Generic.List<string> SupportedExtensions { get; }
        public string Title { get; }
    }
    public class ApplicationRegistrationService : Orc.FileAssociation.IApplicationRegistrationService
    {
        public ApplicationRegistrationService() { }
        protected virtual void AddAppToRegisteredApps(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual void AddApplicationToClassesRoot(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
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
        public FileAssociationService(Orc.FileSystem.IFileService fileService, Orc.FileSystem.IDirectoryService directoryService) { }
        [System.Obsolete("Not supported in Windows 10. Use `AssociateFilesWithApplicationAsync(ApplicationI" +
            "nfo applicationInfo)` instead. Will be removed in version 5.0.0.", true)]
        public void AssociateFilesWithApplication(string applicationName = null) { }
        public System.Threading.Tasks.Task AssociateFilesWithApplicationAsync(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        protected virtual void CreateAssociationRegistryKey(string classesSubKey, string keySubKey, string subKeyValue, string name) { }
        public virtual System.Threading.Tasks.Task OpenPropertiesWindowForExtensionAsync(string extension) { }
        public virtual System.Threading.Tasks.Task OpenPropertiesWindowForExtensionAsync(string extension, string path) { }
        public System.Threading.Tasks.Task UndoAssociationFilesWithApplicationAsync(Orc.FileAssociation.ApplicationInfo applicationInfo) { }
        [System.Runtime.InteropServices.Guid("1968106d-f3b5-44cf-890e-116fcb9ecef1")]
        public class ApplicationAssociationRegistrationUI
        {
            public ApplicationAssociationRegistrationUI() { }
        }
        [System.Runtime.InteropServices.Guid("1f76a169-f994-40ac-8fc8-0959e8874710")]
        [System.Runtime.InteropServices.InterfaceType(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIUnknown)]
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
    public static class IApplicationRegistrationServiceExtensions
    {
        public static void UpdateRegistration(this Orc.FileAssociation.IApplicationRegistrationService applicationRegistrationService, Orc.FileAssociation.ApplicationInfo applicationInfo) { }
    }
    public interface IFileAssociationService
    {
        [System.Obsolete("Not supported in Windows 10. Use `AssociateFilesWithApplicationAsync(ApplicationI" +
            "nfo applicationInfo)` instead. Will be removed in version 5.0.0.", true)]
        void AssociateFilesWithApplication(string applicationName = null);
        System.Threading.Tasks.Task AssociateFilesWithApplicationAsync(Orc.FileAssociation.ApplicationInfo applicationInfo);
        System.Threading.Tasks.Task OpenPropertiesWindowForExtensionAsync(string extension);
        System.Threading.Tasks.Task UndoAssociationFilesWithApplicationAsync(Orc.FileAssociation.ApplicationInfo applicationInfo);
    }
    public static class RegistryExtensions
    {
        public static bool IsRegisteryValueAvailable(this Microsoft.Win32.RegistryHive registryHive, string key, string valueName) { }
        public static bool IsRegistryKeyAvailable(this Microsoft.Win32.RegistryHive registryHive, string key) { }
        public static void RemoveRegistryKey(this Microsoft.Win32.RegistryHive registryHive, string key) { }
        public static void RemoveRegistryValue(this Microsoft.Win32.RegistryHive registryHive, string key, string valueName) { }
        public static void SetRegistryValue(this Microsoft.Win32.RegistryHive registryHive, string key, string valueName, string value) { }
    }
    public static class StringExtensions
    {
        public static string GetApplicationName(this string applicationName) { }
    }
}