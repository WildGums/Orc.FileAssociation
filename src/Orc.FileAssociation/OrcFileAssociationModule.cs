namespace Orc.FileAssociation
{
    using Catel.Services;
    using Catel.ThirdPartyNotices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    /// <summary>
    /// Core module which allows the registration of default services in the service collection.
    /// </summary>
    public static class OrcFileAssociationModule
    {
        public static IServiceCollection AddOrcFileAssociation(this IServiceCollection serviceCollection)
        {
            serviceCollection.TryAddSingleton<IApplicationRegistrationService, ApplicationRegistrationService>();
            serviceCollection.TryAddSingleton<IFileAssociationService, FileAssociationService>();

            serviceCollection.AddSingleton<ILanguageSource>(new LanguageResourceSource("Orc.FileAssociation", "Orc.FileAssociation.Properties", "Resources"));

            serviceCollection.AddSingleton<IThirdPartyNotice>((x) => new LibraryThirdPartyNotice("Orc.FileAssociation", "https://github.com/wildgums/orc.fileassociation"));

            return serviceCollection;
        }
    }
}
