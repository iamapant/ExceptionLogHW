using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogHW {
    internal static class Menus {
        public static void DisplayMenu(string title, string? subtitle = null, List<string> options = null, string finalOption = "Main menu") {
            Console.WriteLine("========= " + title + " =========");
            if (subtitle != null) Console.WriteLine(subtitle);
            int i = 1;
            if (options != null) {
                foreach (string option in options) Console.WriteLine($"\t{i++}. {option}.");
                if (i > 1 && finalOption != null) Console.WriteLine($"\t{i}. {finalOption}.");
            }
        }

        public static int GetOption(int numberOfOptions, string message = "Enter Menu Option Number: ") {
            int count = 1;
            while (true) {
                if (count == 2) Console.WriteLine("Please select option 1-" + numberOfOptions);
                Console.Write(message);
                string? get = Console.ReadLine();
                if (get.Length > 0 && int.TryParse(get, out var option)) {
                    if (numberOfOptions > 0) {
                        if (option > 0 && option <= numberOfOptions) {
                            if (count > 1) {
                                ClearCurrentConsoleLine(2);
                                Console.WriteLine(message + option);
                            }
                            return option;
                        }
                    }
                    else {
                        if (option > 0) {
                            if (count > 1) {
                                ClearCurrentConsoleLine(2);
                                Console.WriteLine(message + option);
                            }
                            return option;
                        }

                    }
                }
                count++;
                ClearCurrentConsoleLine();
            }
        }

        public static string GetString(string message) {
            int count = 1;
            while (true) {
                if (count == 2) Console.WriteLine("Please enter a string!");
                Console.Write(message);
                string? get = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(get) && !string.IsNullOrEmpty(get)) {
                    if (count > 1) {
                        ClearCurrentConsoleLine(2);
                        Console.WriteLine(message + get);
                    }
                    return get;
                }
            Skip:
                count++;
                ClearCurrentConsoleLine();
            }
        }
        private static bool IsLetters(string value) {
            foreach (var c in value) if (!char.IsLetter(c)) return false;
            return true;
        }

        public static void ClearCurrentConsoleLine(int line = 1) {
            while (line > 0) {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                int currentLineCursor = Console.CursorTop;
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.BufferWidth));
                Console.SetCursorPosition(0, currentLineCursor);
                line--;
            }
        }

        public static void ClearScreen() {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.Clear();
        }
    }
}
