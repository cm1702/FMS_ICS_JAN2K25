//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FMS_ICS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserCard
    {
        public int CardID { get; set; }
        public int UserID { get; set; }
        public int CardTypeID { get; set; }
        public string CardNumber { get; set; }
        public Nullable<decimal> RemainingLimit { get; set; }
        public System.DateTime Validity { get; set; }
        public string Status { get; set; }
    
        public virtual CardType CardType { get; set; }
        public virtual User User { get; set; }
    }
}
