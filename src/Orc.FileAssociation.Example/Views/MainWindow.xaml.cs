// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="WildGums">
//   Copyright (c) 2008 - 2015 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Orc.FileAssociation.Views
{
    using Catel.Logging;
    using Orchestra.Logging;

    public partial class MainWindow 
    {
        #region Constructors
        public MainWindow()
        {
            InitializeComponent();

            var logListener = new TextBoxLogListener(logTextBox);
            logListener.IgnoreCatelLogging = true;
            
            LogManager.AddListener(logListener);
        }
        #endregion
    }
}