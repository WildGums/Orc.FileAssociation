namespace Orc.FileAssociation.Tests;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Catel.IoC;
using Catel.Services;
using NUnit.Framework;
using FileSystem;

[TestFixture]
public class FileAssociationsServiceFacts
{
    private IFileService _fileService;
    private IDirectoryService _directoryService;
    private ILanguageService _languageService;

    [Test]
    [TestCaseSource(nameof(Cases))]
    public async Task AssociateFilesWithApplicationAsyncTestAsync(ApplicationInfo applicationInfo, List<string> expectedClassesSubKey, string expectedSubKey, string expectedSubKeyValue, string expectedName)
    {
        var fileAssociationServiceMock = new FileAssociationServiceMock(_fileService, _directoryService, _languageService);
        await fileAssociationServiceMock.AssociateFilesWithApplicationAsync(applicationInfo);

        Assert.That(fileAssociationServiceMock.ClassesSubKey, Is.EqualTo(expectedClassesSubKey).AsCollection);
        Assert.That(fileAssociationServiceMock.SubKey, Is.EqualTo(expectedSubKey));
        Assert.That(fileAssociationServiceMock.SubKeyValue, Is.EqualTo(expectedSubKeyValue));
        Assert.That(fileAssociationServiceMock.Name, Is.EqualTo(expectedName));
    }

    [Test]
    [Explicit]
    public async Task OpenPropertiesWindowForExtensionAsyncTestAsync()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var finalPath = Path.Combine(appDataPath, "WildGums", "Temp");
        var filePath = Path.Combine(finalPath, "Click on 'Change' to select default csv handler.csv");
        var fileAssociationServiceMock = new FileAssociationServiceMock(_fileService, _directoryService, _languageService);

        await fileAssociationServiceMock.OpenPropertiesWindowForExtensionAsync("csv", finalPath);

        Assert.That(File.Exists(filePath), Is.True);

        _directoryService.Delete(finalPath);
    }

    private static ApplicationInfo CreateApplicationInfo(string location, List<string> extensions)
    {
        var applicationInfo = new ApplicationInfo(
            "WildGums",
            "Orc.FileAssociation",
            "Orc.FileAssociation",
            location);
        applicationInfo.SupportedExtensions.AddRange(extensions);
        return applicationInfo;
    }

    [OneTimeSetUp]
    protected void GetServices()
    {
        var serviceLocator = ServiceLocator.Default;
        _fileService = serviceLocator.ResolveType<IFileService>();
        _directoryService = serviceLocator.ResolveType<IDirectoryService>();
        _languageService = serviceLocator.ResolveType<ILanguageService>();
    }
        
    private static readonly object[] Cases =
    {
        new object[] {CreateApplicationInfo(@"C:\Source\Orc.FileAssociation\output\Debug\Orc.FileAssociation.Example\netcoreapp3.1\Orc.FileAssociation.Example.exe", new List<string> { "txt", "abc", "xyz" }),
            new List<string> { @"Software\Classes\.txt", @"Software\Classes\.abc", @"Software\Classes\.xyz" },
            @"shell\open\command",
            "C:\\Source\\Orc.FileAssociation\\output\\Debug\\Orc.FileAssociation.Example\\netcoreapp3.1\\Orc.FileAssociation.Example.exe \"%1\"",
            string.Empty
        },
        new object[] {CreateApplicationInfo(@"C:\Source\Orc.FileAssociation\output\Debug\Orc.FileAssociation.Example\netcoreapp3.1\Orc.FileAssociation.Example.exe", new List<string> { ".txt.txt", ".xyz.abc" }),
            new List<string> { @"Software\Classes\.txt.txt", @"Software\Classes\.xyz.abc" },
            @"shell\open\command",
            "C:\\Source\\Orc.FileAssociation\\output\\Debug\\Orc.FileAssociation.Example\\netcoreapp3.1\\Orc.FileAssociation.Example.exe \"%1\"",
            string.Empty
        },
        new object[] {CreateApplicationInfo(@"C:\Source\Orc.FileAssociation\output\Debug\Orc.FileAssociation.Example\netcoreapp3.1\Orc.FileAssociation.Example.dll", new List<string> { "txt", "abc", "xyz" }),
            new List<string> { @"Software\Classes\.txt", @"Software\Classes\.abc", @"Software\Classes\.xyz" },
            @"shell\open\command",
            "C:\\Source\\Orc.FileAssociation\\output\\Debug\\Orc.FileAssociation.Example\\netcoreapp3.1\\Orc.FileAssociation.Example.exe \"%1\"",
            string.Empty
        }
    };
}

internal class FileAssociationServiceMock : FileAssociationService
{

    public FileAssociationServiceMock(IFileService fileService, IDirectoryService directoryService, ILanguageService languageService)
        : base(fileService, directoryService, languageService)
    {
    }

    public List<string> ClassesSubKey { get; set; } = new();

    public string SubKey { get; set; }

    public string SubKeyValue { get; set; }

    public string Name { get; set; }

    protected override void CreateAssociationRegistryKey(string classesSubKey, string subKey, string subKeyValue, string name)
    {
        ClassesSubKey.Add(classesSubKey);
        SubKey = subKey;
        SubKeyValue = subKeyValue;
        Name = name;
    }
}
