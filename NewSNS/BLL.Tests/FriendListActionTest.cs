using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NLog.Fluent;
using Xunit;
using Assert = Xunit.Assert;


namespace BLL.Tests
{
    public class FriendListActionTest
    {
        private readonly IFriendsListActions _action;

        public FriendListActionTest()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IRepository<FriendDto>, FriendRepositoryTest>();
            container.RegisterType<IRepository<UserDto>, UserRepositoryTest>();
            container.RegisterType<Logger>(new InjectionFactory(f => LogManager.GetCurrentClassLogger(typeof(Log))));
            _action = new FriendsListAction(container);

        }

        [Theory]
        [InlineData(1, 2, false)]
        [InlineData(1, 3, false)]
        [InlineData(1, 4, true)]
        public void TestMethod1(int firstUserId, int secondUserId, bool expected)
        {
            Assert.Equal(expected, _action.AddFriend(firstUserId, secondUserId));
        }

        [Theory]
        [InlineData(2, 1, true)]
        [InlineData(2, 2, false)]
        [InlineData(1, 4, false)]
        public void DeleteFriendTest(int firstUserId, int secondUserId, bool expected)
        {
            Assert.Equal(expected, _action.DeleteFriend(firstUserId, secondUserId));
        }

        [Theory]
        [InlineData(2, 3, true)]
        [InlineData(1, 2, false)]
        [InlineData(1, 4, false)]
        public void FollowTest(int firstUserId, int secondUserId, bool expected)
        {
            Assert.Equal(expected, _action.Follow(firstUserId, secondUserId));
        }

        [Theory]
        [InlineData(3, 1, true)]
        [InlineData(1, 2, false)]
        [InlineData(1, 4, true)]
        public void UnfollowTest(int firstUserId, int secondUserId, bool expected)
        {
            Assert.Equal(expected, _action.Unfollow(firstUserId, secondUserId));
        }

        [Theory]
        [InlineData(1, 3, true)]
        [InlineData(4, 1, true)]
        [InlineData(2, 1, false)]
        public void GetFollowersListTest(int id, int followerId, bool expected)
        {
            var followers = _action.GetFollowersList(id);

            var user = new UserRepositoryTest().Get(followerId);
            user = followers.FirstOrDefault(p => p.Id == user.Id);
            
            bool test = (user != null && user.Id==followerId);
            

            Assert.Equal(test, expected);

        }

        [Theory]
        [InlineData(3, 1, true)]
        [InlineData(1, 4, true)]
        [InlineData(1, 2, false)]
        public void GetFollowsListTest(int id, int followId, bool expected)
        {
            var follows = _action.GetFollowsList(id);

            var user = new UserRepositoryTest().Get(followId);
            user = follows.FirstOrDefault(p => p.Id == user.Id);

            bool test = (user != null && user.Id == followId);


            Assert.Equal(test, expected);

        }

        [Theory]
        [InlineData(1, 3, false)]
        [InlineData(1, 2, true)]
        [InlineData(2, 1, true)]
        public void GetFriendsListTest(int id, int friendId, bool expected)
        {
            var friends = _action.GetFriendsList(id);

            var user = new UserRepositoryTest().Get(friendId);
            user = friends.FirstOrDefault(p => p.Id == user.Id);

            bool test = (user != null && user.Id == friendId);


            Assert.Equal(test, expected);
        }

    }

    public class FriendRepositoryTest : IRepository<FriendDto>
    {
        private readonly List<FriendDto> _db;

        public FriendRepositoryTest()
        {
            _db = new List<FriendDto>();
            _db.Add(new FriendDto
            {
                Id = 1,
                User1_ID = 1,
                User2_ID = 2,
                StatusFriendship = Status.Friend
            });

            _db.Add(new FriendDto
            {
                Id = 2,
                User1_ID = 3,
                User2_ID = 1,
                StatusFriendship = Status.Follow
            });

            _db.Add(new FriendDto
            {
                Id = 3,
                User1_ID = 1,
                User2_ID = 4,
                StatusFriendship = Status.Follow
            });
        }

        public void Add(FriendDto item)
        {
            _db.Add(item);
        }

        public void Close()
        {
        }

        public void Delete(int id)
        {
            _db.Remove(_db.FirstOrDefault(p => p.Id == id));
        }

        public FriendDto Get(int id)
        {
            return _db.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<FriendDto> GetList()
        {
            return _db;
        }

        public void Save()
        {
        }

        public void Update(FriendDto item)
        {
            if (item == null)
            {
                throw new NullReferenceException();
            }

            if (_db.FirstOrDefault(p => p.Id == item.Id) == null) return;
            _db.FirstOrDefault(p => p.Id == item.Id).User1_ID = item.User1_ID;
            _db.FirstOrDefault(p => p.Id == item.Id).User2_ID = item.User2_ID;
            _db.FirstOrDefault(p => p.Id == item.Id).StatusFriendship = item.StatusFriendship;
        }
    }

}
