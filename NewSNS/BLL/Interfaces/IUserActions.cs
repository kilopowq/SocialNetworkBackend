using DAL.Models;
using System.Collections.Generic;

namespace BLL
{
    public interface IUserActions
    {
        UserDto Login(string login, string password);

        bool Register(UserDto user);

        bool DeletePage(int userId);

        bool ReActivatePage(int userId);

        bool BlockPage(int userId);

        IEnumerable<UserDto> GetAllUsers();

        UserDto GetUser(int id);

        bool UpdateUser(UserDto user);

        IEnumerable<UserDto> FindUsersByName(string name);

        UserDto FindUsersByLogin(string name);

        bool SaveAvatar(int id, string url);

        bool ChangePassword(int userId, string oldPass, string newPass);

        bool OffPage(int userId);

        bool OnPage(int userId);

    }
}
