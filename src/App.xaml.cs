using MarkIt.settings;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;
using System.Windows.Threading;

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
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(10);
            timer.Tick += Timer_Tick;
            DispatcherUnhandledException += (s,e) =>
            {
                WindowMessageBox box;
                if(e.Exception is Supabase.Gotrue.Exceptions.GotrueException)
                {
                    box = new WindowMessageBox("Server connection closed.", "Please try again.");
                    box.ShowDialog();
                    Timer_Tick(null, null);
                    return;
                }
                box = new WindowMessageBox("Unexpected Error!", "A unexpected error forced the applicatio to stop, please restart \"MarkIt\".");
                box.ShowDialog();
                Logger.logger.Fatal($"Unexpected Error: {e.Exception}");
                e.Handled = true;
                Environment.Exit(0);
            };
        }
        // Ende
        private HttpClient client;
        private async void Timer_Tick(object? sender, EventArgs e)
        {
            await ServerSettings.Init();
            try
            {
                await client.GetStringAsync(ServerSettings.URL);
            }
            catch
            {
                WindowMessageBox box = new WindowMessageBox("Server unreachable.", "It seems, that our server is currently unreachable. Please restart the app, or continue using it restricted.");
                box.ShowDialog();
            }
        }
    }
}
