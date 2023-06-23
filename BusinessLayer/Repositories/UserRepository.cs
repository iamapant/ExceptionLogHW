using BusinessLayer.Exceptions;
using BusinessLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessLayer.Repositories {
    public class UserRepository {
        #region Singleton
        private static readonly object _lock = new object();
        private static UserRepository instance;
        public static UserRepository Instance {
            get {
                if (instance == null) {
                    lock (_lock) {
                        if (instance == null) {
                            instance = new UserRepository();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        List<Users> Users { get; set; } = new List<Users>();

        /// <summary>
        /// Trả về số lượng tài khoản trong database
        /// </summary>
        public int Count { get { return Users.Count; } }

        /// <summary>
        /// Thêm 1 account vào database
        /// </summary>
        /// <param name="user">account muốn thêm vào database</param>
        public void AddUser(Users user) => Users.Add(user);

        /// <summary>
        /// Kiểm tra id đã được sử dụng trong database chưa
        /// </summary>
        /// <param name="id">Id để kiểm tra</param>
        /// <returns>True nếu ID đã được sử dụng</returns>
        public bool ContainsID(int id) => Users.Any(u => u.UserId == id);

        /// <summary>
        /// Kiểm tra tên người dùng đã được sử dụng trong database chưa
        /// </summary>
        /// <param name="username">Tên người dùng để kiểm tra</param>
        /// <returns>True nếu tên người dùng đã được sử dụng</returns>
        public bool ContainsUsername(string username) => Users.Any(u => u.Username == username);

        /// <summary>
        /// Lấy tên người dùng và mật khẩu, kiểm tra tên người dùng và mật khẩu có đúng không
        /// </summary>
        /// <param name="username">Tên người dùng nhập vào</param>
        /// <param name="password">Password người dùng nhập vào</param>
        /// <returns>True nếu tên người dùng và mật khẩu được tồn tại trong database</returns>
        public bool Login(string username, string password) => Users.Any(u => u.Username == username && u.Password == password);

        /// <summary>
        /// Kiểm tra tên người dùng và mật khẩu có đúng yêu cầu không
        /// </summary>
        /// <param name="username">Tên người dùng và password được nhập vào</param>
        /// <param name="isUsername">True nếu muốn kiểm tra tên người dùng, False nếu muốn kiểm tra password</param>
        /// <param name="e">List các lỗi</param>
        /// <returns>True nếu mà valid</returns>
        public bool IsValidCredentials(string username, bool isUsername, out ErrorType[] e) {
            List<ErrorType> errors = new List<ErrorType>();
            LoginCredentialProperties properties;
            using (StreamReader r = new StreamReader(isUsername ? "User/Username.json" : "User/Password.json")) {
                string json = r.ReadToEnd();
                properties = JsonConvert.DeserializeObject<LoginCredentialProperties>(json);
            }

            if (username.Length > properties.MaxLength) errors.Add(ErrorType.TooLong);
            if (username.Length < properties.MinLength) errors.Add(ErrorType.TooShort);

            if (properties.Numbers == -1 && username.Any(c => char.IsDigit(c))) errors.Add(ErrorType.NoNumbers);
            if (properties.Specials == -1 && username.Any(c => char.IsPunctuation(c) || char.IsSymbol(c))) errors.Add(ErrorType.NoSpecialChars);
            if (properties.Spaces == -1 && username.Any(c => char.IsWhiteSpace(c))) errors.Add(ErrorType.NoSpaces);

            if (properties.Letters == 1 && !username.Any(c => char.IsLetter(c))) errors.Add(ErrorType.RequiredLetters);
            if (properties.Numbers == 1 && !username.Any(c => char.IsNumber(c))) errors.Add(ErrorType.RequiredLetters);
            if (properties.Specials == 1 && !username.Any(c => char.IsPunctuation(c) || char.IsSymbol(c))) errors.Add(ErrorType.RequiredLetters);
            if (properties.Spaces == 1 && !username.Any(c => char.IsWhiteSpace(c))) errors.Add(ErrorType.RequiredLetters);


            e = errors.Distinct().ToArray();
            if (errors.Any()) return false;
            return true;
        }

        class LoginCredentialProperties {
            public int MinLength;
            public int MaxLength;
            public int Letters;
            public int Numbers;
            public int Specials;
            public int Spaces;
        }
    }
}
