using BLL;
using System.Web.Http;

namespace DummyWebAPI.Controllers
{
    /// <summary>
    /// Friends managment controller
    /// </summary>
    public class FriendsController : ApiController
    {
        /// <summary>
        /// Subscribe first user to the second.
        /// </summary>
        [HttpPut]
        [Route("api/follows")]
        public IHttpActionResult Subscribe(int firstUser, int secondUser)
        {
            var action = new FriendsListAction(WebApiConfig.container);
            if (action.Follow(firstUser, secondUser))
            {
                return Ok(true);
            }
            return NotFound();
        }

        /// <summary>
        /// Add first friend to friends list.
        /// </summary>
        [HttpPut]
        [Route("api/friends")]
        public IHttpActionResult AddToFriend(int firstUser, int secondUser)
        {
            return Ok(new FriendsListAction(WebApiConfig.container).AddFriend(firstUser, secondUser));
        }

        /// <summary>
        /// Remove user from friends list.
        /// </summary>
        [HttpDelete]
        [Route("api/friends")]
        public IHttpActionResult RemoveFriend(int firstUser, int secondUser)
        {
            return Ok(new FriendsListAction(WebApiConfig.container).DeleteFriend(firstUser, secondUser));
        }

        /// <summary>
        /// Unfollow first user from the second.
        /// </summary>
        [HttpDelete]
        [Route("api/follows")]
        public IHttpActionResult Unsuncribe(int firstUser, int secondUser)
        {
            return Ok(new FriendsListAction(WebApiConfig.container).Unfollow(firstUser, secondUser));
        }

        /// <summary>
        /// Get all user followers.
        /// </summary>
        [HttpGet]
        [Route("api/followers/{id}")]
        public IHttpActionResult GetFollowers(int id)
        {
            return Ok(new FriendsListAction(WebApiConfig.container).GetFollowersList(id));
        }

        /// <summary>
        /// Get all user follows.
        /// </summary>
        [HttpGet]
        [Route("api/follows/{id}")]
        public IHttpActionResult GetFollows(int id)
        {
            return Ok(new FriendsListAction(WebApiConfig.container).GetFollowsList(id));
        }

        /// <summary>
        /// Get all user friends.
        /// </summary>
        [HttpGet]
        [Route("api/friends/{id}")]
        public IHttpActionResult GetFriends(int id)
        {
            return Ok(new FriendsListAction(WebApiConfig.container).GetFriendsList(id));
        }
    }
}
