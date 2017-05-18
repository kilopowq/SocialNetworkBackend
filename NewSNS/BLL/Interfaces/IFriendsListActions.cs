using DAL.Models;
using System.Collections.Generic;

namespace BLL
{
    public interface IFriendsListActions
    {
        IEnumerable<UserDto> GetFriendsList(int userID);

        IEnumerable<UserDto> GetFollowersList(int userID);

        IEnumerable<UserDto> GetFollowsList(int userID);

        bool Follow(int firstUserID, int secondUserID);

        bool AddFriend(int firstUserID, int secondUserID);

        bool DeleteFriend(int firstUserID, int secondUserID);

        bool Unfollow(int firstUserID, int secondUserID);

    }
}
