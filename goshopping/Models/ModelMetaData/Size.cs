using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace goshopping.Models
{
    [MetadataType(typeof(SizeMetaData))]
    public partial class Size
    {
        private class SizeMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "尺寸編號")]
            [Required(ErrorMessage = "尺寸編號不可空白!!")]
            public string mno { get; set; }
            [Display(Name = "尺寸名稱")]
            public string mname { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }
        }
    }
}