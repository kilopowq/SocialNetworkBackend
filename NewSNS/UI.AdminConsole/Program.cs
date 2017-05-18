using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Authentication;
using Authentication.File;
using DAL.Models;
using BLL;
using DAL;
using Microsoft.Practices.Unity;

namespace UI.AdminConsole
{
    class Program
    {
        #region ** Console Window Parameters

        private const string title = "NewSNS © Admin Console";

        private static string commandHighlightLine = ">   ";

        private static int minWidth = 80;
        private static int minHeight = 30;

        private static ConsoleColor defaultBackColor = Console.BackgroundColor;
        private static ConsoleColor defaultForeColor = Console.ForegroundColor;

        private const ConsoleColor commandPromptBackColor = ConsoleColor.DarkGreen;
        private const ConsoleColor commandPromptForeColor = ConsoleColor.Black;

        private static string xmlFilePath = @"users.xml";

        private static bool cancelPressed = false;

        private static string adminPassFileName = @"adminPass.txt";

        private static IEnumerable<UserDto> usersList;

        public static UnityContainer container;

        #endregion


        #region ** Main

        static void Main(string[] args)
        {
            PrepareConsoleWindow();
            PrepareContainer();

            if (!Authenticate(new AuthenticationFile()))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You are not authenticated. Contact administrator.");
                Console.ForegroundColor = defaultForeColor;
                Console.WriteLine("Exit. Press <Enter>");
                return;
            }
            else
            {
                Console.WriteLine("Welcome!\n");
            }

            string command;

            bool waitForCommand = true;

            while (waitForCommand)
            {
                CommandPrompt();

                command = Console.ReadLine();

                RestoreColors();

                waitForCommand = ProcessCommand(command);
            }
        }

        #endregion


        #region ** Methods

        private static bool ProcessCommand(string command)
        {
            // Ctrl + C or Ctrl + Break at beginning.
            if (command == null)
            {
                return true;
            }

            switch (command.ToLower())
            {
                case "":
                    EmptyLine();
                    break;
                case "cls":
                    Console.Clear();
                    break;
                case "exit":
                    return false;
                case "help":
                    ShowHelp();
                    break;
                case "list users":
                    ListUsers();
                    break;
                case "delete user":
                    DeleteUser();
                    break;
                case "add user":
                    AddUser();
                    break;
                case "update user":
                    UpdateUser();
                    break;
                default:
                    Console.WriteLine("\nUnknown command. Hit 'help' and press <Enter> for display program usage.\n");
                    break;
            }

            return true;
        }

        private static void ShowHelp()
        {
            Console.WriteLine("\n** " + title + " usage:");

            Console.WriteLine(" cls\t\t Clear screen.");
            Console.WriteLine(" exit\t\t Exit program.");
            Console.WriteLine(" help\t\t Displays current usage.");
            Console.WriteLine(" list users\t Get users list from service.");
            Console.WriteLine(" delete user\t Delete user by Id.");
            Console.WriteLine(" update user\t Update user.");

            Console.WriteLine();
        }

        private static void ListUsers()
        {
           
            var action = new UserActions(container);

            var user = action.GetUser(1);

            usersList = action.GetAllUsers();
                
            PrintUsers();
               
          
        }

        private static void PrintUsers()
        {
            Console.OutputEncoding = Encoding.GetEncoding(866);

            foreach (UserDto user in usersList)
            {
                Console.WriteLine("ID: " + user.Id);
                Console.WriteLine("Name: " + user.FirstName);
                Console.WriteLine("Birth Date: " + user.BirthDate);
                Console.WriteLine("Email: " + user.Email);
                Console.WriteLine("Login: " + user.Login);
                Console.WriteLine("State: " + user.UserState);
                Console.WriteLine("--------------");
            }

        }

        private static bool Authenticate(IAuthentication auth)
        {
            Console.Write("Enter login:");
            var login = Console.ReadLine();

            Console.Write("Enter password:");
            StringBuilder password = new StringBuilder();
            while(true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password.Length--;
                    }
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else
                {
                    password.Append(key.KeyChar);
                }
            }

#if DEBUG
            Console.WriteLine(password.ToString());
#else
            Console.WriteLine();
#endif

