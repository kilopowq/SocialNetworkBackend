using System;
using DAL.Models;
using DAL;
using Microsoft.Practices.Unity;
using NLog;

namespace BLL
{
    public class MessagesActions : IMessagesActions
    {
        public MessagesActions(UnityContainer container)
        {
            _messageRepository = container.Resolve<IRepository<MessageDto>>();
            _logger = container.Resolve<Logger>();
        }

        private readonly IRepository<MessageDto> _messageRepository;
        private static Logger _logger;

        /// <summary>
        /// Correct message.</summary>
        public bool CorrectMessage(MessageDto message)
        {
            try
            {
                _messageRepository.Update(message);
                _messageRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Return message by id.</summary>
        public MessageDto GetMessage(int id)
        {
            try
            {
                return _messageRepository.Get(id);
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return null;
            }

        }

        /// <summary>
        /// Send message on map.</summary>
        public bool PostMessage(MessageDto message)
        {
            if (message.ConferenceId != 0 || message.Location==null) return false;
            try
            {
                _messageRepository.Add(message);
                _messageRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }

        /// <summary>
        /// Sent message on conference.</summary>
        public bool SendMessage(MessageDto message)
        {
            if (message.Location != null || message.ConferenceId==0) return false;
            try
            {
                _messageRepository.Add(message);
                _messageRepository.Save();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e.Message + " in " + e.Source);
                return false;
            }
        }
    }
}
