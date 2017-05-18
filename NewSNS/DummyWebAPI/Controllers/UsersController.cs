using System.Web.Http;
using DAL.Models;
using BLL;

namespace DummyWebAPI.Controllers
{

    /// <summary>
    /// Users managment controller
    /// </summary>
    public class UsersController : ApiController
    {
        /// <summary>
        /// Delete user.
        /// </summary>
        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            return Ok(new UserActions(WebApiConfig.container).DeletePage(id));
        }

        /// <summary>
        /// TGet all users lsit.
        /// </summary>
        [HttpGet]
        public IHttpActionResult GetAllUsers()
        {
            return Ok(new UserActions(WebApiConfig.container).GetAllUsers());
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        [HttpGet]
        public IHttpActionResult GetUser(int id)
        {
            var user = new UserActions(WebApiConfig.container).GetUser(id);          
            if (user == null)
            {
                return NotFound();
            }           
            return Ok(user);

        }

        /// <summary>
        /// Reactivate deleted user.
        /// </summary>
        [HttpPut]
        [Route("api/users/reactivate/{id}")]
        public IHttpActionResult ReactivateUser(int id)
        {
            return Ok(new UserActions(WebApiConfig.container).ReActivatePage(id));
        }

        /// <summary>
        /// Update user.
        /// </summary>
        [HttpPut]
        public IHttpActionResult UpdateUser([FromBody] UserDto user)
        {
            return Ok(new UserActions(WebApiConfig.container).UpdateUser(user));
        }

        /// <summary>
        /// Find users by name.
        /// </summary>
        [HttpGet]
        [Route("api/users/byName")]
        public IHttpActionResult FindUsersByName(string name)
        {
            return Ok(new UserActions(WebApiConfig.container).FindUsersByName(name));
        }

        /// <summary>
        /// Find user by login.
        /// </summary>
        [HttpGet]
        [Route("api/users/byLogin")]
        public IHttpActionResult FindUsersByLogin(string login)
        {
            return Ok(new UserActions(WebApiConfig.container).FindUsersByLogin(login));
        }

    }
}
