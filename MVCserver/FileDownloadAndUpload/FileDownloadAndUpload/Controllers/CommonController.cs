using FileDownloadAndUpload.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileDownloadAndUpload.Controllers
{
    public class CommonController : Controller
    {
        //
        // GET: /Common/

        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Duration = 0)]
        public ActionResult VCode()
        {
            string code = ValidateCode.GenerateCode(4);
            Session["vcode"] = code;
            return ValidateCode.WriteImage(code, Response);
        }

    }
}
