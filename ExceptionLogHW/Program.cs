using ExceptionLogHW.User;
using log4net;
using log4net.Config;
using System.Reflection;

namespace ExceptionLogHW {
    internal class Program {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static List<Users> users = new List<Users> {
            new Users("user1", "password1"),
            new Users("user2", "password1"),
            new Users("user3", "password1"),
        };
        static void Main(string[] args) {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            AddUsers(users.ToArray());
            MainMenu();
        }

        private static void MainMenu() {
            string? user = null;
            List<string> options = new List<string> {
                "Login", "Add new User", "Logout"
            };
            while (true) {
                if (user != null) {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Logged in as: " + user);
                    Console.ForegroundColor = color;
                }
                Menus.DisplayMenu("Demo Exceptions and Loggings", null, options, "Exit");
                int get = Menus.GetOption(options.Count + 1);
                Menus.ClearScreen();
                switch (get) {
                    case 1:
                        LoginMenu(out user);
                        break;
                    case 2:
                        AddUserMenu();
                        break;
                    case 3:
                        if (user != null) Console.WriteLine($"Goodbye, {user}!");
                        else Console.WriteLine("No user!");
                        user = null;
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                Console.WriteLine("\nPress any keys to continue. . . ");
                Console.ReadKey();

                Menus.ClearScreen();
            }
        }

        private static bool LoginMenu(out string user, int retryMax = 3) {
            int retry = retryMax;
            var users = UserManager.Instance;
            string? username = null;
            string? password = null;
            while (true) {
                if (retry == 0) {
                    Menus.ClearScreen();
                    Console.WriteLine("Too many tries. Returning to Main menu.");
                    //Thread.Sleep(2000);
                    user = null;
                    return false;
                }
                List<ErrorType> errors = new List<ErrorType>();
                try {
                    Menus.DisplayMenu("Login menu");
                    Console.Write("Username: ");
                    if (username == null) {
                        string get = GetCredentials(true);
                        if (!users.ContainsUsername(get)) throw new AccountNotExistException();

                        username = get;
                        Menus.ClearScreen();
                        retry = retryMax;
                        continue;
                    }
                    else Console.Write(username + "\n");

                    Console.Write("Password: ");
                    if (password == null) {
                        string? get = GetCredentials(false);
                        if (!users.Login(username, get)) throw new PasswordNotMatchException();

                        password = get;
                        Menus.ClearScreen();
                        retry = retryMax;
                        continue;
                    }
                    else Console.Write(password + "\n");

                    Menus.ClearScreen();
                    Console.WriteLine("Login successfully!");
                    user = username;
                    return true;
                }
                catch (CustomException e) {
                    retry--;
                    Menus.ClearScreen();
                    log.Error(e.Message, e);

                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = color;
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }

        private static void AddUserMenu() {
            var users = UserManager.Instance;
            string? username = null;
            string? password = null;
            while (true) {
                try {
                    Menus.DisplayMenu("Add User");
                    Console.Write("Username: ");
                    if (username == null) {
                        username = GetUserName();
                        Menus.ClearScreen();
                        continue;
                    }
                    else Console.Write(username + "\n");
                    Console.Write("Enter password: ");
                    if (password == null) {
                        password = GetPassword();
                        Menus.ClearScreen();
                        continue;
                    }
                    else Console.Write(password + "\n");


                    break;
                }
                catch (CustomException e) {
                    Menus.ClearScreen();
                    log.Error(e.Message,e);

                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ForegroundColor = color;
                }
                catch (Exception e) {
                    Menus.ClearScreen();
                    Console.WriteLine(e);
                }
            }
            Console.WriteLine("New User Added!");
            AddUsers(new Users(username, password));

            string GetUserName() {
                List<ErrorType> errors = new List<ErrorType>();
                string _username = GetCredentials(true);
                if (users.ContainsUsername(_username)) errors.Add(ErrorType.UsernameExisted);
                if (!users.IsValidCredentials(_username, true, out var e)) errors.AddRange(e);

                if (errors.Count > 0) throw new AccountNotEnteredException(errors.ToArray());
                return _username;
            }
            string GetPassword() {
                List<ErrorType> errors = new List<ErrorType>();
                string _password = GetCredentials(true);
                if (!users.IsValidCredentials(_password, false, out var e)) errors.AddRange(e);

                if (errors.Count > 0) throw new AccountNotEnteredException(errors.ToArray());
                return _password;
            }
        }

        private static void AddUsers(params Users[] users) {
            foreach (Users user in users) {
                UserManager.Instance.AddUser(user);
            }
        }

        private static string GetCredentials(bool isUsername) {
            var users = UserManager.Instance;
            string? get = Console.ReadLine();
            if (get == null || get.Length == 0) {
                if (isUsername) throw new AccountNotEnteredException(ErrorType.UsernameEmpty);
                else throw new AccountNotEnteredException(ErrorType.PasswordEmpty);
            }

            return get;
        }

    }
}