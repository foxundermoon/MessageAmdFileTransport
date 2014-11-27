using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FileDownloadAndUpload.Models;
using FileDownloadAndUpload.Core.Common;
using System.Diagnostics;

namespace FileDownloadAndUpload.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/
        MessageEntities1 _men;
        public TestController()
        {
            //db = new FileManagerEntities();
            _men = new MessageEntities1();
        }

        public ActionResult Index()
        {
            ViewBag.Msg = "hello  test";
            //ViewData.Model = db.File.ToList();

            //File f = new File();
            //f.Name = "test1";
            //f.MD5 = "111111111";
            //f.Size = 1232;
            //f.ID = id;
            //db.File.Add(f);
            //db.SaveChanges();

            return View();
        }
        [HttpGet]
        public ActionResult DownloadPage()
        {
            string fileDir = Request.MapPath("UploadFiles");
            var query = from file in _men.File
                        select file;
            List<File> files = new List<File>();
            //ViewDataDictionary<File> vd = new ViewDataDictionary<File>();

            int i = 1;
            foreach (var item in query)
            {
                string filePath = fileDir + System.IO.Path.DirectorySeparatorChar + item.MD5;

                if (!System.IO.File.Exists(filePath))
                {
                    files.Add(item);
                }
            }

            ViewBag.
                Files = files;
            return View();
        }

        public ActionResult DownloadPageAll()
        {
            string fileDir = Request.MapPath("UploadFiles");
            var query = from file in _men.File
                        select file;
            //List<File> files = new List<File>();
            //ViewDataDictionary<File> vd = new ViewDataDictionary<File>();

            //foreach (var item in query)
            //{
            //    string filePath = fileDir + System.IO.Path.DirectorySeparatorChar + item.MD5;

            //    if (System.IO.File.Exists(filePath))
            //    {
            //        files.Add(item);
            //    }
            //}

            ViewBag.
                Files = query.ToList();
            return View();
        }

        public ActionResult Reg()
        {

            FileDownloadAndUpload.Models.User newUser = MessageEntityFictory.ModelsEntities.User.Create();
            return View(newUser);
        }

    //[HttpPost]
        public String TestPost()
        {
            if(Request.Params.Count>0)
            {
                Trace.Write(Request.Params.Keys.ToString() + Request.Params.GetValues("test").ToString(), "TestController.TestPost()");
                if(Request.Params.Get("test")=="im in idea")
                {
                    return "ok";
                }
            }
            return "fail";        }
      


    }
}
