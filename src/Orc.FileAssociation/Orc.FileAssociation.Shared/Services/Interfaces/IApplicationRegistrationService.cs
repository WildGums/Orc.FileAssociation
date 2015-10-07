// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationRegistrationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    using System.Reflection;

    public interface IApplicationRegistrationService
    {
        bool IsApplicationRegistered(ApplicationInfo applicationInfo);
        void RegisterApplication(ApplicationInfo applicationInfo);
        void UnregisterApplication(ApplicationInfo applicationInfo);
    }
}