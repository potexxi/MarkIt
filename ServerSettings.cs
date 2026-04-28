using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MarkIt
{
    public static class ServerSettings
    {
        public static int Port {  get; private set; }
        public static string? PublicIp {  get; private set; }
        public static string? Username {  get; private set; }
        public static string? KeyFilePath {  get; private set; }

        public static void Init(int port, string publicip, string username, string keyfilepath)
        {
            Port = port;
            PublicIp = publicip;
            Username = username;
            KeyFilePath = keyfilepath;
        }
    }
}
