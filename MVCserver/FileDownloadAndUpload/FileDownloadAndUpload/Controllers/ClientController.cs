using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileDownloadAndUpload.Core.Common;
using FileDownloadAndUpload.Models;
using FileDownloadAndUpload.Core.Xmpp;

namespace FileDownloadAndUpload.Controllers
{
    public class ClientController : Controller
    {
        //
        // GET: /Client/

        public ActionResult Index()
        {
            ViewBag.Users = MessageEntityFictory.ModelsEntities.User;
            return View();
        }
        /// <summary>
        /// broadcast message
        /// </summary>
        /// <param name="msg">message body</param>
        /// <returns></returns>
        public JsonResult Broadcast(string msg)
        {
            XmppServer.GetInstance().Broadcast(msg);
            return Json(new {result="success",message=msg });
        }

        public JsonResult GetUsers()
        {
            MessageEntities1 entities = MessageEntityFictory.ModelsEntities;
            return Json(entities.User, JsonRequestBehavior.AllowGet);

        }

    }
}
