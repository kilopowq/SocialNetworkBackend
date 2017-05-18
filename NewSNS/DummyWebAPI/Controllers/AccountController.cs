using System.Web.Http;
using BLL;
using DAL.Models;

namespace DummyWebAPI.Controllers
{
    /// <summary>
    /// Account managment controller
    /// </summary>
    public class AccountController : ApiController
    {
        /// <summary>
        /// Login and turn account "online".
        /// </summary>
        public IHttpActionResult GetAccount(string login, string password)
        {
            var action = new UserActions(WebApiConfig.container);
            var user = action.Login(login, password);
            if (user == null) return NotFound();
            if (user.UserState == State.Offline) action.OnPage(user.Id);
            return Ok(user);
        }

        /// <summary>
        /// Turn account "offline".
        /// </summary>
        [HttpDelete]
        public IHttpActionResult OffAccount(int userId)
        {
            return Ok(new UserActions(WebApiConfig.container).OffPage(userId));
        }

        /// <summary>
        /// Register a new account.
        /// </summary>
        [HttpPost]
        public IHttpActionResult Register([FromBody] UserDto user)
        {
            var action = new UserActions(WebApiConfig.container);
            user.UserState = State.Active;
            if (action.Register(user))
            {
                return Ok(user);
            }
            return Ok<UserDto>(null);
        }

        /// <summary>
        /// Change account password.
        /// </summary>
        [HttpPut]
        [Route("api/account/change")]
        public IHttpActionResult ChangePassword(int userId, string oldPass, string newPass)
        {
            if (new UserActions(WebApiConfig.container).ChangePassword(userId, oldPass, newPass))
            {
                return Ok();
            }
            return NotFound();
        }

    }
}
