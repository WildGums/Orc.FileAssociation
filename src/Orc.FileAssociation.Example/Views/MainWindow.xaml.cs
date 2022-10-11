namespace Orc.FileAssociation.Views
{
    using Catel.Logging;
    using Orchestra.Logging;

    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();

            var logListener = new TextBoxLogListener(logTextBox);
            logListener.IgnoreCatelLogging = true;
            
            LogManager.AddListener(logListener);
        }
    }
}
