using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FileDownloadAndUpload.Models;

namespace FileDownloadAndUpload.Core.Common
{
    public class MessageEntityFictory
    {
        private static MessageEntities1 instance;
        public static MessageEntities1 ModelsEntities
        {
            get
            {
                if(instance==null)
                {
                    instance = new MessageEntities1();
                }
                return instance;
            }
        }
    }
}