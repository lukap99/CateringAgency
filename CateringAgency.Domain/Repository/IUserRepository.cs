using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringAgency.Domain.Repository
{
    public interface IUserRepository
    {
        void Create(UserBo userBo);
        void CreateUser(UserBo userBo);
        void CreateManager(UserBo userBo);
        void Delete(UserBo userBo);
        void Delete(int userId);
        void Edit(UserBo userBo);
        void UpdatePointBalance(int userId, int pointDifference);
        IEnumerable<UserBo> GetAllUsers();
        IEnumerable<UserBo> GetAllManagers();
        UserBo GetUser(int userId);
        UserBo GetUser(string email);
        UserBo GetUser(UserBo userBo);
        bool Exists(UserBo userBo);
        bool Exists(string email);
        bool Exists(int id);
        bool IsValid(UserBo userBo);
        string GetUserRole(string username);
        string GetUserRoleForEmail(string email);
    }
}
