using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    public class UserBo
    {
        #region Fields
        private int userId;
        private string username;
        private string email;
        private string password;
        private string passwordConfirm;
        private string firstName;
        private string lastName;
        private RoleBo role;
        private int points;
        #endregion

        #region Properties
        public int UserId { get => userId; set => userId = value; }
        [Required(ErrorMessage = "Username must not be empty!")]
        public string Username { get => username; set => username = value; }
        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Email must not be empty!")]
        public string Email { get => email; set => email = value; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password must not be empty!")]
        public string Password { get => password; set => password = value; }

        [Display(Name = "Repeat Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password),ErrorMessage = "Passwords must match!")]
        public string PasswordConfirm { get => passwordConfirm; set => passwordConfirm = value; }
        

        [Display(Name = "First name")]
        [Required(ErrorMessage = "Names required!")]
        public string FirstName { get => firstName; set => firstName = value; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "Surnames required!")]
        public string LastName { get => lastName; set => lastName = value; }
        public RoleBo Role { get => role; set => role = value; }
        public int Points { get => points; set => points = value < 0 ? 0 : value; }
        #endregion
    }
}
