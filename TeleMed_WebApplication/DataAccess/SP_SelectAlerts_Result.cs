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
    
    public partial class SP_SelectAlerts_Result
    {
        public long alertID { get; set; }
        public string alertText { get; set; }
        public string cb { get; set; }
        public Nullable<System.DateTime> cd { get; set; }
        public string mb { get; set; }
        public Nullable<System.DateTime> md { get; set; }
        public bool active { get; set; }
        public Nullable<bool> read { get; set; }
        public Nullable<long> alertFor { get; set; }
    }
}
