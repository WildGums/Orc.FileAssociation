// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationRegistrationServiceExtensions.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    using Catel;

    public static class IApplicationRegistrationServiceExtensions
    {
        public static void UpdateRegistration(this IApplicationRegistrationService applicationRegistrationService, ApplicationInfo applicationInfo)
        {
            Argument.IsNotNull(() => applicationRegistrationService);
            Argument.IsNotNull(() => applicationInfo);

            // Just a forward call to the register application, maybe in the future we will uninstall / install
            applicationRegistrationService.RegisterApplication(applicationInfo);
        }
    }
}