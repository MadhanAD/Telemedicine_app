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
    using System.Collections.Generic;
    
    public partial class AppAttachment
    {
        public long appAttachmentID { get; set; }
        public Nullable<long> appID { get; set; }
        public string FileName { get; set; }
        public byte[] fileContent { get; set; }
        public string fileurl { get; set; }
    
        public virtual Appointment Appointment { get; set; }
    }
}
