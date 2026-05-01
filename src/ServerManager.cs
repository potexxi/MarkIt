using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkIt
{
    public class ServerManager
    {
        public ConnectionInfo? ConnectionInfo { get; private set; }
        public ServerManager() 
        {
            ServerSettings.Init(10220, "potexxi.duckdns.org", "markit", "sources/markitkey");
        }

        public void InitSupabaseClient()
        {
            MainWindow.supabase = new Supabase.Client(
                "http://89.247.162.152:10223", 
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJyb2xlIjoiYW5vbiIsImlzcyI6InN1cGFiYXNlIiwiaWF0IjoxNzc3NDg2MDEzLCJleHAiOjE5MzUxNjYwMTN9.nD1CD7gEaXtslcNpTd34JV9ACeD-06HjKzv8PERf3S0"
                );
            Logger.logger.Debug("Supabase client successfully initiated.");
        }

        public void CreatePrivateKeyAuth()
        {
            ConnectionInfo connection;
            try
            {
                PrivateKeyFile privateKey = new PrivateKeyFile(ServerSettings.KeyFilePath);
                PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(ServerSettings.Username, privateKey);
                ConnectionInfo = new ConnectionInfo(ServerSettings.PublicIp, ServerSettings.Port, ServerSettings.Username, privateKeyAuth);
                Logger.logger.Debug("Successfully initiated connection with private key.");
            }
            catch
            {
                ConnectionInfo = null;
                Logger.logger.Error("Privat key authentication or server unreachable");
            }
        }
    }
}
