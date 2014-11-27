using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using FileDownloadAndUpload.Models;
using FileDownloadAndUpload.Core.Utils;
using System.Data.Entity.Validation;
namespace FileDownloadAndUpload.Controllers
{
    public class UploadController : Controller
    {
        //Models.FileManagerEntities _fms;
        Models.MessageEntities1 _enti;
        public UploadController()
        {
            //_fms = new Models.FileManagerEntities();
            _enti = new MessageEntities1();
        }
        //
        // GET: /Upload/
        //[HttpPost]
        public JsonResult Index()
        {
            bool success = false;
            HttpFileCollectionBase files = Request.Files;
            List<FileDownloadAndUpload.Models.File> uploadFiles = new List<Models.File>();
            List<string> downloadSrc = new List<string>();
            //int maxid = _fms. 
            foreach (var key in files.AllKeys)
            {
                HttpPostedFileBase hpf = files.Get(key);
                //Models.File mf = new Models.File();
                Models.File mf = new Models.File();
                mf.Name = hpf.FileName;
                mf.Size = hpf.ContentLength;
                string guid = Guid.NewGuid().ToString();
                string p = Path.Combine(Request.MapPath("UploadFiles"), Path.GetFileName(guid));
                hpf.SaveAs(p);
                if (mf.Size > 0)
                {
                    mf.MD5 = HashHelper.MD5File(p);
                    string newPath = p.Replace(guid, mf.MD5);
                    if (!System.IO.File.Exists(newPath)) //如果相同校验的文件不存在则以md5为文件名存储
                        System.IO.File.Move(p, newPath);
                    else
                        System.IO.File.Delete(p); //如果有相同校验的文件就删除已经上传的文件
                    _enti.File.Add(mf);
                    uploadFiles.Add(mf);
                    downloadSrc.Add(mf.MD5);
                }
               

            }
            if (Request.Headers.AllKeys.Any((key)=>key=="IsStream"))
            {
                if(Request.Headers.Get("IsStream")=="true")
                {
                    Models.File mf = new Models.File();
                    mf.Name = Request.Form.Get("file_name");
                    mf.Size = (int)Request.InputStream.Length;
                    if (Request.InputStream.CanRead)
                    {
                        string guid = Guid.NewGuid().ToString();
                        string p = Path.Combine(Request.MapPath("UploadFiles"), Path.GetFileName(guid));
                        using(FileStream fs = new FileStream(p,FileMode.Create))
                        {
                            Request.InputStream.CopyTo(fs);
                        }
                        mf.MD5 = HashHelper.MD5File(p);
                        string newPath = p.Replace(guid, mf.MD5);
                        if (!System.IO.File.Exists(newPath)) //如果相同校验的文件不存在则以md5为文件名存储
                            System.IO.File.Move(p, newPath);
                        else
                            System.IO.File.Delete(p); //如果有相同校验的文件就删除已经上传的文件
                        _enti.File.Add(mf);
                        uploadFiles.Add(mf);
                        downloadSrc.Add(mf.MD5);

                    }

                }

            }
            ViewBag.Files = uploadFiles;
            //_fms.Configuration.ValidateOnSaveEnabled = false; 
            try
            {
                _enti.SaveChanges();
                ViewBag.SrcList = downloadSrc;  //"http://localhost:6608/Download?fid=" + 
                success = true;
            }
            catch (DbEntityValidationException bdex)
            {
                string msg = bdex.Message;
                ViewBag.Message = msg;
            }
            ViewBag.Success = success;
            //_fms.Configuration.ValidateOnSaveEnabled = true; 

            //foreach(HttpPostedFileBase f in files)
            //{
            //    string p = Path.Combine(Request.MapPath("UploadFiles"), Path.GetFileName(f.FileName));
            //    f.SaveAs(p);
            //    ViewBag.Msg += "success!" + p +"<br />";
            //}

            return Json(downloadSrc);
        }

   


    }
}
