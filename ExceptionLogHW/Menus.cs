using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogHW {
    internal static class Menus {
        /// <summary>
        /// Hiển thị menu custom. Bao gồm tiêu đề, phụ đề, list các options và 1 final option (Exit, Return. . . )
        /// </summary>
        /// <param name="title">Tiêu đề menu</param>
        /// <param name="options">List các options. Không yêu cầu</param>
        /// <param name="subtitle">Phụ đề. Không yêu cầu</param>
        /// <param name="finalOption">Final option. Không yêu cầu, không hiển thị nếu options trống</param>
        public static void DisplayMenu(string title, List<string> options = null, string? subtitle = null, string finalOption = "Main menu") {
            Console.WriteLine("========= " + title + " =========");
            if (subtitle != null) Console.WriteLine(subtitle);
            int i = 1;
            if (options != null) {
                foreach (string option in options) Console.WriteLine($"\t{i++}. {option}.");
                if (i > 1 && finalOption != null) Console.WriteLine($"\t{i}. {finalOption}.");
            }
        }

        /// <summary>
        /// Nhận lựa chọn của người dùng. Chỉ nhận số từ 1 - số lựa chọn
        /// </summary>
        /// <param name="numberOfOptions">Số lựa chọn</param>
        /// <param name="message">Prompt yêu cầu người dùng lựa chọn số</param>
        /// <returns>Lựa chọn của người dùng dưới dạng số</returns>
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

        /// <summary>
        /// Nhận dữ liệu từ bàn phím và trả về 1 string
        /// </summary>
        /// <param name="message">Prompt yêu cầu người dùng nhập dữ liệu</param>
        /// <returns>Dữ liệu người dùng nhập</returns>
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

        /// <summary>
        /// Hàm check string chỉ bao gồm chữ cái
        /// </summary>
        /// <param name="value">string để check</param>
        /// <returns>True nếu string chỉ bao gồm chữ cái</returns>
        private static bool IsLetters(string value) {
            foreach (var c in value) if (!char.IsLetter(c)) return false;
            return true;
        }

        /// <summary>
        /// Xóa 1 hoặc nhiều dòng từ dưới trở lên. Bắt đầu từ vị trị hiện tại của con trỏ
        /// </summary>
        /// <param name="line">Số dòng muốn xóa</param>
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

        /// <summary>
        /// Xóa toàn bộ màn hình console
        /// </summary>
        public static void ClearScreen() {
            Console.Clear();
            Console.WriteLine("\x1b[3J");
            Console.Clear();
        }
    }
}
