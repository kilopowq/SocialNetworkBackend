using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using EFModels;
using AutoMapper;

namespace DAL
{
    public class FriendRepository : IRepository<FriendDto>
    {
        private readonly SNSContext _db = new SNSContext();
        
        public void Add(FriendDto item)
        {
            var friendD = Mapper.Map<FriendDto, Friend>(item);
            try { 
            _db.Friends.Add(friendD);
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
            _db.Friends.Remove(_db.Friends.Find(id));
        }

        public FriendDto Get(int id)
        {
            var friendEntity = _db.Friends.Find(id);
            var friendDTO = Mapper.Map<Friend, FriendDto>(friendEntity);
            return friendDTO;
        }

        public IEnumerable<FriendDto> GetList()
        {
            IEnumerable<Friend> friendsEntity = _db.Friends.ToList();
            var friendsDTO = Mapper.Map<IEnumerable<Friend>, IEnumerable<FriendDto>>(friendsEntity).ToList();
            return friendsDTO;
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

        public void Update(FriendDto item)
        {
            if (item == null)
            {
                throw new NullReferenceException();
            }
            var friend = Mapper.Map<FriendDto, Friend>(item);

            if (_db.Friends.Find(item.Id) == null) return;
            _db.Friends.Find(item.Id).User1_ID = friend.User1_ID;
            _db.Friends.Find(item.Id).User2_ID = friend.User2_ID;
            _db.Friends.Find(item.Id).StatusFriendship = friend.StatusFriendship;
        }
    }
}
