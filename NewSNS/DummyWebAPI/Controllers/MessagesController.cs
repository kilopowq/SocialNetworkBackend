using BLL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using DummyWebAPI.Models;

namespace DummyWebAPI.Controllers
{

    /// <summary>
    /// Messages managment controller
    /// </summary>
    public class MessagesController : ApiController
    {
        /// <summary>
        /// Get all messages from conference.
        /// </summary>
        [HttpGet]
        [Route("api/conferences/{id}/messages")]
        public IEnumerable<MessageDto> GetMessagesFromConf(int id)
        {
            var confAction = new ConferenceAction(WebApiConfig.container);
            return confAction.GetMessageFromConf(id);
        }

        /// <summary>
        /// Get all messages from conferences, where message id > startId.
        /// </summary>
        [HttpGet]
        [Route("api/conferences/{confId}/messages/{startId}")]
        public IEnumerable<MessageDto> GetNewMessagesFromConf(int confId, int startId)
        {
            var confAction = new ConferenceAction(WebApiConfig.container);
            return confAction.GetNewMessagesFromConf(confId, startId);
        }

        /// <summary>
        /// Send message to the conference.
        /// </summary>
        [HttpPost]
        public IHttpActionResult SendMessage([FromBody] MessageDto message)
        {
            var action = new MessagesActions(WebApiConfig.container);

            message.Date= DateTime.Now;

            action.SendMessage(message);
            return Ok();
        }

        /// <summary>
        /// Post message on the map.
        /// </summary>
        [HttpPost]
        [Route("api/messages/post")]
        public IHttpActionResult PostMessage([FromBody] MessageDto message)
        {
            var action = new MessagesActions(WebApiConfig.container);

            message.Date = DateTime.Now;

            action.SendMessage(message);
            return Ok();
        }

        /// <summary>
        /// Correct message.
        /// </summary>
        [HttpPut]
        public IHttpActionResult CorrectMessage([FromBody] MessageForCorrect message)
        {
            var action = new MessagesActions(WebApiConfig.container);
            var initialMessage = action.GetMessage(message.Id);
            initialMessage.Text = message.Text;
            initialMessage.Date = DateTime.Now;
            action.CorrectMessage(initialMessage);
            return Ok();
        }

    }
}
