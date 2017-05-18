using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using DAL;
using Microsoft.Practices.Unity;
using NLog;

namespace BLL
{
    public class UserActions : IUserActions
    {
        public UserActions(IUnityContainer container)
        {
            _userRepository = container.Resolve<IRepository<UserDto>>();
            _logger = container.Resolve<Logger>();
        }

        private readonly IRepository<UserDto> _userRepository;

        private static Logger _logger;

        /// <summary>
        /// Set user status "deleted".</summary>
        public bool DeletePage(int userId)
        {
            var user = _userRepository.Get(userId);
            if (user == null || user.UserState == State.Deleted) return false;

            try
            {
                user.UserState = State.Deleted;
                _userRepository.Update(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Login user by login and password and return it.</summary>
        public UserDto Login(string login, string password)
        {
            var allUsers = _userRepository.GetList();
            var user = allUsers.FirstOrDefault(p => p.Login.Equals(login) && p.Password.Equals(password));
            return user;
        }

        /// <summary>
        /// Sets user status "active".</summary>
        public bool ReActivatePage(int userId)
        {
            var user = _userRepository.Get(userId);
            if (user == null || user.UserState == State.Active) return false;

            try
            {
                user.UserState = State.Active;
                _userRepository.Update(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Sets user status "blocked".</summary>
        public bool BlockPage(int userId)
        {
            var user = _userRepository.Get(userId);
            if (user == null || user.UserState == State.Blocked) return false;

            try
            {
                user.UserState = State.Blocked;
                _userRepository.Update(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }

        }

        /// <summary>
        /// Register new user in DB.</summary>
        public bool Register(UserDto user)
        {
            var allUsers = _userRepository.GetList();

            if (allUsers.Any(listedUser => listedUser.Login.Equals(user.Login) || listedUser.Email.Equals(user.Email)))
            {
                return false;
            }
            try
            {
                _userRepository.Add(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Returns list of all users.</summary>
        public IEnumerable<UserDto> GetAllUsers()
        {
            return _userRepository.GetList();
        }

        /// <summary>
        /// Returns user by id.</summary>
        public UserDto GetUser(int id)
        {
            return _userRepository.Get(id);
        }

        /// <summary>
        /// Update user.</summary>
        public bool UpdateUser(UserDto user)
        {
            try
            {
                _userRepository.Update(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Find and return users by his name.</summary>
        public IEnumerable<UserDto> FindUsersByName(string name)
        {
            return
                _userRepository.GetList()
                    .Where(
                        p => p.FirstName.ToLower().Equals(name.ToLower()) || p.LastName.ToLower().Equals(name.ToLower()));
        }

        /// <summary>
        /// Find and return users by his login.</summary>
        public UserDto FindUsersByLogin(string login)
        {
            return
                _userRepository.GetList().FirstOrDefault(p => p.Login.ToLower().Equals(login.ToLower()));
        }

        /// <summary>
        /// Save the avatar path on server in DB.</summary>
        public bool SaveAvatar(int id, string url)
        {
            var user = _userRepository.Get(id);
            if (user == null) return false;

            user.Avatar = url;
            try
            {
                _userRepository.Update(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Change user password.</summary>
        public bool ChangePassword(int userId, string oldPass, string newPass)
        {
            var user = _userRepository.Get(userId);

            if (user == null) return false;
            if (!user.Password.Equals(oldPass)) return false;

            user.Password = newPass;
            try
            {
                _userRepository.Update(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Sets user status "offline".</summary>
        public bool OffPage(int userId)
        {
            var user = _userRepository.Get(userId);
            if (user == null || user.UserState != State.Active) return false;

            try
            {
                user.UserState = State.Offline;
                _userRepository.Update(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Sets user status "active" if it been "offline".</summary>
        public bool OnPage(int userId)
        {
            var user = _userRepository.Get(userId);
            if (user == null || user.UserState != State.Offline) return false;

            try
            {
                user.UserState = State.Active;
                _userRepository.Update(user);
                _userRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }
    }
}
