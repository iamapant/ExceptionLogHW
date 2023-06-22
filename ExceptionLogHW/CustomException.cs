using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionLogHW {
    public enum ErrorType {
        UsernameEmpty,
        PasswordEmpty,
        LettersOnly,
        NoNumbers,
        NoSpecialChars,
        NoSpaces,
        RequiredLetters,
        RequiredNumbers,
        RequiredSpecials,
        UsernameNotFound,
        PasswordIncorrect,
        TooLong,
        TooShort,
        UsernameExisted,
    }
    public class CustomException : Exception {
    }
    public class AccountNotExistException : CustomException {
        public override string Message => "Account is not available";
    }
    public class PasswordNotMatchException : CustomException {
        public override string Message => "Your password do not match";
    }
    public class AccountNotEnteredException : CustomException {
        ErrorType[] errors;
        public AccountNotEnteredException(params ErrorType[] e) {
            errors = e;
        }
        public override string Message {
            get {
                if (errors.Length == 1) return ToErrorMessage(errors[0]);
                else return "Account invalid. Issues: " + string.Join(", ", errors);
            }
        }

        private static string ToErrorMessage(ErrorType errorType) {
            string message;
            switch (errorType) {
                case ErrorType.UsernameEmpty:
                    message = "Username must not be empty.";
                    break;
                case ErrorType.PasswordEmpty:
                    message = "Password must not be empty.";
                    break;
                case ErrorType.LettersOnly:
                    message = "Username or password must only consist of letters.";
                    break;
                case ErrorType.UsernameNotFound:
                    message = "Cannot find username.";
                    break;
                case ErrorType.PasswordIncorrect:
                    message = "Password incorrect.";
                    break;
                case ErrorType.TooLong:
                    message = "Username or password is too long.";
                    break;
                case ErrorType.TooShort:
                    message = "Username or password is too short.";
                    break;
                case ErrorType.NoNumbers:
                    message = "Username or password must not contains any numbers.";
                    break;
                case ErrorType.NoSpecialChars:
                    message = "Username or password must not contains any special characters.";
                    break;
                case ErrorType.UsernameExisted:
                    message = "Username's already existed.";
                    break;
                case ErrorType.NoSpaces:
                    message = "Username or password must not contains any spaces.";
                    break;
                case ErrorType.RequiredLetters:
                    message = "Username or password must contains letters.";
                    break;
                case ErrorType.RequiredNumbers:
                    message = "Username or password must contains numbers.";
                    break;
                case ErrorType.RequiredSpecials:
                    message = "Username or password must contains special characters.";
                    break;
                default:
                    throw new NotImplementedException();
            }
            return message;
        }
    }

}
