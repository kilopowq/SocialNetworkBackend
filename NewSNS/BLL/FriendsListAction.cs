using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using DAL;
using Microsoft.Practices.Unity;
using NLog;

namespace BLL
{
    public class FriendsListAction : IFriendsListActions
    {
        public FriendsListAction(IUnityContainer container)
        {
            _friendsRepository = container.Resolve<IRepository<FriendDto>>();
            _userRepository = container.Resolve<IRepository<UserDto>>();
            _logger = container.Resolve<Logger>();
        }

        private readonly IRepository<FriendDto> _friendsRepository;
        private readonly IRepository<UserDto> _userRepository;
        private static Logger _logger;

        /// <summary>
        /// Add friend from follows (first user = follower).</summary>
        public bool AddFriend(int firstUserID, int secondUserID)
        {
            var friend =
                _friendsRepository.GetList()
                    .FirstOrDefault(p => p.User1_ID == firstUserID && p.User2_ID == secondUserID);
            if (friend == null) return false;
            if (friend.StatusFriendship == Status.Friend) return false;
            friend.StatusFriendship = Status.Friend;
            try
            {
                _friendsRepository.Update(friend);
                _friendsRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Delete user from friends list.</summary>
        public bool DeleteFriend(int firstUserID, int secondUserID)
        {
            var friend =
                _friendsRepository.GetList()
                    .FirstOrDefault(p => (p.User1_ID == firstUserID && p.User2_ID == secondUserID)
                                         || (p.User2_ID == firstUserID && p.User1_ID == secondUserID));
            if (friend == null || friend.StatusFriendship!=Status.Friend) return false;
            try
            {
                _friendsRepository.Delete(friend.Id);
                _friendsRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Follow to user (first user = follower).</summary>
        public bool Follow(int firstUserID, int secondUserID)
        {
            if (
                _friendsRepository.GetList()
                    .FirstOrDefault(
                        p =>
                            (p.User1_ID == firstUserID && p.User2_ID == secondUserID) ||
                            (p.User2_ID == firstUserID && p.User1_ID == secondUserID)) != null)
            {
                return false;
            }
            try
            {
                _friendsRepository.Add(new FriendDto
                {
                    User1_ID = firstUserID,
                    User2_ID = secondUserID,
                    StatusFriendship = Status.Follow
                });
                _friendsRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Returns list of user followers.</summary>
        public IEnumerable<UserDto> GetFollowersList(int userID)
        {
            var followers =
                _friendsRepository.GetList()
                    .Where(p => p.User2_ID == userID && p.StatusFriendship.Equals(Status.Follow));
            var users = _userRepository.GetList();

            return
                (from follower in followers from user in users where user.Id == follower.User1_ID select user).ToList();
        }

        /// <summary>
        /// Returns list of user follows.</summary>
        public IEnumerable<UserDto> GetFollowsList(int userID)
        {
            var follows =
                _friendsRepository.GetList()
                    .Where(p => p.User1_ID == userID && p.StatusFriendship.Equals(Status.Follow));
            var users = _userRepository.GetList();

            return (from follower in follows from user in users where user.Id == follower.User2_ID select user).ToList();
        }

        /// <summary>
        /// Returns list of user friends.</summary>
        public IEnumerable<UserDto> GetFriendsList(int userID)
        {
            var allFriends = _friendsRepository.GetList().Where(p => p.User1_ID == userID || p.User2_ID == userID);
            var allUsers = _userRepository.GetList();

            return (from friend in allFriends
                where friend.StatusFriendship == Status.Friend
                select
                friend.User1_ID != userID
                    ? allUsers.FirstOrDefault(p => p.Id == friend.User1_ID)
                    : allUsers.FirstOrDefault(p => p.Id == friend.User2_ID)).ToList();
        }

        /// <summary>
        /// Unfollow from user (first user = follower).</summary>
        public bool Unfollow(int firstUserID, int secondUserID)
        {
            var friendToDelete =
                _friendsRepository.GetList()
                    .FirstOrDefault(p => p.User1_ID == firstUserID && p.User2_ID == secondUserID);
            if (friendToDelete == null || friendToDelete.StatusFriendship!=Status.Follow) return false;
            try
            {
                _friendsRepository.Delete(friendToDelete.Id);
                _friendsRepository.Save();
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
