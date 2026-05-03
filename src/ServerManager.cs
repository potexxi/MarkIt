using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIt
{
    public class ServerManager
    {
        public enum ServerStatus
        {
            On,
            Off,
            Unknown
        }

        public ServerManager() 
        {
            ServerSettings.Init(10220, "potexxi.duckdns.org", "markit", "sources/markitkey");
        }

        public void InitSupabaseClient()
        {
            MainWindow.supabase = new Supabase.Client(
                "http://potexxi.duckdns.org:10223", 
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYW5vbiIsImlzcyI6InN1cGFiYXNlIiwiaWF0IjoxNzc3NDg2MDEzLCJleHAiOjE5MzUxNjYwMTN9.nD1CD7gEaXtslcNpTd34JV9ACeD-06HjKzv8PERf3S0"
                );
            Logger.logger.Debug("Supabase client successfully initiated.");
        }

        public ServerStatus GetStatus()
        {
            //TODO
            return ServerStatus.On;
        }
    }
}
