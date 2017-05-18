using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.Models;
using EFModels;

namespace DAL
{

    public class MessageRepository : IRepository<MessageDto>
    {
        private readonly SNSContext _db = new SNSContext();
        
        public void Add(MessageDto item)
        {
            var messageD = Mapper.Map<MessageDto, Message>(item);
            try { 
            _db.Messages.Add(messageD);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public void Close()
        {
            _db.Dispose();
        }

        public void Delete(int id)
        {
            _db.Messages.Remove(_db.Messages.Find(id));
        }

        public MessageDto Get(int id)
        {
            var messageEntity = _db.Messages.Find(id);
            var messageDTO = Mapper.Map<Message, MessageDto>(messageEntity);
            return messageDTO;
        }

        public IEnumerable<MessageDto> GetList()
        {
            IEnumerable<Message> messagesEntity = _db.Messages.ToList();
            var messagesDTO = Mapper.Map<IEnumerable<Message>, IEnumerable<MessageDto>>(messagesEntity).ToList();
            return messagesDTO;
        }

        public void Save()
        {
            try { 
            _db.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public void Update(MessageDto item)
        {
            if (item == null)
            {
                throw new NullReferenceException();
            }
            if (_db.Messages.Find(item.Id) == null) return;
            _db.Messages.Find(item.Id).Text = item.Text;
            _db.Messages.Find(item.Id).Location = item.Location;
        }
    }
}
