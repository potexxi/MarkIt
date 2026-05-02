using MarkIt.login_register;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Supabase;
using System.Net.Sockets;
using System.Net.Http;

namespace MarkIt
{
    public class UserManager
    {
        public UserManager() { }

        public enum ErrorType
        {
            Unknown,
            OK,
            ServerUnreachable,
            PasswordFalse,
            BadPassword,
            BadEmail,
            EmailExists,
            Requests
        }
        public async Task<ErrorType> SignInAndHandleErrors(string email, string password, Grid loadingScreen)
        {
            loadingScreen.Visibility = Visibility.Visible;
            try
            {
                await MainWindow.supabase.Auth.SignIn(email, password);
                Logger.logger.Information("Login succesfully to server.");
                loadingScreen.Visibility = Visibility.Hidden;
                return ErrorType.OK;
            }
            catch (Supabase.Gotrue.Exceptions.GotrueException ex)
            {
                loadingScreen.Visibility = Visibility.Hidden;
                if (ex.InnerException is HttpRequestException)
                {
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                    Logger.logger.Error($"Server unreachable. {ex.Message}");
                    return ErrorType.ServerUnreachable;
                }
                Logger.logger.Debug("Invalid password or email.");
                return ErrorType.PasswordFalse;
            }
            catch(Exception ex)
            {
                loadingScreen.Visibility = Visibility.Hidden;
                MessageBox.Show("Our server caused a fatal error, please try again later.", "Unknown Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.logger.Fatal($"Unknown Error: {ex.Message}");
                return ErrorType.OK;
            }
        }

        public async Task<ErrorType> SignUpAndHandleErrors(string email, string password, Grid loadingScreen)
        {
            loadingScreen.Visibility = Visibility.Visible;
            try
            {
                await MainWindow.supabase.Auth.SignUp(email, password);
                MainWindow.currentUser = new ClassUser(0, email, password);
                Logger.logger.Information("Register succesfully to server.");
                loadingScreen.Visibility = Visibility.Hidden;
                return ErrorType.OK;
            }
            catch(Supabase.Gotrue.Exceptions.GotrueException ex)
            {
                loadingScreen.Visibility = Visibility.Hidden;
                if(ex.InnerException is HttpRequestException)
                {
                    MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
                    Logger.logger.Error($"Server unreachable. {ex.Message}");
                    return ErrorType.ServerUnreachable;
                }
                else if(ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserBadPassword)
                {
                    MessageBox.Show("Bad Password! Password should be least 6 characters.", "Bad Password", MessageBoxButton.OK, MessageBoxImage.Information);
                    Logger.logger.Debug("Bad Password.");
                    return ErrorType.BadPassword;
                }
                else if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserBadEmailAddress)
                {
                    MessageBox.Show("Bad Email-Address! Check if your Email-Address is in valid format.", "Bad Email", MessageBoxButton.OK, MessageBoxImage.Information);
                    Logger.logger.Debug($"Bad Email: {email}");
                    return ErrorType.BadEmail;
                }
                else if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserAlreadyRegistered)
                {
                    Logger.logger.Debug($"Email already exists: {email}");
                    return ErrorType.EmailExists;
                }
                else if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserTooManyRequests)
                {
                    Logger.logger.Debug($"Too many requests {email}");
                    MessageBox.Show("Too many requests! Please try again later.", "Too many requests.", MessageBoxButton.OK, MessageBoxImage.Information);
                    return ErrorType.Requests;
                }
                loadingScreen.Visibility = Visibility.Hidden;
                MessageBox.Show("Our server caused a fatal error, please try again later.", "Unknown Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.logger.Fatal($"Unknown Error: {ex.Message}");
                return ErrorType.Unknown;
            }
            catch (Exception ex)
            {
                loadingScreen.Visibility = Visibility.Hidden;
                MessageBox.Show("Our server caused a fatal error, please try again later.", "Unknown Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.logger.Fatal($"Unknown Error: {ex.Message}");
                return ErrorType.Unknown;
            }
        }

        //public async Task<ClassUserList?> GetUsersFromServerAndHandleErrors(Grid loadingScreen)
        //{
        //    // code inspired by StackOverflow/Autocompletion
        //    loadingScreen.Visibility = Visibility.Visible;
        //    var (userList,errortype)  = await Task.Run(() =>
        //    {
        //        if(MainWindow.ServerManager.ConnectionInfo == null)
        //        {
        //            MainWindow.ServerManager.CreatePrivateKeyAuth();
        //            if(MainWindow.ServerManager.ConnectionInfo == null)
        //            {
        //                return (null, ErrorType.PrivKey);

