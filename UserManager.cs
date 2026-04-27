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
using System.Windows.Controls;

namespace MarkIt
{
    public class UserManager
    {
        public UserManager() { }

        private enum ErrorType
        {
            Unknown,
            OK,
            ServerUnreachable,
            PrivKey,
            UsersFile
        }
        public async Task<ClassUserList?> GetUsersFromServerAndHandleErrors(Grid loadingScreen)
        {
            // code inspired by StackOverflow/Autocompletion
            loadingScreen.Visibility = Visibility.Visible;
            await Task.Delay(1000);
            var (userList,errortype)  = await Task.Run(() =>
            {
                ConnectionInfo connection;
                try
                {
                    PrivateKeyFile privateKey = new PrivateKeyFile(ServerSettings.KeyFilePath);
                    PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(ServerSettings.Username, privateKey);
                    connection = new ConnectionInfo(ServerSettings.PublicIp, ServerSettings.Port, ServerSettings.Username, privateKeyAuth);
                    Logger.logger.Debug("Successfully initiated connection with private key.");
                }
                catch
                {
                    Logger.logger.Error("Privat key authentication or server unreachable");
                    return (null, ErrorType.PrivKey);
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
                            return (userList, ErrorType.OK);
                        }
                    }
                }
                // Exception for client.connect()
                catch (System.Net.Sockets.SocketException)
                {
                    Logger.logger.Error("Server unreachable.");
                    return (null, ErrorType.ServerUnreachable);
                }
                catch (Exception e)
                {
                    Logger.logger.Fatal($"No file \"users.json\" or {e.Message}");
                    return (null, ErrorType.UsersFile);
                }
            });
            loadingScreen.Visibility = Visibility.Hidden;
            if(userList == null)
            {
                if(errortype == ErrorType.ServerUnreachable || errortype == ErrorType.ServerUnreachable)
                {
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                }
                else if(errortype == ErrorType.UsersFile)
                {
                    MessageBox.Show("Our server caused a fatal error, please try again later.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return userList;
        }

        public async Task<bool> WriteUsersToServer(ClassUserList userList, Grid loadingScreen)
        {
            // code inspired by StackOverflow/Autocompletion
            loadingScreen.Visibility= Visibility.Visible;
            var (result, errortype) = await Task.Run(() =>
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
                    return (false, ErrorType.PrivKey);
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
                        using (var stream = client.OpenWrite("/files/users.json"))
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            string json = JsonSerializer.Serialize(userList, options: jsonoptions);
                            writer.Write(json);
                            Logger.logger.Debug("Successfully wrote users to server.");
                        }
                        client.Disconnect();
                        return (true, ErrorType.OK);
                    }
                }
                // Exception for client.connect()
                catch (System.Net.Sockets.SocketException)
                {
                    Logger.logger.Error("Server unreachable.");
                    return (false, ErrorType.ServerUnreachable);
                }
                catch(Exception e)
                {
                    Logger.logger.Fatal($"No file \"users.json\" or {e.Message}");
                    return (false, ErrorType.UsersFile);
                }
            });
            loadingScreen.Visibility = Visibility.Hidden;
            if (result == false)
            {
                if (errortype == ErrorType.ServerUnreachable || errortype == ErrorType.ServerUnreachable)
                {
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                }
                else if (errortype == ErrorType.UsersFile)
                {
                    MessageBox.Show("Our server caused a fatal error, please try again later.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            return result;
        }

        public ClassUserList? GetRemeberedUsers()
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

        public void WriteToRememberedUsers(ClassUser user)
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