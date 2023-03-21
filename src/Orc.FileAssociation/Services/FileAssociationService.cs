namespace Orc.FileAssociation;

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Catel.Logging;
using Catel.Services;
using Microsoft.Win32;
using Win32;
using FileSystem;

public class FileAssociationService : IFileAssociationService
{
    private static readonly ILog Log = LogManager.GetCurrentClassLogger();

    private readonly IFileService _fileService;
    private readonly IDirectoryService _directoryService;
    private readonly ILanguageService _languageService;

    public FileAssociationService(IFileService fileService, IDirectoryService directoryService, ILanguageService languageService)
    {
        ArgumentNullException.ThrowIfNull(fileService);
        ArgumentNullException.ThrowIfNull(directoryService);
        ArgumentNullException.ThrowIfNull(languageService);

        _fileService = fileService;
        _directoryService = directoryService;
        _languageService = languageService;
    }

    [Guid("1f76a169-f994-40ac-8fc8-0959e8874710")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IApplicationAssociationRegistrationUI
    {
        [PreserveSig]
        int LaunchAdvancedAssociationUI([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegName);
    }

    [ComImport]
    [Guid("1968106d-f3b5-44cf-890e-116fcb9ecef1")]
    public class ApplicationAssociationRegistrationUI
    {
    }

    public async Task AssociateFilesWithApplicationAsync(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull( applicationInfo);

        var applicationName = applicationInfo.Name.GetApplicationName();
        var appPath = applicationInfo.Location;
        const string subKey = "shell\\open\\command";
        var subKeyValue = $"{appPath} \"%1\"";
        var name = string.Empty;

        Log.Info("Associating files with '{0}'", applicationName);

        foreach (var extension in applicationInfo.SupportedExtensions)
        {
            var finalExtension = extension;
            if (!finalExtension.StartsWith("."))
            {
                finalExtension = "." + finalExtension;
            }

            var classesSubKey = $"Software\\Classes\\{finalExtension}";
            CreateAssociationRegistryKey(classesSubKey, subKey, subKeyValue, name);
        }

        Log.Info("Associated files with '{0}'", applicationName);
    }

    protected virtual void CreateAssociationRegistryKey(string classesSubKey, string keySubKey, string subKeyValue, string name)
    {
        using var key = Registry.CurrentUser.CreateSubKey(classesSubKey);
        using var subKey = key.CreateSubKey(keySubKey);
        subKey.SetValue(name, subKeyValue);
    }

    public async Task UndoAssociationFilesWithApplicationAsync(ApplicationInfo applicationInfo)
    {
        ArgumentNullException.ThrowIfNull(applicationInfo);

        foreach (var extension in applicationInfo.SupportedExtensions)
        {
            var finalExtension = extension;
            if (!finalExtension.StartsWith("."))
            {
                finalExtension = "." + finalExtension;
            }

            Log.Debug($"Removing extension association {finalExtension} capabilities from current user");

            Registry.CurrentUser.DeleteSubKeyTree($"SOFTWARE\\Classes\\{finalExtension}");

            Log.Debug($"Removed extension association for {finalExtension} from current user");
        }
    }

    public virtual async Task OpenPropertiesWindowForExtensionAsync(string extension)
    {
        var appPath = AppDomain.CurrentDomain.BaseDirectory;
        var resourcesPath = Path.Combine(appPath, "Resources");
        await OpenPropertiesWindowForExtensionAsync(extension, resourcesPath);
    }

    public virtual async Task OpenPropertiesWindowForExtensionAsync(string extension, string path)
    {
        var fileName = string.Format(_languageService.GetRequiredString("OrcFileAssociation_FileAssociationService_FileName"), extension);
        var finalPath = Path.Combine(path, fileName);

        _directoryService.Create(path);
        await using (_fileService.Create(finalPath)) { }
        Log.Debug($"Opening properties window for {extension} extension");
        Shell32.ShowFileProperties(finalPath);
        Log.Debug($"Opened properties window for {extension} extension");
    }
}