        //            }
        //        }
        //        try
        //        {
        //            using (SftpClient client = new SftpClient(MainWindow.ServerManager.ConnectionInfo))
        //            {
        //                client.Connect();
        //                using (var stream = client.OpenRead("/files/users.json"))
        //                using (StreamReader reader = new StreamReader(stream))
        //                {
        //                    string content = reader.ReadToEnd();
        //                    ClassUserList? userList = JsonSerializer.Deserialize<ClassUserList>(content);
        //                    Logger.logger.Debug("Successfully got users from server.");
        //                    client.Disconnect();
        //                    return (userList, ErrorType.OK);
        //                }
        //            }
        //        }
        //        // Exception for client.connect()
        //        catch (System.Net.Sockets.SocketException)
        //        {
        //            Logger.logger.Error("Server unreachable.");
        //            return (null, ErrorType.ServerUnreachable);
        //        }
        //        catch (Exception e)
        //        {
        //            Logger.logger.Fatal($"No file \"users.json\" or {e.Message}");
        //            return (null, ErrorType.UsersFile);
        //        }
        //    });
        //    loadingScreen.Visibility = Visibility.Hidden;
        //    if(userList == null)
        //    {
        //        if(errortype == ErrorType.ServerUnreachable || errortype == ErrorType.ServerUnreachable)
        //        {
        //            MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
        //        }
        //        else if(errortype == ErrorType.UsersFile)
        //        {
        //            MessageBox.Show("Our server caused a fatal error, please try again later.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    return userList;
        //}

        //public async Task<bool> WriteUsersToServer(ClassUserList userList, Grid loadingScreen)
        //{
        //    // code inspired by StackOverflow/Autocompletion
        //    loadingScreen.Visibility= Visibility.Visible;
        //    var (result, errortype) = await Task.Run(() =>
        //    {
        //        ConnectionInfo connection;
        //        try
        //        {
        //            PrivateKeyFile privateKey = new PrivateKeyFile(ServerSettings.KeyFilePath);
        //            PrivateKeyAuthenticationMethod privateKeyAuth = new PrivateKeyAuthenticationMethod(ServerSettings.Username, privateKey);
        //            connection = new ConnectionInfo(ServerSettings.PublicIp, ServerSettings.Port, ServerSettings.Username, privateKeyAuth);
        //            Logger.logger.Debug("Successfully created connection with private key.");
        //        }
        //        catch
        //        {
        //            Logger.logger.Error("Privat key authentication or server unreachable.");
        //            return (false, ErrorType.PrivKey);
        //        }
        //        try
        //        {
        //            using (SftpClient client = new SftpClient(connection))
        //            {
        //                client.Connect();
        //                JsonSerializerOptions jsonoptions = new JsonSerializerOptions
        //                {
        //                    WriteIndented = true
        //                };
        //                using (var stream = client.OpenWrite("/files/users.json"))
        //                using (StreamWriter writer = new StreamWriter(stream))
        //                {
        //                    string json = JsonSerializer.Serialize(userList, options: jsonoptions);
        //                    writer.Write(json);
        //                    Logger.logger.Debug("Successfully wrote users to server.");
        //                }
        //                client.Disconnect();
        //                return (true, ErrorType.OK);
        //            }
        //        }
        //        // Exception for client.connect()
        //        catch (System.Net.Sockets.SocketException)
        //        {
        //            Logger.logger.Error("Server unreachable.");
        //            return (false, ErrorType.ServerUnreachable);
        //        }
        //        catch(Exception e)
        //        {
        //            Logger.logger.Fatal($"No file \"users.json\" or {e.Message}");
        //            return (false, ErrorType.UsersFile);
        //        }
        //    });
        //    loadingScreen.Visibility = Visibility.Hidden;
        //    if (result == false)
        //    {
        //        if (errortype == ErrorType.ServerUnreachable || errortype == ErrorType.ServerUnreachable)
        //        {
        //            MessageBox.Show("Currently our server is offline, please try again later or continue as guest.", "Server offline", MessageBoxButton.OK, MessageBoxImage.Question);
        //        }
        //        else if (errortype == ErrorType.UsersFile)
        //        {
        //            MessageBox.Show("Our server caused a fatal error, please try again later.", "File not found", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    return result;
        //}

        public List<Supabase.Gotrue.Session>? GetRemeberedUsers()
        {
            if (File.Exists("sources/remembered.json"))
            {
                try
                {
                    using (StreamReader reader = new StreamReader("sources/remembered.json"))
                    {
                        return JsonSerializer.Deserialize<List<Supabase.Gotrue.Session>>(reader.ReadToEnd());
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

        public void WriteToRememberedUsers(Supabase.Gotrue.Session currentSession)
        {
            List<Supabase.Gotrue.Session>? sessions = GetRemeberedUsers();
            if (sessions == null)
            {
                sessions = new List<Supabase.Gotrue.Session>();
            }
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            sessions.Add(currentSession);
            using (StreamWriter writer = new StreamWriter("sources/remembered.json"))
            {
                writer.Write(JsonSerializer.Serialize(sessions, options: options));
            }
        }
    }
}