using BLL;
using DAL.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace DummyWebAPI.Controllers
{

    /// <summary>
    /// Conferences managment controller
    /// </summary>
    public class ConferencesController : ApiController
    {
        /// <summary>
        /// Get all user conferences starting from startId.
        /// </summary>
        [Route("api/users/{userID}/conferences/{startId}")]
        public IEnumerable<ConferenceDto> GetNewUserConferences(int userId, int startId)
        {
            var action = new ConferenceAction(WebApiConfig.container);

            var confs = action.GetAllConfs();
            var returnConfs = new List<ConferenceDto>();
            foreach (var conf in confs)
            {
                foreach (var member in conf.Members)
                {
                    if (member.Id == userId && conf.Id > startId)
                    {
                        returnConfs.Add(conf);
                    }
                }
            }
            return returnConfs;
        }
        
        /// <summary>
        /// Get all user conferences.
        /// </summary>
        [Route("api/users/{userID}/conferences")]
        public IEnumerable<ConferenceDto> GetUserConferences(int userId)
        {
            var action = new ConferenceAction(WebApiConfig.container);

            var confs = action.GetAllConfs();
            var returnConfs = new List<ConferenceDto>();
            foreach (var conf in confs)
            {
                foreach (var member in conf.Members)
                {
                    if (member.Id == userId)
                    {
                        returnConfs.Add(conf);
                    }
                }
            }
            return returnConfs;
        }

        /// <summary>
        /// Get a conference by id.
        /// </summary>
        public IHttpActionResult GetConference(int id)
        {
            var action = new ConferenceAction(WebApiConfig.container);
            var conference = action.GetConf(id);
            if (conference == null)
            {
                return NotFound();
            }
            return Ok(conference);
        }

        /// <summary>
        /// Create a new conference with two members.
        /// </summary>
        [HttpPost]
        public IHttpActionResult CreateDialog(int firstUser, int secondUser, string title)
        {
            var action = new ConferenceAction(WebApiConfig.container);
            var userAction = new UserActions(WebApiConfig.container);
            var members = new List<UserDto>();
            members.Add(userAction.GetUser(firstUser));
            members.Add(userAction.GetUser(secondUser));

            var conf = new ConferenceDto
            {
                Members = members,
                Title = title
            };

            return Ok(action.CreateConference(conf));
        }

        /// <summary>
        /// Create new conference.
        /// </summary>
        [HttpPost]
        public IHttpActionResult CreateConf([FromBody] List<UserDto> members, string title)
        {
            var action = new ConferenceAction(WebApiConfig.container);
            var conf = new ConferenceDto
            {
                Members = members,
                Title = title
            };

            return Ok(action.CreateConference(conf));
        }

        /// <summary>
        /// Create new conference named by members names.
        /// </summary>
        [HttpPost]
        public IHttpActionResult CreateDefaultNamedConf([FromBody] List<UserDto> members)
        {
            var action = new ConferenceAction(WebApiConfig.container);
            string name = "";
            members.ForEach(p=>name = name + p.FirstName+" "+p.LastName+", ");
            var conf = new ConferenceDto
            {
                Members = members,
                Title = name
            };

            return Ok(action.CreateConference(conf));
        }
    }
}
