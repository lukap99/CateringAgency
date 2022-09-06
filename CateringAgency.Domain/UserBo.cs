using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain
{
    class UserBo
    {
        #region Fields
        private int userId;
        private string username;
        private string email;
        private string firstName;
        private string lastName;
        private RoleBo role;
        private int points;
        #endregion

        #region Properties
        public int UserId { get => userId; set => userId = value; }
        public string Username { get => username; set => username = value; }
        public string Email { get => email; set => email = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public RoleBo Role { get => role; set => role = value; }
        public int Points { get => points; set => points = value < 0 ? 0 : value; }
        #endregion
    }
}
