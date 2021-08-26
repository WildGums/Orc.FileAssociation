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


            //var applicationAssociationRegistrationUi = (IApplicationAssociationRegistrationUI)new ApplicationAssociationRegistrationUI();
            //var hr = applicationAssociationRegistrationUi.LaunchAdvancedAssociationUI(applicationName);
            //var exception = Marshal.GetExceptionForHR(hr);
            //if (exception is not null)
            //{
            //    Log.Error(exception, "Failed to associate the files with application '{0}'", applicationName);
            //    throw exception;
            //}

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

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        //[Guid("1f76a169-f994-40ac-8fc8-0959e8874710")]
        //[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        //public interface IApplicationAssociationRegistrationUI
        //{
        //    [PreserveSig]
        //    int LaunchAdvancedAssociationUI([MarshalAs(UnmanagedType.LPWStr)] string pszAppRegName);
        //}

        //[ComImport]
        //[Guid("1968106d-f3b5-44cf-890e-116fcb9ecef1")]
        //public class ApplicationAssociationRegistrationUI
        //{
        //}
    }
}
