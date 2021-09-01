namespace Orc.FileAssociation.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NUnit.Framework;

    [TestFixture]
    public class FileAssociationsServiceFacts
    {
        [Test]
        [TestCaseSource(nameof(Cases))]
        public async Task AssociateFilesWithApplicationAsyncTestAsync(ApplicationInfo applicationInfo, List<string> expectedClassesSubKey, string expectedSubKey, string expectedSubKeyValue, string expectedName)
        {
            var fileAssociationServiceMock = new FileAssociationServiceMock();
            await fileAssociationServiceMock.AssociateFilesWithApplicationAsync(applicationInfo);

            CollectionAssert.AreEqual(expectedClassesSubKey, fileAssociationServiceMock.ClassesSubKey);
            Assert.AreEqual(expectedSubKey, fileAssociationServiceMock.SubKey);
            Assert.AreEqual(expectedSubKeyValue, fileAssociationServiceMock.SubKeyValue);
            Assert.AreEqual(expectedName, fileAssociationServiceMock.Name);
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
}
