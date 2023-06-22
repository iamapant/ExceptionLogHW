﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogHW.User {
    public class Users {
        static int count = 1;
        private int userId;
        private string? username;
        private string? password;

        public int UserId {
            get { return userId; }
            set {
                if (UserManager.Instance.ContainsID(value)) throw new Exception("Contains id");
                userId = value;
            }
        }


        public string? Username {
            get { return username; }
            set {
                if (value == null) throw new Exception("Null username");
                if (UserManager.Instance.ContainsUsername(value)) throw new Exception("Contains username");
                username = value;
            }
        }

        public string? Password {
            get { return password; }
            set {
                if (value == null) throw new Exception("Null password");
                password = value;
            }
        }

        public Users() { }

        public Users(string _username, string _password) {
            int id = UserId = UserManager.Instance.Count - 1;
            while (true) {
                try {
                    UserId = id++;
                    break;
                }
                catch { }
            }

            Username = _username;
            Password = _password;
        }

        public Users(int id, string _username, string _password) {
            UserId = id;
            Username = _username;
            Password = _password;
        }
    }
}
