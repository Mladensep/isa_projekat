//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IsaProjekat.DataAcces.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Visit
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RestourantId { get; set; }
        public Nullable<decimal> Rating { get; set; }
        public Nullable<System.DateTime> DateTime { get; set; }
    
        public virtual Restaurant Restaurant { get; set; }
        public virtual User User { get; set; }
    }
}