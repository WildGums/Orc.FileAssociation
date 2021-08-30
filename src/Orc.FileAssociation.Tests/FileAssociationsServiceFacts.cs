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
        public async Task AssociateFilesWithApplicationAsyncTestAsync(ApplicationInfo applicationInfo, List<String> extensions, string path)
        {
            var fileAssociationServiceMock = new FileAssociationServiceMock();
            await fileAssociationServiceMock.AssociateFilesWithApplicationAsync(applicationInfo);

            CollectionAssert.AreEqual(extensions, fileAssociationServiceMock.Extensions);
            Assert.AreEqual(path, fileAssociationServiceMock.ApplicationPath);
        }

        private static ApplicationInfo CreateApplicationInfo(string location, List<String> extensions)
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
                new List<String>{ ".txt", ".abc", ".xyz" },
                @"C:\Source\Orc.FileAssociation\output\Debug\Orc.FileAssociation.Example\netcoreapp3.1\Orc.FileAssociation.Example.exe"
            },
            new object[] {CreateApplicationInfo(@"C:\Source\Orc.FileAssociation\output\Debug\Orc.FileAssociation.Example\netcoreapp3.1\Orc.FileAssociation.Example.exe", new List<string> { ".txt.txt", ".xyz.abc" }),
                new List<String>{ ".txt.txt", ".xyz.abc" },
                @"C:\Source\Orc.FileAssociation\output\Debug\Orc.FileAssociation.Example\netcoreapp3.1\Orc.FileAssociation.Example.exe"
            },
              new object[] {CreateApplicationInfo(@"C:\Source\Orc.FileAssociation\output\Debug\Orc.FileAssociation.Example\netcoreapp3.1\Orc.FileAssociation.Example.dll", new List<string> { "txt", "abc", "xyz" }),
                new List<String>{ ".txt", ".abc", ".xyz" },
                @"C:\Source\Orc.FileAssociation\output\Debug\Orc.FileAssociation.Example\netcoreapp3.1\Orc.FileAssociation.Example.exe"
        }
        };
    }

    internal class FileAssociationServiceMock : FileAssociationService
    {
        public string ApplicationPath { get; set; }

        public List<String> Extensions { get; set; } = new();

        protected override void CreateAssociationRegistryKey(string appPath, string extension)
        {
            Extensions.Add(extension);
            ApplicationPath = appPath;
        }
    }
}
