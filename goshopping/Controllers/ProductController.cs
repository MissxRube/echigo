using DevStudio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using goshopping.Models;
using DevStudio.Securitys;

namespace goshopping.Controllers
{
    public class ProductController : Controller
    {
        goshoppingEntities db = new goshoppingEntities();

        public ActionResult Product()
        {
            //Products model = new Products ();

            cvmHomeIndex model = new cvmHomeIndex()
            {
                CarouseImages = Shop.GetCarouselImages(),
                TopProducts = Shop.GetTopProducts(),
                GetProductsItem = Shop.GetProducts(),
                GetProductsLife = Shop.GetTopLifeProducts(),
                GetProductsColth = Shop.GetTopColthProducts(),
                GetProductsBeaut = Shop.GetBeautProducts()

            };
            return View(model);

        }

        public ActionResult Productlists()
        {
            //Products model = new Products ();

            cvmHomeIndex model = new cvmHomeIndex()
            {
                CarouseImages = Shop.GetCarouselImages(),
                TopProducts = Shop.GetTopProducts(),
                GetProductsItem = Shop.GetProducts(),
                GetProductsLife = Shop.GetTopLifeProducts(),
                GetProductsColth = Shop.GetTopColthProducts(),
                GetProductsBeaut = Shop.GetBeautProducts()

            };
            return View(model);

        }


        public ActionResult CategoryList(string id)
        {
            int int_id = 0;

            ViewData["CategoryNo"] = id;
            ViewBag.CategoryNo = id;

            ViewBag.CategoryName = Shop.GetCategoryName(id, ref int_id);
            var model = db.Products
                .Where(m => m.categoryid == int_id)
                .OrderBy(m => m.product_no)
                .ToList();
            return View(model);
        }



        public ActionResult ProductDetail(string id)
        {
            if (id == null) id = "";
            var model = db.Products.Where(m => m.product_no == id).FirstOrDefault();
            if (model == null) return RedirectToAction("Index", "Home");

            string categoty_no = "";
            string categoty_name = "";
            Shop.GetCategoryByProductNo(model.product_no, ref categoty_no, ref categoty_name);

            List<string> color_list = Shop.GetColorList(model.product_no);
            List<string> size_list = Shop.GetSizeList(model.product_no);

            ViewBag.CategoryNo = categoty_no;
            ViewBag.CategoryName = categoty_name;
            ViewBag.ColorList = color_list;
            ViewBag.SizeList = size_list;
            return View(model);
        }

        [HttpPost]
        public ActionResult ProductDetail(string qty, string color_no, string size_no, string product_no)
        {
            int int_qty = 0;
            int.TryParse(qty, out int_qty);
            Cart.AddCart(product_no, color_no, size_no, int_qty);
            return RedirectToAction("ProductDetail", "Product", new { id = product_no });
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult CartList()
        {
            var models = db.Carts
                .Where(m => m.user_no == UserAccount.UserNo)
                .OrderBy(m => m.product_no)
                .ToList();
            return View(models);
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult AddCart(string id)
        {
            return RedirectToAction("ProductDetail", "Product", new { id = id });
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult CartDelete(int id)
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                db.Carts.Remove(data);
                db.SaveChanges();
            }
            return RedirectToAction("CartList");
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult CartPlus(int id)
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                data.qty += 1;
                data.amount = data.qty * data.price;
                db.SaveChanges();
            }
            return RedirectToAction("CartList");
        }

        [LoginAuthorize(RoleNo = "Guest,Member")]
        public ActionResult CartMinus(int id)
        {
            var data = db.Carts
                .Where(m => m.rowid == id)
                .FirstOrDefault();
            if (data != null)
            {
                if (data.qty > 1)
                {
                    data.qty -= 1;
                    data.amount = data.qty * data.price;
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CartList");
        }

        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult Checkout()
        {
            cvmOrders models = new cvmOrders()
            {
                receive_name = "",
                receive_email = "",
                receive_address = "",
                payment_no = "01",
                shipping_no = "01",
                remark = "",
                PaymentsList = db.Payments.OrderBy(m => m.mno).ToList(),
                ShippingsList = db.Shippings.OrderBy(m => m.mno).ToList()
            };

            return View(models);
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult Checkout(cvmOrders model)
        {
            if (!ModelState.IsValid)
            {
                if (model.PaymentsList == null)
                {
                    model.PaymentsList = db.Payments.OrderBy(m => m.mno).ToList();
                }
                if (model.ShippingsList == null)
                {
                    model.ShippingsList = db.Shippings.OrderBy(m => m.mno).ToList();
                }
                return View(model);
            }

            Cart.CartPayment(model);

            return RedirectToAction("CheckoutReport");
        }

        public ActionResult CheckoutReport()
        {
            return View();
        }

        public ActionResult agreement() 
        {
            return View();

        }



        [HttpPost]
        public ActionResult Search(string searchText)
        {
            ViewBag.SearchText = searchText;
            //空白返回XXXX不給搜
            if (string.IsNullOrEmpty(searchText)) searchText = "XXXXXXXXX";
            var model = db.Products
                .Where(m => m.product_no.Contains(searchText) || m.product_name.Contains(searchText))
                .ToList();
            return View(model);
        }



        public JsonResult GetCategoryMenuList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                int int_count = 0;
                string str_text = "";
                string str_href = "";
                List<Node> models = new List<Node>();

                var node1 = Shop.GetCategorys(0);
                if (node1.Count > 0)
                {
                    foreach (var item1 in node1)
                    {
                        int_count = Shop.GetProductCount(item1.rowid);
                        str_text = item1.category_name;
                        str_href = string.Format("/Product/CategoryList/{0}", item1.category_no);
                        Node model1 = new Node();
                        model1.text = str_text;

                        var node2 = Shop.GetCategorys(item1.rowid);
                        if (node2.Count == 0 && int_count > 0) model1.href = str_href;

                        if (node2.Count > 0)
                        {
                            foreach (var item2 in node2)
                            {
                                int_count = Shop.GetProductCount(item2.rowid);
                                str_text = item2.category_name;
                                str_href = string.Format("/Product/CategoryList/{0}", item2.category_no);
                                Node model2 = new Node();
                                model2.text = str_text;

                                var node3 = Shop.GetCategorys(item2.rowid);
                                if (node3.Count == 0 && int_count > 0) model2.href = str_href;

                                if (node3.Count > 0)
                                {
                                    foreach (var item3 in node3)
                                    {
                                        int_count = Shop.GetProductCount(item3.rowid);
                                        str_text = item3.category_name;
                                        str_href = string.Format("/Product/CategoryList/{0}", item3.category_no);
                                        Node model3 = new Node();
                                        model3.text = str_text;
                                        if (int_count > 0) model3.href = str_href;

                                        model2.nodes.Add(model3);
                                    }
                                }
                                model1.nodes.Add(model2);
                            }
                        }
                        models.Add(model1);
                    }
                }

                var jdata = Json(models, JsonRequestBehavior.AllowGet);
                return jdata;
            }
        }
        


    }
}