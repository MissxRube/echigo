using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevStudio;
using DevStudio.Securitys;
using goshopping.Models;

namespace goshopping.Areas.Vendor.Controllers
{
    public class ProductsController : Controller
    {
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Index()
        {
            return View();
        }

        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult GetDataList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.Products
                    .Where(m => m.vendor_no == UserAccount.UserNo)
                    .OrderBy(m => m.product_no)
                    .ToList();
                if (models.Count > 0)
                {
                    for (int i = 0; i < models.Count; i++)
                    {
                        models[i].bool_istop = (models[i].istop == 1);
                        models[i].bool_issale = (models[i].issale == 1);
                        models[i].bool_iscolor = (models[i].iscolor == 1);
                        models[i].bool_issize = (models[i].issize == 1);
                    }
                }
                return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Edit(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.Products.Where(m => m.rowid == id).FirstOrDefault();

                //Category DropDownList
                string str_rowid = "0";
                var categoryList = new List<SelectListItem>();
                List<Categorys> clists = db.Categorys.OrderBy(m => m.category_no).ToList();
                foreach (var item in clists)
                {
                    SelectListItem list = new SelectListItem();
                    list.Value = item.rowid.ToString();
                    list.Text = Shop.GetCategoryName(item.rowid);
                    categoryList.Add(list);
                    if (id == 0)
                    { if (str_rowid == "0") str_rowid = item.rowid.ToString(); }
                }

                if (models != null) str_rowid = models.categoryid.ToString();

                //預設選擇哪一筆
                categoryList.Where(m => m.Value == str_rowid).First().Selected = true;
                ViewBag.CategoryList = categoryList;

                if (id == 0)
                {
                    Products new_model = new Products();
                    new_model.size_name = "";
                    new_model.color_name = "";
                    new_model.remark = "";
                    new_model.start_count = 5;
                    new_model.browse_count = 0;
                    new_model.bool_istop = false;
                    new_model.bool_issale = true;
                    new_model.bool_iscolor = true;
                    new_model.bool_issize = true;
                    return View(new_model);
                }

                models.bool_istop = (models.istop == 1);
                models.bool_issale = (models.issale == 1);
                models.bool_iscolor = (models.iscolor == 1);
                models.bool_issize = (models.issize == 1);
                return View(models);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Edit(Products models)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (goshoppingEntities db = new goshoppingEntities())
                {
                    int int_cate_id = 0;
                    if (models.rowid > 0)
                    {
                        //Edit 
                        var products = db.Products.Where(m => m.rowid == models.rowid).FirstOrDefault();
                        if (products != null)
                        {
                            int_cate_id = models.categoryid.GetValueOrDefault();
                            products.product_no = models.product_no;
                            products.product_name = models.product_name;
                            products.product_spec = models.product_spec;
                            products.categoryid = int_cate_id;
                            products.category_name = Shop.GetCategoryName(int_cate_id);
                            products.istop = (models.bool_istop) ? 1 : 0;
                            products.issale = (models.bool_issale) ? 1 : 0;
                            products.issize = (models.bool_issize) ? 1 : 0;
                            products.iscolor = (models.bool_iscolor) ? 1 : 0;
                            products.price = models.price;
                            products.start_count = models.start_count;
                            products.browse_count = models.browse_count;
                            products.vendor_no = UserAccount.UserNo;
                            products.remark = models.remark;
                            products.product_content = models.product_content;
                            products.deadline_end = models.deadline_end;
                            products.deadline_start = models.deadline_start;
                            products.product_tag = models.product_tag;
                            products.color_name = models.color_name;
                            products.size_name = models.size_name;
                        }
                    }
                    else
                    {
                        //Save
                        models.vendor_no = UserAccount.UserNo;
                        int_cate_id = models.categoryid.GetValueOrDefault();
                        models.category_name = Shop.GetCategoryName(int_cate_id);
                        models.istop = (models.bool_istop) ? 1 : 0;
                        models.issale = (models.bool_issale) ? 1 : 0;
                        models.issize = (models.bool_issize) ? 1 : 0;
                        models.iscolor = (models.bool_iscolor) ? 1 : 0;
                        db.Products.Add(models);
                    }
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Delete(int id)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Products.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult DeleteData(int id)
        {
            bool status = false;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Products.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    db.Products.Remove(model);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Upload(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Products.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    Shop.ProductNo = model.product_no;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            try
            {
                if (files != null && files.Length > 0)
                {
                    int int_int_index = 0;
                    foreach (HttpPostedFileBase file in files)
                    {
                        if (file != null)
                        {
                            if (file.ContentLength > 0)
                            {
                                string fileName = Shop.ProductNo;
                                if (int_int_index > 0)
                                    fileName += '-' + int_int_index.ToString().PadLeft(2, '0');
                                fileName += ".jpg";

                                var path = Path.Combine(Server.MapPath("~/Images/product"), Shop.ProductNo);
                                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                                path = Path.Combine(path, fileName);
                                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                                file.SaveAs(path);
                                int_int_index++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str_message = ex.Message;
            }
            UserAccount.UploadImageMode = false;
            return RedirectToAction("ShowPhotos");

        }
        public string ShowPhotos()
        {
            string show = "";
            var path = Path.Combine(Server.MapPath("~/Images/product"), Shop.ProductNo);
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fInfo = dir.GetFiles();
            int n = 0;
            foreach (FileInfo result in fInfo)
            {
                n++;
                show += "<a href= ../../Images/product/"+ Shop.ProductNo + "/"+result.Name + "'target='_blank'><img src='../../Images/product/" + Shop.ProductNo + "/" + result.Name+ "'width='90' height ='60' border='0' ></a>";
                if (n % 4 == 0) 
                {
                    show += "<p>";
                }
            
            }
            show += "<p><a href='index'>確認</a>";
            return show;
        }

        [LoginAuthorize(RoleNo = "Vendor")]
        public ActionResult Remark(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Products.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    Shop.ProductNo = model.product_no;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Vendor")]
        [ValidateInput(false)]
        public ActionResult Remark(Products products)
        {
            bool status = false;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.Products.Where(m => m.rowid == products.rowid).FirstOrDefault();
                if (model != null)
                {
                    model.remark = products.product_content;
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            string url; // url to return
            string message; // message to display (optional)

            // 設定圖片上傳路徑
            string path = Server.MapPath("~/Images/uploads");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = System.IO.Path.Combine(path, Shop.ProductNo);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            string ImageName = upload.FileName;
            string str_file_name = System.IO.Path.Combine(path, ImageName);
            if (System.IO.File.Exists(str_file_name)) System.IO.File.Delete(str_file_name);
            upload.SaveAs(str_file_name);


            // 取得網址
            // http://localhost:9999/Images/uploads/00001/ImageName.jpg
            url = Request.Url.GetLeftPart(UriPartial.Authority) + "/Images/uploads/" + Shop.ProductNo + "/" + ImageName;

            // passing message success/failure
            message = "圖片儲存成功!!";

            // since it is an ajax request it requires this string
            string output = @"<html><body><script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\", \"" + message + "\");</script></body></html>";
            return Content(output);
        }
    }
}