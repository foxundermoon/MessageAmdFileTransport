using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileDownloadAndUpload.Models;
using FileDownloadAndUpload.Core.Common;

namespace FileDownloadAndUpload.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }
        public string Reg(User user)
        {
            MessageEntities1  entities = MessageEntityFictory.ModelsEntities;
            if(entities.User.Any(u=>u.Email==user.Email))
            {
                return "the email was registered!";

            }
            else
            {
                try
                {
                entities.User.Add(user);
                entities.SaveChanges();
                }
                catch(Exception e)
                {
                    return "Exception when save!  message:" + e.Message;
                }
            }

            return "reg success ";
        }

    }
}
