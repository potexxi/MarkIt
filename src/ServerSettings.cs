namespace MarkIt
{
    public static class ServerSettings
    {
        public static int Port {  get; private set; }
        public static string? PublicIp {  get; private set; }

        public static void Init(int port, string publicip)
        {
            Port = port;
            PublicIp = publicip;
        }
    }
}
