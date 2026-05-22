using System.Configuration;
using System.Data;
using System.Windows;

namespace MarkIt
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Microsoft forum
        // search: dispatcher exceptions over the whole application
        // link: https://learn.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netframework-4.8.1
        public App()
        {
            DispatcherUnhandledException += (s,e) =>
            {
                WindowMessageBox box = new WindowMessageBox("Unexpected Error!", "A unexpected error forced the applicatio to stop, please restart \"MarkIt\".");
                box.ShowDialog();
                Logger.logger.Fatal($"Unexpected Error: {e.Exception}");
                e.Handled = true;
            };
        }
        // Ende
    }
}
