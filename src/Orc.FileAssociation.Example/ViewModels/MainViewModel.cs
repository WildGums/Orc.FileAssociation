namespace Orc.FileAssociation.ViewModels;

using System;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Reflection;
using FileSystem;

public class MainViewModel : ViewModelBase
{
    private readonly IApplicationRegistrationService _applicationRegistrationService;
    private readonly IFileAssociationService _fileAssociationService;
    private readonly IFileService _fileService;

    public MainViewModel(IServiceProvider serviceProvider, IApplicationRegistrationService applicationRegistrationService, 
        IFileAssociationService fileAssociationService, IFileService fileService)
        : base(serviceProvider)
    {
        _applicationRegistrationService = applicationRegistrationService;
        _fileAssociationService = fileAssociationService;
        _fileService = fileService;

        var entryAssembly = AssemblyHelper.GetEntryAssembly();
        Title = entryAssembly.Title();
        ApplicationInfo = new ApplicationInfo(entryAssembly);
        FileAssociations = "abc;xyz;txt";

        RegisterApplication = new Command(serviceProvider, OnRegisterApplicationExecute, OnRegisterApplicationCanExecute);
        UnregisterApplication = new Command(serviceProvider, OnUnregisterApplicationExecute, OnUnregisterApplicationCanExecute);
        AssociateFiles = new TaskCommand(serviceProvider, OnAssociateFilesExecuteAsync, OnAssociateFilesCanExecute);
        UndoAssociationFiles = new TaskCommand(serviceProvider, OnUndoAssociateFilesExecuteAsync, OnAssociateFilesCanExecute);
        OpenExtensionProperties = new TaskCommand(serviceProvider, OnOpenExtensionPropertiesAsync);

        Title = "Orc.FileAssociation example";
    }

    public override string Title { get; protected set; }

    public ApplicationInfo ApplicationInfo { get; protected set; }

    public string FileAssociations { get; set; }

    public bool IsApplicationRegistered { get; private set; }

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

    public TaskCommand AssociateFiles { get; private set; }

    private bool OnAssociateFilesCanExecute()
    {
        return IsApplicationRegistered;
    }

    private async Task OnAssociateFilesExecuteAsync()
    {
        await _fileAssociationService.AssociateFilesWithApplicationAsync(ApplicationInfo);
    }

    public TaskCommand UndoAssociationFiles { get; private set; }

    private async Task OnUndoAssociateFilesExecuteAsync()
    {
        await _fileAssociationService.UndoAssociationFilesWithApplicationAsync(ApplicationInfo);
    }

    public TaskCommand OpenExtensionProperties { get; private set; }

    private async Task OnOpenExtensionPropertiesAsync()
    {
        foreach (var extension in FileAssociations.Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries))
        {
            await _fileAssociationService.OpenPropertiesWindowForExtensionAsync(extension);
        }
    }

    protected override async Task InitializeAsync()
    {
        await base.InitializeAsync();
        UpdateState();
    }

    private void UpdateState()
    {
        IsApplicationRegistered = _applicationRegistrationService.IsApplicationRegistered(ApplicationInfo);
    }
}
