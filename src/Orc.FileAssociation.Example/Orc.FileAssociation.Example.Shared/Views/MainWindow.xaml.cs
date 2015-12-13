// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
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