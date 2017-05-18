using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.Models;
using EFModels;

namespace DAL
{
    public class UserRepository:IRepository<UserDto>
    {
        private readonly SNSContext _db = new SNSContext();

        public IEnumerable<UserDto> GetList()
        {
            IEnumerable<User> usersEntity = _db.Users.ToList();
            var usersDTO = Mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(usersEntity).ToList();
            return usersDTO;
        }       

        public UserDto Get(int id)
        {
            var userEntity = _db.Users.Find(id);
            var userDTO = Mapper.Map<User, UserDto>(userEntity);
            return userDTO;
        }

        public void Add(UserDto item)
        {
            var userD = Mapper.Map<UserDto, User>(item);
            try
            {
                _db.Users.Add(userD);
            }
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        public void Update(UserDto item)
        {
            var user = Mapper.Map<UserDto, User>(item);
            if (item == null)
            {
                throw new NullReferenceException();
            }
            if (_db.Users.Find(item.Id) == null) return;
            _db.Users.Find(item.Id).FirstName = user.FirstName;
            _db.Users.Find(item.Id).LastName = user.LastName;
            _db.Users.Find(item.Id).Login = user.Login;
            _db.Users.Find(item.Id).Password = user.Password;
            _db.Users.Find(item.Id).Info = user.Info;
            _db.Users.Find(item.Id).Phone = user.Phone;
            _db.Users.Find(item.Id).UserState = user.UserState;
            _db.Users.Find(item.Id).Avatar = user.Avatar;

        }

        public void Delete(int id)
        {
            _db.Users.Remove(_db.Users.Find(id));
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

        public void Close()
        {
            _db.Dispose();
        }
    }
}
