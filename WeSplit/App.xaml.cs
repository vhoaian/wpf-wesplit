using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WeSplit.Screens;

namespace WeSplit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (UserSettings.ShowSplash)
            {
                Splash window = new Splash();
                window.Show();
            } else
            {
                MainWindow window = new MainWindow();
                window.Show();
            }
        }
    }
}
