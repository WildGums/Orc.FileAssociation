// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileAssociationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    using System;
    using System.Runtime.InteropServices;
    using Catel;
    using Catel.Logging;
    using Microsoft.Win32;

    public class FileAssociationService : IFileAssociationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public void AssociateFilesWithApplication(ApplicationInfo applicationInfo)
        {
            string applicationName = null;
            if (applicationInfo.Name.GetApplicationName() is not null)
            {
               applicationName = applicationInfo.Name.GetApplicationName();
            }

            Log.Info("Associating files with '{0}'", applicationName);

            foreach (var extension in applicationInfo.SupportedExtensions)
            {
                var finalExtension = extension;
                if (!finalExtension.StartsWith("."))
                {
                    finalExtension = "." + finalExtension;
                }
                var appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                var key = Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + finalExtension);
                var subKey = key.CreateSubKey("shell\\open\\command");
                subKey.SetValue("", appPath + " \"%1\"");
            }

            Log.Info("Associated files with '{0}'", applicationName);
        }

        public void UndoAssociationFilesWithApplication(ApplicationInfo applicationInfo)
        {
            foreach (var extension in applicationInfo.SupportedExtensions)
            {
                var finalExtension = extension;
                if (!finalExtension.StartsWith("."))
                {
                    finalExtension = "." + finalExtension;
                }

                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree($"SOFTWARE\\Classes\\{finalExtension}");
                Log.Debug($"Removing extension association {finalExtension} capabilities from current user");
            }

        }
    }
}
