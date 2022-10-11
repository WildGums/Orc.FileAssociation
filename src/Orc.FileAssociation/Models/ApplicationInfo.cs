namespace Orc.FileAssociation
{
    using System.Collections.Generic;
    using System.Reflection;
    using System;
    using Catel;
    using Catel.Reflection;

    public class ApplicationInfo
    {
        private string _location;

        public ApplicationInfo(string company, string name, string title, string location)
        {
            Argument.IsNotNullOrWhitespace(() => company);
            Argument.IsNotNullOrWhitespace(() => name);
            Argument.IsNotNullOrWhitespace(() => title);
            Argument.IsNotNullOrWhitespace(() => location);

            Company = company;
            Name = name;
            Title = title;
            _location = location;
            SupportedExtensions = new List<string>();
        }

        public ApplicationInfo(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            Company = assembly.Company() ?? string.Empty;
            Name = assembly.GetName().Name ?? string.Empty;
            Title = assembly.Title() ?? string.Empty;
            _location = assembly.Location;
            SupportedExtensions = new List<string>();
        }

        public string Company { get; private set; }

        public string Name { get; private set; }

        public string Title { get; private set; }

        public string Location
        {
            get
            {
                if (_location.EndsWith("dll"))
                {
                    // Note: we need to make sure this is an executable, otherwise the file association won't work
                    return _location.Replace("dll", "exe", StringComparison.OrdinalIgnoreCase);
                }

                return _location;
            }
            private set => _location = value;
        }

        public List<string> SupportedExtensions { get; private set; }
    }
}
