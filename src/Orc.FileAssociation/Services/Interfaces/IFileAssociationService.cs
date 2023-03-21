namespace Orc.FileAssociation;

using System.Threading.Tasks;

public interface IFileAssociationService
{
    Task AssociateFilesWithApplicationAsync(ApplicationInfo applicationInfo);

    Task UndoAssociationFilesWithApplicationAsync(ApplicationInfo applicationInfo);

    Task OpenPropertiesWindowForExtensionAsync(string extension);
}
