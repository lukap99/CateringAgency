using CateringAgency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CateringAgency.Models.EntityFramework
{
    public class UserRepository
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
            user userM = new user
            {
                id = userBo.UserId,
                username = userBo.Username,
                email = userBo.Email,
                firstname = userBo.FirstName,
                lastname = userBo.LastName,
                role_id = userBo.Role.RoleId,
                points = userBo.Points
            };

            try
            {
                cateringEntities.users.Add(userM);
                cateringEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
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
            /*  username = userBo.Username,
                email = userBo.Email,
                firstname = userBo.FirstName,
                lastname = userBo.LastName,
                role_id = userBo.Role.RoleId,
                points = userBo.Points
             */
            userModel.username = userBo.Username;
            userModel.email = userBo.Email;
            userModel.firstname = userBo.FirstName;
            userModel.lastname = userBo.LastName;

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
    }
}