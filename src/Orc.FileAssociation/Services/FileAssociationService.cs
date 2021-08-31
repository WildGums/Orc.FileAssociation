// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileAssociationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Logging;
    using Microsoft.Win32;

    public class FileAssociationService : IFileAssociationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        [ObsoleteEx(Message = "Not supported in Windows 10.",
                  TreatAsErrorFromVersion = "4.2.0",
                  RemoveInVersion = "5.0.0",
                  ReplacementTypeOrMember = "AssociateFilesWithApplicationAsync(ApplicationInfo applicationInfo)")]
        public void AssociateFilesWithApplication(string applicationName = null)
        {
            applicationName = applicationName.GetApplicationName();

            Log.Info("Associating files with '{0}'", applicationName);

            var applicationAssociationRegistrationUi = (IApplicationAssociationRegistrationUI)new ApplicationAssociationRegistrationUI();
            var hr = applicationAssociationRegistrationUi.LaunchAdvancedAssociationUI(applicationName);
            var exception = Marshal.GetExceptionForHR(hr);
            if (exception is not null)
            {
                Log.Error(exception, "Failed to associate the files with application '{0}'", applicationName);
                throw exception;
            }

            Log.Info("Associated files with '{0}'", applicationName);
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
            Argument.IsNotNull(() =>  applicationInfo);

            var applicationName = applicationInfo.Name.GetApplicationName();
            var appPath = applicationInfo.Location;
            var subKey = "shell\\open\\command";
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
            var key = Registry.CurrentUser.CreateSubKey(classesSubKey);
            var subKey = key.CreateSubKey(keySubKey);
            subKey.SetValue(name, subKeyValue);
        }

        public async Task UndoAssociationFilesWithApplicationAsync(ApplicationInfo applicationInfo)
        {
            Argument.IsNotNull(() => applicationInfo);

            foreach (var extension in applicationInfo.SupportedExtensions)
            {
                var finalExtension = extension;
                if (!finalExtension.StartsWith("."))
                {
                    finalExtension = "." + finalExtension;
                }
                Log.Debug($"Removing extension association {finalExtension} capabilities from current user");
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree($"SOFTWARE\\Classes\\{finalExtension}");
                Log.Debug($"Removed extension association for {finalExtension} from current user");
            }

        }
    }
}
