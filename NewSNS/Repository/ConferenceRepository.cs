using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DAL.Models;
using EFModels;

namespace DAL
{
    public class ConferenceRepository : IRepository<ConferenceDto>
    {
        private readonly SNSContext _db = new SNSContext();

        public void Add(ConferenceDto item)
        {
            var conferenceD = Mapper.Map<ConferenceDto, Conference>(item);
            
            var ids = conferenceD.Members.Select(mem => mem.Id).ToList();

            ICollection<User> query = (from p in _db.Users where ids.Contains(p.Id) select p).ToList();

            conferenceD.Members = query;

            try { 
            _db.Conferences.Add(conferenceD);
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
            _db.Conferences.Remove(_db.Conferences.Find(id));
        }

        public ConferenceDto Get(int id)
        {
            var conferenceEntity = _db.Conferences.Find(id);
            var conferenceDTO = Mapper.Map<Conference, ConferenceDto>(conferenceEntity);

            return conferenceDTO;
        }

        public IEnumerable<ConferenceDto> GetList()
        {
            IEnumerable<Conference> conferencesEntitys = _db.Conferences;

            var conferencesDTO = Mapper.Map<IEnumerable<Conference>, IEnumerable<ConferenceDto>>(conferencesEntitys);
            
            return conferencesDTO;
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

        public void Update(ConferenceDto item)
        {
            if (item == null)
            {
                throw new NullReferenceException();
            }
            var conferenceD = Mapper.Map<ConferenceDto, Conference>(item);
            var ids = conferenceD.Members.Select(mem => mem.Id).ToList();

            ICollection<User> query = (from p in _db.Users where ids.Contains(p.Id) select p).ToList();

            if (_db.Conferences.Find(item.Id) == null) return;
            _db.Conferences.Find(item.Id).Photo = item.Photo;
            _db.Conferences.Find(item.Id).Title = item.Title;
            _db.Conferences.Find(item.Id).Members = query;
        }
    }
}
