//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileDownloadAndUpload.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class File
    {
        public File()
        {
            this.User = new HashSet<User>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Size { get; set; }
        public Nullable<int> UploadSize { get; set; }
        public string MD5 { get; set; }
        public byte[] UploadTime { get; set; }
        public Nullable<int> UID { get; set; }
    
        public virtual ICollection<User> User { get; set; }
    }
}
