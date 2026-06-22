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
                if (e.Exception is Supabase.Gotrue.Exceptions.GotrueException ||
                    e.Exception is System.Net.Http.HttpRequestException ||
                    e.Exception is System.Net.Sockets.SocketException)
                {
                    box = new WindowMessageBox("Server unreachable", "Please try again later or continue as guest.");
                    box.ShowDialog();
                    e.Handled = true;
                    return;
                }
                box = new WindowMessageBox("Unexpected Error!", "A unexpected error appeared. Please try again, or restart \"MarkIt\".");
                box.ShowDialog();
                Logger.logger.Fatal($"Unexpected Error: {e.Exception}");
                if (Current.MainWindow?.IsLoaded == false) {
                    Environment.Exit(1); // clean ending
                }
                e.Handled = true;
            };
        }
        // Ende
        private HttpClient client;
        private async void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                await ServerSettings.Init();
                await client.GetStringAsync(ServerSettings.URL);
            }
            catch
            {

            }
        }
    }
}
