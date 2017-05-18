using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DummyWebAPI.Controllers
{
    public class FriendListController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies["userId"] != null)
            {
                return View("Index");
            }
            return Content("<script>window.location = '/';</script>");
        }
    }
}