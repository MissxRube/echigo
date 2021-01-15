using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace goshopping.Models
{
    [MetadataType(typeof(HeartDetailsMetaData))]
    //[Bind(Exclude = "flag")]
    public partial class HeartDetails
    {
        //[Display(Name = "驗證")]
        //public bool bool_flag { get; set; }
        private class HeartDetailsMetaData
        {
            [Key]
            [Display(Name = "心願ID")]
            public int rowid { get; set; }

            [Display(Name = "會員id")]

            public int Mno { get; set; }
            [Display(Name = "會員姓名")]
            public string Mname { get; set; }
           
           
            [Display(Name = "標題")]
            public string heart_title { get; set; }
            [Display(Name = "產品名稱")]
            public string heart_name { get; set; }
            [Display(Name = "內容")]

            public string heart_content { get; set; }
        
           
            [Display(Name = "產品網址")]

            public string heart_url { get; set; }


            [Display(Name = "期限")]

            public Nullable<System.DateTime> deadline_end { get; set; }


            [Display(Name = "登記")]
            public Nullable<System.DateTime> deadline_start { get; set; }

            [Display(Name = "優惠")]
            public string discount { get; set; }



            [Display(Name = "備註")]
            public string remark { get; set; }


        }

    }
}
