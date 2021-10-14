// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileAssociationService.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation
{
    using System.Threading.Tasks;
    using Orc.FileSystem;

    public interface IFileAssociationService
    {
        [ObsoleteEx(Message = "Not supported in Windows 10.",
                    TreatAsErrorFromVersion = "4.2.0",
                    RemoveInVersion = "5.0.0",
                    ReplacementTypeOrMember = "AssociateFilesWithApplicationAsync(ApplicationInfo applicationInfo)")]
        void AssociateFilesWithApplication(string applicationName = null);

        Task AssociateFilesWithApplicationAsync(ApplicationInfo applicationInfo);

        Task UndoAssociationFilesWithApplicationAsync(ApplicationInfo applicationInfo);

        Task OpenPropertiesWindowForExtensionAsync(string extension, IFileService fileService);
    }
}
