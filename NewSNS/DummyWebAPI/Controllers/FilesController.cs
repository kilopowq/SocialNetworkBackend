using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using BLL;

namespace DummyWebAPI.Controllers
{
    /// <summary>
    /// Files managment controller
    /// </summary>
    public class FilesController : ApiController
    {

        /// <summary>
        /// Upload avatar to server.
        /// </summary>
        [HttpPost]
        [Route("api/files/avatar")]
        public IHttpActionResult UploadFile(int userId)
        {
            if (HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Current.Request.Files["avatar"];
                if (httpPostedFile != null)
                {
                    var fileSavePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Files"), httpPostedFile.FileName);
                    httpPostedFile.SaveAs(fileSavePath);
                    new UserActions(WebApiConfig.container).SaveAvatar(userId, "/Files/" + httpPostedFile.FileName);
                    return Ok();
                }

            }
            return NotFound();
        }

        /// <summary>
        /// Get user avatar file.
        /// </summary>
        [HttpGet]
        [Route("api/files/avatar")]
        public HttpResponseMessage Post(int userId)
        {
            var action = new UserActions(WebApiConfig.container);
            var path = @action.GetUser(userId).Avatar;
            if (path == null) return new HttpResponseMessage(HttpStatusCode.NotFound);
            path = HttpContext.Current.Server.MapPath("~") + path;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            
            return result;
    }

        /// <summary>
        /// Get user avatar path.
        /// </summary>
        [HttpGet]
        [Route("api/files/avatar/path")]
        public IHttpActionResult Get(int userId)
        {
            var host = HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            var action = new UserActions(WebApiConfig.container);
            var path = @action.GetUser(userId).Avatar;

            if (path == null) return NotFound();

            path = "http://" + host + path;
            return Ok(path);
        }
    }
}
