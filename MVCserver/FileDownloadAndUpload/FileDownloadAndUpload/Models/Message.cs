//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileDownloadAndUpload.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Nullable<int> From { get; set; }
        public Nullable<int> To { get; set; }
        public byte[] Time { get; set; }
        public Nullable<int> Status { get; set; }
        public string Mid { get; set; }
        public string Resource { get; set; }
    }
}