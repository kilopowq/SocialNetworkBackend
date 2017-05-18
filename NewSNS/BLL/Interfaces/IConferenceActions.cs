using DAL.Models;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IConferenceActions
    {

        bool CreateConference(ConferenceDto conf);

        IEnumerable<MessageDto> GetMessageFromConf(int confId);
            
        IEnumerable<UserDto> GetUsersFromConf(int confId);

        IEnumerable<ConferenceDto> GetAllConfs();

        ConferenceDto GetConf(int id);

        IEnumerable<MessageDto> GetNewMessagesFromConf(int confId, int startFrom);

        IEnumerable<ConferenceDto> GetUserConferences(int userId);
    }
}
