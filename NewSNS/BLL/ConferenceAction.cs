using System;
using BLL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using DAL.Models;
using DAL;
using Microsoft.Practices.Unity;
using NLog;

namespace BLL
{
    public class ConferenceAction : IConferenceActions
    {
        public ConferenceAction(IUnityContainer container)
        {
            _conferenceRepository = container.Resolve<IRepository<ConferenceDto>>();
            _messageRepository = container.Resolve<IRepository<MessageDto>>();
            _userRepository = container.Resolve<IRepository<UserDto>>();
            _logger = container.Resolve<Logger>();
        }

        private readonly IRepository<ConferenceDto> _conferenceRepository;
        private readonly IRepository<MessageDto> _messageRepository;
        private readonly IRepository<UserDto> _userRepository;
        private static Logger _logger;

        /// <summary>
        /// Create a new conference.</summary>
        public bool CreateConference(ConferenceDto conf)
        {
            if (conf.Members.Count < 2) return false;
            
            if (conf.Members.Any(member => member == null))
            {
                return false;
            }
            
            try
            {
                _conferenceRepository.Add(conf);
                _conferenceRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Returns all conferences.</summary>
        public IEnumerable<ConferenceDto> GetAllConfs()
        {
            var confs = _conferenceRepository.GetList();
            
            return confs;
        }

        /// <summary>
        /// Returns conference by id.</summary>
        public ConferenceDto GetConf(int id)
        {
            return _conferenceRepository.Get(id);
        }

        /// <summary>
        /// Returns all messages from conference.</summary>
        public IEnumerable<MessageDto> GetMessageFromConf(int confId)
        {
            return _messageRepository.GetList().Where(p=>p.ConferenceId==confId);            
        }

        /// <summary>
        /// Returns messages from conferences where Message id >= startFrom.</summary>
        public IEnumerable<MessageDto> GetNewMessagesFromConf(int confId, int startFrom)
        {
            return _conferenceRepository.Get(confId).Messages.Where(p => p.Id > startFrom);
        }

        /// <summary>
        /// Returns all user conferences.</summary>
        public IEnumerable<ConferenceDto> GetUserConferences(int userId)
        {
            var confs = _conferenceRepository.GetList();

            return (from conf in confs from member in conf.Members where member.Id == userId select conf).ToList();
        }

        /// <summary>
        /// Returns members of conference.</summary>
        public IEnumerable<UserDto> GetUsersFromConf(int confId)
        {
            var allUsers = _userRepository.GetList();
            var conf = _conferenceRepository.Get(confId);

            return conf.Members.Select(member => allUsers.FirstOrDefault(p => p.Id == member.Id)).ToList();
        }
    }
}
