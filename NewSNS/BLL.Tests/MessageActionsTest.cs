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
    public class MessageActionsTest
    {
        private readonly IMessagesActions _action;

        public MessageActionsTest()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<IRepository<MessageDto>, MessageRepositoryTest>();
            container.RegisterType<Logger>(new InjectionFactory(f => LogManager.GetCurrentClassLogger(typeof(Log))));
            _action = new MessagesActions(container);

        }

        [Theory]
        [InlineData("MessageText", 2, 3, null, true)]
        [InlineData("MessageText1", 1, 3, "dfdfd", false)]
        [InlineData("MessageTexte", 0, 3, null, false)]
        public void SendMessageTest(string text, int confId, int userId, string location, bool expected)
        {
            var message = new MessageDto
            {
                Id = 4,
                ConferenceId = confId,
                Date = DateTime.Now,
                UserId = userId,
                Location = location
            };
            Assert.Equal(expected, _action.SendMessage(message));
        }

        [Theory]
        [InlineData("MessageText", 2, 3, null, false)]
        [InlineData("MessageText1", 1, 3, "dfdfd", false)]
        [InlineData("MessageTexte", 0, 3, null, false)]
        [InlineData("MessageTexte", 0, 3, "lviv", true)]
        public void PostMessageTest(string text, int confId, int userId, string location, bool expected)
        {
            var message = new MessageDto
            {
                Id = 4,
                ConferenceId = confId,
                Date = DateTime.Now,
                UserId = userId,
                Location = location
            };
            Assert.Equal(expected, _action.PostMessage(message));
        }

        [Theory]
        [InlineData("MessageText", 2, 3, null, true)]
        [InlineData("MessageText1", 1, 3, "dfdfd", true)]
        [InlineData("MessageTexte", 0, 3, null, true)]
        [InlineData("MessageTexte", 0, 3, "lviv", true)]
        public void CorrectMessageTest(string text, int confId, int userId, string location, bool expected)
        {
            var message = new MessageDto
            {
                Id = 4,
                ConferenceId = confId,
                Date = DateTime.Now,
                UserId = userId,
                Location = location
            };
            Assert.Equal(expected, _action.CorrectMessage(message));
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        public void GetMessageTest(int id, bool expected)
        {
            Assert.Equal(expected, _action.GetMessage(id).Id==id);
        }
        

    }

    public class MessageRepositoryTest : IRepository<MessageDto>
    {
        private readonly List<MessageDto> _db;

        public MessageRepositoryTest()
        {
            _db = new List<MessageDto>();
            _db.Add(new MessageDto
            {
                Id = 1,
                ConferenceId = 1,
                Date = DateTime.Today,
                Text = "oko-oko-oko",
                UserId = 1
            });

            _db.Add(new MessageDto
            {
                Id = 2,
                ConferenceId = 1,
                Date = DateTime.Today,
                Text = "oko-oko-oko2",
                UserId = 2
            });
            _db.Add(new MessageDto
            {
                Id = 3,
                Date = DateTime.Today,
                Text = "oko-oko-oko",
                Location = "701,358",
                UserId = 1
            });

            _db.Add(new MessageDto
            {
                Id = 4,
                ConferenceId = 2,
                Date = DateTime.Today,
                Text = "oko-oko-oko2",
                UserId = 2
            });

            _db.Add(new MessageDto
            {
                Id = 5,
                ConferenceId = 2,
                Date = DateTime.Today,
                Text = "oko-oko-ok3",
                UserId = 3
            });
        }

        public void Add(MessageDto item)
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

        public MessageDto Get(int id)
        {
            return _db.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<MessageDto> GetList()
        {
            return _db;
        }

        public void Save()
        {
        }

        public void Update(MessageDto item)
        {
            if (item == null)
            {
                throw new NullReferenceException();
            }
            if (_db.FirstOrDefault(p => p.Id == item.Id) == null) return;
            _db.FirstOrDefault(p => p.Id == item.Id).Text = item.Text;
            _db.FirstOrDefault(p => p.Id == item.Id).Location = item.Location;
        }
    }
}
