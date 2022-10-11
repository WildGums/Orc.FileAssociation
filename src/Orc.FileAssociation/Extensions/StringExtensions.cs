namespace Orc.FileAssociation
{
    using Catel.Reflection;

    public static class StringExtensions
    {
        public static string GetApplicationName(this string applicationName)
        {
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                var entryAssembly = AssemblyHelper.GetRequiredEntryAssembly();
                applicationName = entryAssembly.GetName().Name ?? string.Empty;
            }

            return applicationName;
        }
    }
}
