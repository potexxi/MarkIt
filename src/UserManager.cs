using MarkIt.login_register;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Net.Http;
using Supabase.Gotrue;

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
            WindowMessageBox box;
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
                    box = new WindowMessageBox("Server offline", "Currently our server is offline, please try again later or continue as guest.");
                    box.ShowDialog();
                    Logger.logger.Error($"Server unreachable. {ex.Message}");
                    return ErrorType.ServerUnreachable;
                }
                Logger.logger.Debug("Invalid password or email.");
                return ErrorType.PasswordFalse;
            }
            catch(Exception ex)
            {
                loadingScreen.Visibility = Visibility.Hidden;
                box = new WindowMessageBox("Unknown Error", "Our server caused a fatal error, please try again later.");
                box.ShowDialog();
                Logger.logger.Fatal($"Unknown Error: {ex.Message}");
                return ErrorType.OK;
            }
        }

        public async Task<ErrorType> SignUpAndHandleErrors(string email, string password, Grid loadingScreen)
        {
            loadingScreen.Visibility = Visibility.Visible;
            WindowMessageBox box;
            try
            {
                await MainWindow.supabase.Auth.SignUp(email, password);
                MainWindow.currentUser = new ClassUser(email, password);
                Logger.logger.Information("Register succesfully to server.");
                loadingScreen.Visibility = Visibility.Hidden;
                return ErrorType.OK;
            }
            catch(Supabase.Gotrue.Exceptions.GotrueException ex)
            {
                loadingScreen.Visibility = Visibility.Hidden;
                if(ex.InnerException is HttpRequestException)
                {
                    box = new WindowMessageBox("Server offline", "Currently our server is offline, please try again later or continue as guest.");
                    box.ShowDialog();
                    Logger.logger.Error($"Server unreachable. {ex.Message}");
                    return ErrorType.ServerUnreachable;
                }
                else if(ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserBadPassword)
                {
                    box = new WindowMessageBox("Bad Password", "Bad Password! Password should be least 6 characters.");
                    box.ShowDialog();
                    Logger.logger.Debug("Bad Password.");
                    return ErrorType.BadPassword;
                }
                else if (ex.Reason is Supabase.Gotrue.Exceptions.FailureHint.Reason.UserBadEmailAddress)
                {
                    box = new WindowMessageBox("Bad Email", "Bad Email-Address! Check if your Email-Address is in valid format.");
                    box.ShowDialog();
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
                    box = new WindowMessageBox("Too many requests.", "Too many requests! Please try again later.");
                    box.ShowDialog();
                    return ErrorType.Requests;
                }
                loadingScreen.Visibility = Visibility.Hidden;
                box = new WindowMessageBox("Unknown Error", "Our server caused a fatal error, please try again later.");
                Logger.logger.Fatal($"Unknown Error: {ex.Message}");
                return ErrorType.Unknown;
            }
            catch (Exception ex)
            {
                loadingScreen.Visibility = Visibility.Hidden;
                box = new WindowMessageBox("Unknown Error", "Our server caused a fatal error, please try again later.");
                box.ShowDialog();
                Logger.logger.Fatal($"Unknown Error: {ex.Message}");
                return ErrorType.Unknown;
            }
        }

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

        public void RemoveFromRememberedUsers(Session session)
        {
            List<Supabase.Gotrue.Session>? sessions = GetRemeberedUsers();
            if (sessions == null)
            {
                sessions = new List<Session>();
            }
            else
            {
                // ChatGPT-Anfang
                // Prompt: davor: sessions.Remove(session) und dann STRG c STRG v : wieso wird sie session nicht richtig removed
                sessions.RemoveAll(s => s.AccessToken == session.AccessToken);
                // ChatGPT-Ende
            }
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            using (StreamWriter writer = new StreamWriter("sources/remembered.json"))
            {
                writer.Write(JsonSerializer.Serialize(sessions, options: options));
            }
        }
    }
}