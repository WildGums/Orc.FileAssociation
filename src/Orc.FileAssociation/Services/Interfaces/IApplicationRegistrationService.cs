// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IApplicationRegistrationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
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