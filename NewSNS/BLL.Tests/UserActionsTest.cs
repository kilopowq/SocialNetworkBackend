using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using DAL.Models;
using Microsoft.Practices.Unity;
using NLog;
using NLog.Fluent;
using Xunit;

namespace BLL.Tests
{
    
    public class UserActionsTest
    {
        private readonly IUserActions _action;

        public UserActionsTest()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IRepository<UserDto>, UserRepositoryTest>();
            container.RegisterType<Logger>(new InjectionFactory(f => LogManager.GetCurrentClassLogger(typeof(Log))));
            _action = new UserActions(container);
        }

        [Theory]
        [InlineData(2, true)]
        [InlineData(1, false)]
        public void GetUserTest(int id, bool expected)
        {
            var user = _action.GetUser(id);
            Assert.Equal(expected, user.Id==2);
        }

        [Theory]
        [InlineData("kilopo", "qwerty", true, 1)]
        [InlineData("kilopo1", "qwerty1", true, 2)]
        [InlineData("kilopo1", "qwerty1", false, 1)]
        public void LoginTest(string login, string password, bool expected, int id)
        {
            var user = _action.Login(login, password);
            Assert.Equal(expected, user.Id == id);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(3, true)]
        public void ReactivatePageTest(int id, bool expected)
        {
            Assert.Equal(_action.ReActivatePage(id), expected);
        }

        [Theory]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        public void OffPageTest(int id, bool expected)
        {
            Assert.Equal(_action.OffPage(id), expected);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        public void DeletePageTest(int id, bool expected)
        {
            Assert.Equal(_action.DeletePage(id), expected);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false)]
        [InlineData(3, false)]
        public void OnPageTest(int id, bool expected)
        {
            Assert.Equal(_action.OnPage(id), expected);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(4, false)]
        public void BlockPageTest(int id, bool expected)
        {
            Assert.Equal(_action.BlockPage(id), expected);
        }

        [Theory]
        [InlineData(1,"qwerty","qwqweerty", true)]
        [InlineData(1, "qwerty3", "qwqweerty", false)]
        [InlineData(2, "qwerty1", "qwqweerty", true)]
        [InlineData(2, "qwertyr", "qwqweerty", false)]
        public void ChangePasswordTest(int userId, string oldPass, string newPass, bool expected)
        {
            Assert.Equal(expected, _action.ChangePassword(userId, oldPass, newPass));
        }

        [Theory]
        [InlineData("kilopo", 1, true)]
        [InlineData("kilopo1", 2, true)]
        [InlineData("kilopo1", 1, false)]
        [InlineData("kilopo", 3, false)]
        public void FindUsersByLoginTest(string login, int id, bool expected)
        {
            var user = _action.FindUsersByLogin(login);
            Assert.Equal(expected, user.Id==id);
        }

        [Theory]
        [InlineData("Marian", 1, true)]
        [InlineData("maRiaN", 1, true)]
        [InlineData("Marian1", 1, false)]
        [InlineData("Marian2", 3, true)]
        public void FindUsersByNameTest(string name, int id, bool expected)
        {
            var users = _action.FindUsersByName(name);
            var testUser = new UserDto();
            foreach (var user in users)
            {
                if (user.Id == id)
                {
                    testUser = user;
                }
            }

            Assert.Equal(expected, testUser.Id == id);
        }

        [Theory]
        [InlineData(5, true)]
        [InlineData(6, false)]
        public void RegisterTest(int id, bool expected)
        {
            var user = new UserDto
            {
                Id = 5
            };
            _action.Register(user);
            Assert.Equal(expected, _action.GetAllUsers().LastOrDefault().Id==id);
        }

        [Theory]
        [InlineData(4)]
        public void GetAllTest(int count)
        {
            Assert.True(_action.GetAllUsers().Count() == count);
        }
    }



    public class UserRepositoryTest : IRepository<UserDto>
    {
        private List<UserDto> _db;

        public UserRepositoryTest()
        {
            _db = new List<UserDto>();
            _db.Add(new UserDto
            {
                Id = 1,
                FirstName = "Marian",
                LastName = "Brynetskyy",
                Login = "kilopo",
                Password = "qwerty",
                BirthDate = DateTime.Now,
                City = "Lviv",
                Country = "Ukraine",
                Email = "kilopo@ex.ua",
                UserState = State.Offline
            });

            _db.Add(new UserDto
            {
                Id = 2,
                FirstName = "Marian1",
                LastName = "Brynetskyy1",
                Login = "kilopo1",
                Password = "qwerty1",
                BirthDate = DateTime.Now,
                City = "Lviv1",
                Country = "Ukraine1",
                Email = "kilopo@ex.ua1",
                UserState = State.Active
            });

            _db.Add(new UserDto
            {
                Id = 3,
                FirstName = "Marian2",
                LastName = "Brynetskyy2",
                Login = "kilopo2",
                Password = "qwerty2",
                BirthDate = DateTime.Now,
                City = "Lviv2",
                Country = "Ukraine2",
                Email = "kilopo@ex.ua2",
                UserState = State.Deleted
            });

            _db.Add(new UserDto
            {
                Id = 4,
                FirstName = "Marian2",
                LastName = "Brynetskyy2",
                Login = "kilopo2",
                Password = "qwerty2",
                BirthDate = DateTime.Now,
                City = "Lviv2",
                Country = "Ukraine2",
                Email = "kilopo@ex.ua2",
                UserState = State.Blocked
            });
        }

        public IEnumerable<UserDto> GetList()
        {
            return _db;
        }

        public UserDto Get(int id)
        {
            return _db.FirstOrDefault(p => p.Id == id);
        }

        public void Add(UserDto item)
        {
            try
            {
                _db.Add(item);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public void Update(UserDto item)
        {
           
            if (item == null)
            {
                throw new NullReferenceException();
            }
            if (_db.FirstOrDefault(p=>p.Id==item.Id) == null) return;

            _db.FirstOrDefault(p => p.Id == item.Id).FirstName = item.FirstName;
            _db.FirstOrDefault(p=>p.Id==item.Id).LastName = item.LastName;
            _db.FirstOrDefault(p=>p.Id==item.Id).Login = item.Login;
            _db.FirstOrDefault(p=>p.Id==item.Id).Password = item.Password;
            _db.FirstOrDefault(p=>p.Id==item.Id).Info = item.Info;
            _db.FirstOrDefault(p=>p.Id==item.Id).Phone = item.Phone;
            _db.FirstOrDefault(p=>p.Id==item.Id).UserState = item.UserState;
            _db.FirstOrDefault(p=>p.Id==item.Id).Avatar = item.Avatar;
        }

        public void Delete(int id)
        {
            _db.Remove(_db.FirstOrDefault(p=>p.Id==id));
        }

        public void Save()
        {
        }

        public void Close()
        {
        }
    }
}


