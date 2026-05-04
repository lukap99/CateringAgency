using CateringAgency.Domain;
using CateringAgency.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CateringAgency.Models.EntityFramework
{
    public class UserRepository : IUserRepository
    {
        private readonly CateringAgencyEntities cateringEntities;

        public UserRepository()
        {
            cateringEntities = new CateringAgencyEntities();
        }

        private UserBo UserMap(user userModel)
        {
            UserBo userBo = new UserBo
            {
                UserId = userModel.id,
                Username = userModel.username,
                Email = userModel.email,
                FirstName = userModel.firstname,
                LastName = userModel.lastname,
                Points = userModel.points,
                Role = new RoleBo
                {
                    RoleId = userModel.role.id,
                    RoleName = userModel.role.role_name
                }
            };
            return userBo;
        }

        public void Create(UserBo userBo)
        {
            if (Exists(userBo)) return;

            user userM = new user
            {
                id = userBo.UserId,
                username = userBo.Username,
                email = userBo.Email,
                firstname = userBo.FirstName,
                lastname = userBo.LastName,
                password = userBo.Password,
                role_id = userBo.Role.RoleId,
                points = userBo.Points
            };

            try
            {
                cateringEntities.users.Add(userM);
                cateringEntities.SaveChanges();
                Console.WriteLine("User succesfully added");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UserRepository.Create(UserBo userbo): " + ex.Message);
            }
        }

        public void CreateUser(UserBo userBo)
        {
            if (IsValid(userBo)) return;

            user userM = new user
            {
                id = userBo.UserId,
                username = userBo.Username,
                email = userBo.Email,
                firstname = userBo.FirstName,
                lastname = userBo.LastName,
                password = userBo.Password,
                role_id = 1,
                points = userBo.Points
            };

            try
            {
                cateringEntities.users.Add(userM);
                cateringEntities.SaveChanges();
                Console.WriteLine("User succesfully added");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UserRepository.Create(UserBo userbo): " + ex.Message);
            }
        }

        public void CreateManager(UserBo userBo)
        {
            if (Exists(userBo)) return;

            user userM = new user
            {
                id = userBo.UserId,
                username = userBo.Username,
                email = userBo.Email,
                firstname = userBo.FirstName,
                lastname = userBo.LastName,
                password = userBo.Password,
                role_id = 2,
                points = userBo.Points
            };

            try
            {
                cateringEntities.users.Add(userM);
                cateringEntities.SaveChanges();
                Console.WriteLine("Manager succesfully added");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in UserRepository.Create(UserBo userbo): " + ex.Message);
            }
        }

        public void Delete(UserBo userBo)
        {
            user userModel = cateringEntities.users.FirstOrDefault(t => t.id == userBo.UserId);

            try
            {
                cateringEntities.users.Remove(userModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void Delete(int userId)
        {
            user userModel = cateringEntities.users.FirstOrDefault(t => t.id == userId);

            try
            {
                cateringEntities.users.Remove(userModel);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public void Edit(UserBo userBo)
        {
            user userModel = cateringEntities.users.FirstOrDefault(t => t.id == userBo.UserId);

            userModel.username = userBo.Username;
            userModel.email = userBo.Email;
            userModel.firstname = userBo.FirstName;
            userModel.lastname = userBo.LastName;
            userModel.password = userBo.Password;

            try
            {
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void UpdatePointBalance(int userId, int pointDifference)
        {
            user userModel = cateringEntities.users.FirstOrDefault(t => t.id == userId);
            int oldPointBalance = userModel.points;

            if (userModel.points + pointDifference >= 0)
            {
                try
                {
                    userModel.points += pointDifference;
                    cateringEntities.SaveChanges();
                    Console.WriteLine("User #" + userId + "new points balance: " + userModel.points + " from: " + oldPointBalance);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in UserRepository.UpdatePointBalance(int userId, int pointDifference): " + ex.Message);
                }
            }
        }
        public IEnumerable<UserBo> GetAllUsers()
        {
            List<UserBo> userBoList = new List<UserBo>();
            foreach (user userItem in cateringEntities.users.Where(t=>t.role_id == 1))
            {
                userBoList.Add(UserMap(userItem));
            }
            return userBoList;
        }

        public IEnumerable<UserBo> GetAllManagers()
        {
            List<UserBo> userBoList = new List<UserBo>();
            foreach (user userItem in cateringEntities.users.Where(t => t.role_id == 2))
            {
                userBoList.Add(UserMap(userItem));
            }
            return userBoList;
        }

        public UserBo GetUser(int userId)
        {
            UserBo user = UserMap(cateringEntities.users.First(t => t.id == userId));
            return user;
        }
        public UserBo GetUser(string email)
        {
            UserBo user = UserMap(cateringEntities.users.First(t => t.email == email));
            return user;
        }
        public UserBo GetUser(UserBo userBo)
        {
            UserBo user = UserMap(cateringEntities.users.First(t => t.email == userBo.Email));
            return user;
        }

        // check for exception "System.InvalidOperationException: 'Sequence contains no elements'"
        public int GetUserPoints(int id)
        {
            return cateringEntities.users.First(t => t.id == id).points;
        }

        public bool Exists(UserBo userBo)
        {
            bool isValid = cateringEntities.users.Any(t => t.email == userBo.Email);
            return isValid;
        }
        public bool Exists(string email)
        {
            bool isValid = cateringEntities.users.Any(t => t.email == email);
            return isValid;
        }
        public bool Exists(int id)
        {
            bool isValid = cateringEntities.users.Any(t => t.id == id);
            return isValid;
        }
        public bool IsValid(UserBo userBo)
        {
            bool isValid = cateringEntities.users.Any(t => t.email == userBo.Email && t.password == userBo.Password);
            return isValid;
        }

        public string GetUserRole(string username)
        {
            user userModel = cateringEntities.users.FirstOrDefault(t => t.username == username);
            return userModel?.role.role_name;
        }
        public string GetUserRoleForEmail(string email)
        {
            user userModel = cateringEntities.users.FirstOrDefault(t => t.email == email);
            return userModel?.role.role_name;
        }
    }
}