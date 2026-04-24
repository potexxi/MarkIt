using MarkIt.login_register;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarkIt
{
    public class UserManager
    {
        public enum ErrorType
        {
            NoError,
            Unknown,
            ServerUnreachable,
            PrivatKeyAuth,
            UsersFile
        }

        UserManager() { }

        public static Task<(ClassUserList?, ErrorType)> GetUsersFromServer()
        {
            // code inspired by StackOverflow/Autocompletion
            return Task.Run(() =>
            {
                ConnectionInfo connection;
                try
                {
                    PrivateKeyFile privateKey = new PrivateKeyFile(ServerSettings.KeyFilePath);

                    PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(ServerSettings.Username, privateKey);
                    connection = new ConnectionInfo(ServerSettings.PublicIp, ServerSettings.Port, ServerSettings.Username, privateKeyAuth);
                    Logger.logger.Debug("Successfully created connection with private key.");
                }
                catch
                {
                    Logger.logger.Error("Privat key authentication or server unreachable");
                    return (null, ErrorType.PrivatKeyAuth);
                }
                try
                {
                    using (SftpClient client = new SftpClient(connection))
                    {
                        client.Connect();

                        using (var stream = client.OpenRead("/files/users.json"))
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            string content = reader.ReadToEnd();
                            ClassUserList? userList = JsonSerializer.Deserialize<ClassUserList>(content);
                            Logger.logger.Debug("Successfully got users from server.");
                            client.Disconnect();
                            return (userList, ErrorType.NoError);
                        }
                    }
                }
                catch (System.Net.Sockets.SocketException)
                {
                    Logger.logger.Error("Server unreachable.");
                    return (null, ErrorType.ServerUnreachable);
                }
                catch
                {
                    Logger.logger.Fatal("No file \"users.json\" or incorrect format.");
                    return (null, ErrorType.UsersFile);
                }
            });
        }

        public static void WriteUsersToServer(ClassUserList userList)
        {
            // code inspired by StackOverflow/Autocompletion
            ConnectionInfo connection;
            try
            {
                PrivateKeyFile privateKey = new PrivateKeyFile(ServerSettings.KeyFilePath);

                PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(ServerSettings.Username, privateKey);
                connection = new ConnectionInfo(ServerSettings.PublicIp, ServerSettings.Port, ServerSettings.Username, privateKeyAuth);
                Logger.logger.Debug("Successfully created connection with private key.");
            }
            catch
            {
                Logger.logger.Error("Privat key authentication or server unreachable.");
                throw new Exception("server");
            }
            try
            {
                using (SftpClient client = new SftpClient(connection))
                {
                    try
                    {
                        client.Connect();
                    }
                    catch
                    {
                        Logger.logger.Error("Server unreachable.");
                        throw new Exception("server");
                    }
                    JsonSerializerOptions jsonoptions = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    using (var stream = client.OpenWrite("files/users.json"))
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string json = JsonSerializer.Serialize(userList, options: jsonoptions);
                        writer.Write(json);
                        Logger.logger.Debug("Successfully wrote users to server.");

                    }
                    client.Disconnect();
                }
            }
            catch (Exception e)
            {
                if (e.Message == "server")
                {
                    throw new Exception("server");
                }
                Logger.logger.Error("No file \"users.json\".");
                throw new Exception("users file");
            }

        }

        public static ClassUserList? GetRemeberedUsers()
        {
            if (File.Exists("sources/remembered.json"))
            {
                try
                {
                    using (StreamReader reader = new StreamReader("sources/remembered.json"))
                    {
                        return JsonSerializer.Deserialize<ClassUserList>(reader.ReadToEnd());
                    }
                }
                catch
                {
                    Logger.logger.Warning("remembered.json incorrect format.");
                    return null;
                }
            }
            else
            {
                Logger.logger.Information("No remembered users, continue to login.");
                return null;
            }
        }

        public static void WriteToRememberedUsers(ClassUser user)
        {
            ClassUserList? userList = GetRemeberedUsers();
            if (userList == null)
            {
                userList = new ClassUserList();
            }
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            userList.Users.Add(user);
            using (StreamWriter writer = new StreamWriter("sources/remembered.json"))
            {
                writer.Write(JsonSerializer.Serialize(userList, options: options));
            }
        }
    }
}
