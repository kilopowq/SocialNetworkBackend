using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interfaces;
using DAL;
using DAL.Models;
using Microsoft.Practices.Unity;
using NLog;
using NLog.Fluent;
using Xunit;

namespace BLL.Tests
{
    
    public class ConferenceActionsTest
    {
        private readonly IConferenceActions _action;

        public ConferenceActionsTest()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IRepository<UserDto>, UserRepositoryTest>();
            container.RegisterType<IRepository<ConferenceDto>, ConferenceRepositoryTest>();
            container.RegisterType<IRepository<MessageDto>, MessageRepositoryTest>();
            container.RegisterType<Logger>(new InjectionFactory(f => LogManager.GetCurrentClassLogger(typeof(Log))));
            _action = new ConferenceAction(container);
        }

        [Theory]
        [InlineData(5, 1, 2, "lpoki", true)]
        [InlineData(5, 1, 2, "lpoki", true)]
        [InlineData(5, 1, 0, "lpoki", false)]
        public void CreateConferenceTest(int id, int one, int second, string title, bool expected)
        {
            var conf = new ConferenceDto
            {
                Id = id,
                Members = new List<UserDto>(),
                Messages = new List<MessageDto>(),
                Title = title
            };

            var rep = new UserRepositoryTest();

            conf.Members.Add(rep.Get(one));
            conf.Members.Add(rep.Get(second));

            Assert.Equal(expected, _action.CreateConference(conf));
        }

        [Theory]
        [InlineData(2)]
        [InlineData(1)]
        public void GetConfTest(int id)
        {
            Assert.True(_action.GetConf(id).Id==id);
        }

        [Theory]
        [InlineData(2)]
        public void GetAllConfsTest(int count)
        {
            Assert.True(_action.GetAllConfs().Count() == count);
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 2)]
        public void GetMessageFromConfTest(int id, int count)
        {
            Assert.True(count == _action.GetMessageFromConf(id).Count());
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(1, 2, 0)]
        [InlineData(1, 0, 2)]
        public void GetNewMessagesFromConf(int id, int start, int count)
        {
            Assert.True(count == _action.GetNewMessagesFromConf(id, start).Count());
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 1)]
        public void GetUserConferences(int id, int count)
        {
            Assert.True(count == _action.GetUserConferences(id).Count());
        }

        [Theory]
        [InlineData(1, 2, true)]
        [InlineData(1, 1, true)]
        [InlineData(1, 3, false)]
        public void GetUsersFromConf(int id, int userId, bool expected)
        {
            bool test = _action.GetConf(id).Members.FirstOrDefault(p => p.Id == userId) != null;
            Assert.Equal(expected, test);
        }
    }

    public class ConferenceRepositoryTest : IRepository<ConferenceDto>
    {
        private readonly List<ConferenceDto> _db;

        public ConferenceRepositoryTest()
        {
            _db = new List<ConferenceDto>();

            var rep = new UserRepositoryTest();
            var repMess = new MessageRepositoryTest();

            _db.Add(new ConferenceDto
            {
                Id = 1,
                Members = new List<UserDto>(),
                Messages = new List<MessageDto>(),
                Photo = "fsdfsdf",
                Title = "kilopo"
            });
            
            _db[0].Members.Add(rep.Get(1));
            _db[0].Members.Add(rep.Get(2));
            _db[0].Messages.Add(repMess.Get(1));
            _db[0].Messages.Add(repMess.Get(2));

            _db.Add(new ConferenceDto
            {
                Id = 2,
                Members = new List<UserDto>(),
                Messages = new List<MessageDto>(),
                Photo = "qweqweqwe",
                Title = "kilopo11"
            });
            _db[1].Members.Add(rep.Get(2));
            _db[1].Members.Add(rep.Get(3));
            _db[1].Messages.Add(repMess.Get(4));
            _db[1].Messages.Add(repMess.Get(5));
        }

        public void Add(ConferenceDto item)
        {
            _db.Add(item);
        }

        public void Close()
        {
        }

        public void Delete(int id)
        {
            _db.Remove(_db.FirstOrDefault(p=>p.Id==id));
        }

        public ConferenceDto Get(int id)
        {
            return _db.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<ConferenceDto> GetList()
        {
            return _db;
        }

        public void Save()
        {
        }

        public void Update(ConferenceDto item)
        {
            if (item == null)
            {
                throw new NullReferenceException();
            }
           
            if (_db.FirstOrDefault(p => p.Id == item.Id) == null) return;
            _db.FirstOrDefault(p => p.Id == item.Id).Photo = item.Photo;
            _db.FirstOrDefault(p => p.Id == item.Id).Title = item.Title;
            _db.FirstOrDefault(p => p.Id == item.Id).Members = item.Members;
        }
    }
}
