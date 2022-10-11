namespace Orc.FileAssociation
{
    using System;

    public static class IApplicationRegistrationServiceExtensions
    {
        public static void UpdateRegistration(this IApplicationRegistrationService applicationRegistrationService, ApplicationInfo applicationInfo)
        {
            ArgumentNullException.ThrowIfNull(applicationRegistrationService);
            ArgumentNullException.ThrowIfNull(applicationInfo);

            // Just a forward call to the register application, maybe in the future we will uninstall / install
            applicationRegistrationService.RegisterApplication(applicationInfo);
        }
    }
}
