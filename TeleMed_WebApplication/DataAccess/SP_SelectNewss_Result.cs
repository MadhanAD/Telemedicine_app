//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess
{
    using System;
    
    public partial class SP_SelectNewss_Result
    {
        public long newsID { get; set; }
        public string newsTitle { get; set; }
        public string newsDetail { get; set; }
        public byte[] newsThumbnail { get; set; }
        public byte[] newsImage { get; set; }
        public string cb { get; set; }
        public Nullable<System.DateTime> cd { get; set; }
        public string mb { get; set; }
        public Nullable<System.DateTime> md { get; set; }
        public Nullable<bool> active { get; set; }
        public string newsThumbnailBase64 { get; set; }
        public string newsImageBase64 { get; set; }
    }
}