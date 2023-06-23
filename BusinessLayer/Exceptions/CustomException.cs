using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exceptions {
    /// <summary>
    /// Sử dụng enum để tag lỗi gặp phải khi tạo và đăng nhập 
    /// </summary>
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

    /// <summary>
    /// Lớp cha của các Exceptions tự tạo
    /// </summary>
    public abstract class CustomException : Exception {
    }

    /// <summary>
    /// Exception thrown khi tên đăng nhập không được tìm thấy
    /// </summary>
    public class AccountNotExistException : CustomException {
        public override string Message => "Account is not available";
    }

    /// <summary>
    /// Exception thrown khi tên đăng nhập và mật khẩu không trùng khớp
    /// </summary>
    public class PasswordNotMatchException : CustomException {
        public override string Message => "Your password do not match";
    }

    /// <summary>
    /// Exception thrown khi tên đăng nhập và mật khẩu không theo định dạng yêu cầu
    /// </summary>
    public class AccountNotEnteredException : CustomException {
        ErrorType[] errors;
        public AccountNotEnteredException(params ErrorType[] e) {
            errors = e;
        }

        /// <summary>
        /// Thông báo lỗi. Hiển thị dưới dạng error message nếu chỉ có 1 error, và dưới dạng liệt kê lỗi nếu có từ 2 errors trở lên
        /// </summary>
        public override string Message {
            get {
                if (errors.Length == 1) return ToErrorMessage(errors[0]);
                else return "Account invalid. Issues: " + string.Join(", ", errors);
            }
        }

        /// <summary>
        /// Nhận enum ErrorType và trả ra 1 error mesage tương ứng
        /// </summary>
        /// <param name="errorType">Enum error type nhận được</param>
        /// <returns>Error message</returns>
        /// <exception cref="NotImplementedException"></exception>
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
