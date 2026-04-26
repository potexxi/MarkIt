using MarkIt.login_register;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace MarkIt
{
    public class UserManager
    {
        UserManager() { }

        public static Task<ClassUserList?> GetUsersFromServer()
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
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                    return null;
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
                            return userList;
                        }
                    }
                }
                // Exception for client.connect()
                catch (System.Net.Sockets.SocketException)
                {
                    Logger.logger.Error("Server unreachable.");
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                    return null;
                }
                catch
                {
                    Logger.logger.Fatal("No file \"users.json\" or incorrect format.");
                    MessageBox.Show("Our server caused a fatal error, please try again later.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            });
        }

        public static Task<bool> WriteUsersToServer(ClassUserList userList)
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
                    Logger.logger.Error("Privat key authentication or server unreachable.");
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                    return false;
                }
                try
                {
                    using (SftpClient client = new SftpClient(connection))
                    {
                        client.Connect();
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
                            client.Disconnect();
                            return true;
                        }
                    }
                }
                // Exception for client.connect()
                catch (System.Net.Sockets.SocketException)
                {
                    Logger.logger.Error("Server unreachable.");
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                    return false;
                }
                catch
                {
                    Logger.logger.Fatal("No file \"users.json\" or incorrect format.");
                    MessageBox.Show("Our server caused a fatal error, please try again later.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            });

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
