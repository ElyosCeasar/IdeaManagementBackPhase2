//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataAccess.Mapper
{
    using System;
    using System.Collections.Generic;
    
    public partial class IDEA_COMMENTS
    {
        public int ID { get; set; }
        public string USERNAME { get; set; }
        public int IDEA_ID { get; set; }
        public string COMMENT { get; set; }
        public System.DateTime SAVE_DATE { get; set; }
        public Nullable<System.DateTime> MODIFY_DATE { get; set; }
    
        public virtual IDEA IDEA { get; set; }
        public virtual USER USER { get; set; }
    }
}
