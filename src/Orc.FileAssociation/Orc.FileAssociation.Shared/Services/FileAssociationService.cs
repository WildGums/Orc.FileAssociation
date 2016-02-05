// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileAssociationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    using System.Runtime.InteropServices;
    using Catel;
    using Catel.Logging;

    public class FileAssociationService : IFileAssociationService
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        public void AssociateFilesWithApplication(string applicationName = null)
        {
            applicationName = applicationName.GetApplicationName();

            Log.Info("Associating files with '{0}'", applicationName);

            var applicationAssociationRegistrationUi = (IApplicationAssociationRegistrationUI)new ApplicationAssociationRegistrationUI();
            var hr = applicationAssociationRegistrationUi.LaunchAdvancedAssociationUI(applicationName);
            var exception = Marshal.GetExceptionForHR(hr);
            if (exception != null)
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
    }
}