            return auth.IsAuthenticated(new AuthData() {
                Login = login,
                Password = password.ToString(),
                AuthFilePath = Path.GetFullPath(adminPassFileName)
            });
        }

        private static void DeleteUser()
        {
            var action = new UserActions(container);
            Console.WriteLine("Введи id: ");

            var idForDel = int.Parse(Console.ReadLine());
            action.DeletePage(idForDel);
            Console.WriteLine("Deleted");
        }

        private static void AddUser()
        {
            var action = new UserActions(container);
            var user = EnterUser();

            Console.WriteLine();

            action.Register(user);
            
            Console.WriteLine("Added");

        }

        private static void UpdateUser()
        {
            var action = new UserActions(container);
            var user = EnterUser();

            Console.WriteLine("Id:");
            user.Id = int.Parse(Console.ReadLine());

            Console.WriteLine();

            action.UpdateUser(user);

            Console.WriteLine("Updated");
        }

        private static UserDto EnterUser()
        {
            var user = new UserDto();

            Console.WriteLine("First Name:");
            user.FirstName = Console.ReadLine();

            Console.WriteLine("Last Name:");
            user.LastName = Console.ReadLine();

            Console.WriteLine("Login");
            user.Login = Console.ReadLine();

            Console.WriteLine("Password");
            user.Password = Console.ReadLine();

            Console.WriteLine("Birth date:");
            var dateTime = Console.ReadLine();
            try
            {
                user.BirthDate = Convert.ToDateTime(dateTime);
            }
            catch (Exception e)
            {
                Console.WriteLine("Wrong date");
                return null;
            }

            Console.WriteLine("City");
            user.City = Console.ReadLine();

            Console.WriteLine("Country");
            user.Country = Console.ReadLine();

            Console.WriteLine("email");
            user.Email = Console.ReadLine();
            user.UserState = 0;

            Console.WriteLine("Phone");
            user.Phone = Console.ReadLine();

            Console.WriteLine("Info");
            user.Info = Console.ReadLine();

            return user;
        }

        #endregion


        #region ** Command Window Setting Methods

        private static void PrepareConsoleWindow()
        {
            Console.OutputEncoding = Encoding.GetEncoding("utf-8");

            Console.CancelKeyPress += Console_CancelKeyPress;

            Console.Title = title + " - " + Console.Title;

            if (Console.WindowHeight < minHeight)
            {
                Console.WindowHeight = minHeight;
            }
            if (Console.WindowWidth < minWidth)
            {
                Console.WindowWidth = minWidth;
            }
        }

        private static void PrepareContainer()
        {
            container = new UnityContainer();
            container.RegisterType<IRepository<UserDto>, UserRepository>();
            container.RegisterType<IRepository<MessageDto>, MessageRepository>();
            container.RegisterType<IRepository<ConferenceDto>, ConferenceRepository>();
            container.RegisterType<IRepository<FriendDto>, FriendRepository>();
        }
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;

            cancelPressed = true;
        }

        private static void CommandPrompt()
        {
            Console.BackgroundColor = commandPromptBackColor;
            Console.ForegroundColor = commandPromptForeColor;

            int cursorTop = Console.CursorTop;

            Console.CursorLeft = 0;
            Console.Write(GetHighlightLine());

            Console.CursorTop = cursorTop;
            Console.CursorLeft = 2; // > _

            if (cancelPressed)
            {
                Console.WriteLine("Press 'exit' and hit <Enter> for exit.\n");
                Console.Write(GetHighlightLine());

                Console.CursorTop = cursorTop + 2;
                Console.CursorLeft = 2; // > _

                cancelPressed = false;
            }
        }

        private static void EmptyLine()
        {
            var currentBackColor = Console.BackgroundColor;
            var currentForeColor = Console.ForegroundColor;

            Console.CursorTop--;

            Console.Write(GetHighlightLine());
        }

        private static void RestoreColors()
        {
            Console.BackgroundColor = defaultBackColor;
            Console.ForegroundColor = defaultForeColor;
        }

        private static string GetHighlightLine()
        {
            if (commandHighlightLine.Length != Console.WindowWidth)
            {
                StringBuilder sb = new StringBuilder(">");
                for (int i = 0; i < Console.WindowWidth - 1; i++)
                {
                    sb.Append(" ");
                }
                commandHighlightLine = sb.ToString();
            }

            return commandHighlightLine;
        }

        #endregion
    }
}
