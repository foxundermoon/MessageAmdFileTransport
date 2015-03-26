using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload.Models;

namespace FileDownloadAndUpload.Core.Common
{
    public class MessageEntityFictory
    {
        private static Entities instance;
        public static Entities ModelsEntities
        {
            get
            {
                if(instance==null)
                {
                    instance = new Entities();
                }
                return instance;
            }
        }
    }
}