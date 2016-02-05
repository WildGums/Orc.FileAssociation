// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace Orc.FileAssociation.ViewModels
{
    using System;
    using System.Threading.Tasks;
    using Catel;
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Reflection;

    public class MainViewModel : ViewModelBase
    {
        private readonly IApplicationRegistrationService _applicationRegistrationService;
        private readonly IFileAssociationService _fileAssociationService;

        public MainViewModel(IApplicationRegistrationService applicationRegistrationService, IFileAssociationService fileAssociationService)
        {
            Argument.IsNotNull(() => applicationRegistrationService);
            Argument.IsNotNull(() => fileAssociationService);

            _applicationRegistrationService = applicationRegistrationService;
            _fileAssociationService = fileAssociationService;

            var entryAssembly = AssemblyHelper.GetEntryAssembly();
            Title = entryAssembly.Title();
            ApplicationInfo = new ApplicationInfo(entryAssembly);
            FileAssociations = "abc;xyz;txt";

            RegisterApplication = new Command(OnRegisterApplicationExecute, OnRegisterApplicationCanExecute);
            UnregisterApplication = new Command(OnUnregisterApplicationExecute, OnUnregisterApplicationCanExecute);
            AssociateFiles = new Command(OnAssociateFilesExecute, OnAssociateFilesCanExecute);
        }

        #region Properties
        public override string Title { get; protected set; }

        public ApplicationInfo ApplicationInfo { get; protected set; }

        public string FileAssociations { get; set; }

        public bool IsApplicationRegistered { get; private set; }
        #endregion

        #region Commands
        public Command RegisterApplication { get; private set; }

        private bool OnRegisterApplicationCanExecute()
        {
            return !IsApplicationRegistered;
        }

        private void OnRegisterApplicationExecute()
        {
            var applicationInfo = ApplicationInfo;

            applicationInfo.SupportedExtensions.Clear();
            foreach (var extension in FileAssociations.Split(new string[] {",", ";"}, StringSplitOptions.RemoveEmptyEntries))
            {
                applicationInfo.SupportedExtensions.Add(extension);
            }

            _applicationRegistrationService.RegisterApplication(ApplicationInfo);

            UpdateState();
        }

        public Command UnregisterApplication { get; private set; }

        private bool OnUnregisterApplicationCanExecute()
        {
            return IsApplicationRegistered;
        }

        private void OnUnregisterApplicationExecute()
        {
            _applicationRegistrationService.UnregisterApplication(ApplicationInfo);

            UpdateState();
        }

        public Command AssociateFiles { get; private set; }

        private bool OnAssociateFilesCanExecute()
        {
            return IsApplicationRegistered;
        }

        private void OnAssociateFilesExecute()
        {
            _fileAssociationService.AssociateFilesWithApplication(ApplicationInfo.Name);
        }
        #endregion

        #region Methods
        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            UpdateState();
        }

        private void UpdateState()
        {
            IsApplicationRegistered = _applicationRegistrationService.IsApplicationRegistered(ApplicationInfo);
        }
        #endregion
    }
}