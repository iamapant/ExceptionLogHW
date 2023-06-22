using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogHW.User
{
    internal class UserManager
    {
        #region Singleton
        private static readonly object _lock = new object();
        private static UserManager instance;
        public static UserManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_lock)
                    {
                        if (instance == null)
                        {
                            instance = new UserManager();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        List<Users> Users { get; set; } = new List<Users>();

        public int Count { get { return Users.Count; } }

        public void AddUser(Users user) => Users.Add(user);

        public bool ContainsID(int id) => Users.Any(u => u.UserId == id);

        public bool ContainsUsername(string username) => Users.Any(u => u.Username == username);

        public bool Login(string username, string password) => Users.Any(u => u.Username == username && u.Password == password);
        internal bool IsValidCredentials(string username, bool isUsername, out ErrorType[] e) {
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
