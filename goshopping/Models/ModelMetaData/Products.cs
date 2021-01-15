using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace goshopping.Models
{
    [MetadataType(typeof(ProductsMetaData))]
    public partial class Products
    {
        [Display(Name = "首頁")]
        public bool bool_istop { get; set; }
        [Display(Name = "上架")]
        public bool bool_issale { get; set; }
        [Display(Name = "顏色")]
        public bool bool_iscolor { get; set; }
        [Display(Name = "尺寸")]
        public bool bool_issize { get; set; }
        private class ProductsMetaData
        {
            [Key]
            [Display(Name = "記錄ID")]
            public int rowid { get; set; }
            [Display(Name = "分類ID")]
            public Nullable<int> categoryid { get; set; }
            [Display(Name = "商品分類")]
            public string category_name { get; set; }
            [Display(Name = "首頁顯示")]
            public Nullable<int> istop { get; set; }
            [Display(Name = "上架銷售")]
            public Nullable<int> issale { get; set; }
            [Display(Name = "區分顏色")]
            public Nullable<int> iscolor { get; set; }
            [Display(Name = "區分尺寸")]
            public Nullable<int> issize { get; set; }
            [Display(Name = "自定尺寸")]
            public string size_name { get; set; }
            [Display(Name = "自定顏色")]
            public string color_name { get; set; }
            [Display(Name = "瀏覽次數")]
            public Nullable<int> browse_count { get; set; }
            [Display(Name = "廠商編號")]
            public string vendor_no { get; set; }
            [Display(Name = "商品編號")]
            public string product_no { get; set; }
            [Display(Name = "商品名稱")]
            public string product_name { get; set; }
            [Display(Name = "商品規格")]
            public string product_spec { get; set; }
            [Display(Name = "銷售單價")]
            public Nullable<int> price { get; set; }
            [Display(Name = "推薦星數")]
            public Nullable<int> start_count { get; set; }
            [Display(Name = "備註")]
            public string remark { get; set; }


            [Display(Name = "商品敘述")]
            public string product_content { get; set; }

            [Display(Name = "下架")]
            public DateTime deadline_end { get; set; }
            [Display(Name = "上架")]
            public DateTime deadline_start { get; set; }
            [Display(Name = "標籤")]
            public string product_tag { get; set; }
        }
    }
}