// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileAssociationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    public interface IFileAssociationService
    {
        void AssociateFilesWithApplication(string applicationName = null);
    }
}