//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace goshopping.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HeartDetails
    {
        public int rowid { get; set; }
        public string Mno { get; set; }
        public string Mname { get; set; }
        public string heart_no { get; set; }
        public string heart_title { get; set; }
        public string heart_name { get; set; }
        public string heart_content { get; set; }
        public Nullable<System.DateTime> deadline_start { get; set; }
        public string heart_url { get; set; }
        public string discount { get; set; }
        public Nullable<System.DateTime> deadline_end { get; set; }
        public string remark { get; set; }
        public Nullable<int> flag { get; set; }
        public Nullable<int> interest { get; set; }
    }
}