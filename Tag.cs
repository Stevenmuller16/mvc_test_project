//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVC_Challenge
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tag
    {
        public long TagID { get; set; }
        public Nullable<long> FK_PostID { get; set; }
        public string Tag1 { get; set; }
    
        public virtual Post Post { get; set; }
    }
}
