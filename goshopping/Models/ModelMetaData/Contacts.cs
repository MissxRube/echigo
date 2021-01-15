using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace goshopping.Models
{
    [MetadataType(typeof(ContactMetaData))]

    public partial class Contacts

    {

        private class ContactMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Required(ErrorMessage = "公司名稱不可空白")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入姓名")]
            [Display(Name = "請輸入公司名稱")]
            public string company { get; set; }



            [Required(ErrorMessage = "聯繫方式不可空白")]
         
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入姓名")]
            [Display(Name = "請輸入聯繫方式")]
            public string contact { get; set; }
            [Required(ErrorMessage = "訊息不可空白")]
            [DisplayFormat(ApplyFormatInEditMode = true, ConvertEmptyStringToNull = false, HtmlEncode = true, NullDisplayText = "請輸入姓名")]
            [Display(Name = "請輸入訊息")]
            public string remark { get; set; }
        }
    }
}