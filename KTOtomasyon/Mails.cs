//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KTOtomasyon
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mails
    {
        public int Mail_Id { get; set; }
        public string MailSubject { get; set; }
        public string MailBody { get; set; }
        public string MailTo { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public Nullable<bool> IsBodyHtml { get; set; }
        public Nullable<bool> IsSend { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string ErrorMessage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedUser { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
