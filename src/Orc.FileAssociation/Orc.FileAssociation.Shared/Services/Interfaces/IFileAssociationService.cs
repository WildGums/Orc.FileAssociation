﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileAssociationService.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    public interface IFileAssociationService
    {
        void AssociateFilesWithApplication(string applicationName = null);
    }
}