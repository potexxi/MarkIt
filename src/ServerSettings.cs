using System.Net.Http;
namespace MarkIt
{
    public static class ServerSettings
    {
        public static string? URL {  get; private set; }

        public async static Task Init()
        {
            HttpClient client = new HttpClient();
            URL = await client.GetStringAsync("https://gist.githubusercontent.com/potexxi/2a982358d449e60b466923f0fc5127b9/raw/api-url.txt?cache=" + DateTime.UtcNow.Ticks);
            Logger.logger.Debug($"API-URL: {URL}");
        }
    }
}
