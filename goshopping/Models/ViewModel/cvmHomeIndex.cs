using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace goshopping.Models
{
    public class cvmHomeIndex
    {
        public List<string> CarouseImages { get; set; }
        public List<Products> TopProducts { get; set; }

        public List<HeartDetails> HeartProducts { get; set; }

        public List<HeartDetails> HeartNewProducts { get; set; }
        public List<Products> GetProductsLife { get; set; }
        public List<Products> GetProductsColth { get; set; }
        public List<Products> GetProductsItem { get; set; }
        public List<Products>  GetProductsBeaut { get; set; }

    }
}