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
