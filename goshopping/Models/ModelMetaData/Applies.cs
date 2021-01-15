using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace goshopping.Models
{
    [MetadataType(typeof(AppliesMetaData))]
  
    public partial class Applies
    {
    
        private class AppliesMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Required(ErrorMessage = "名稱不可空白")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入姓名")]
            [Display(Name = "請輸入公司名稱")]
            public string company { get; set; }


       
            [Required(ErrorMessage = "電子信箱不可空白")]
            [Display(Name = "電子信箱")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入電子信箱")]
            [EmailAddress(ErrorMessage = "請輸入電子信箱")]
            public string company_email { get; set; }

            [Required(ErrorMessage = "連絡電話不可空白")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入姓名")]
            [Display(Name = "請輸入連絡電話")]
            public string company_tel { get; set; }

            
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入姓名")]
            [Display(Name = "請輸入備註訊息")]
            public string remark { get; set; }
        }
    }
}