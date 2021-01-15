using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using DevStudio;
using DevStudio.Securitys;
using goshopping.Models;


namespace goshopping.Areas.Member.Controllers
{
    public class HeartDetailController : Controller
    {
        // GET: Member/HeartDetail
        public ActionResult Index()
        {
            return View();
        }

        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult GetDataList()
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.HeartDetails
                    .Where(m => m.Mno == UserAccount.UserNo)
                    .OrderBy(m => m.heart_no)
                    .ToList();

                return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult Edit(int id = 0)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                if (id == 0)
                {
                    HeartDetails new_model = new HeartDetails();
                    return View(new_model);
                }
                var models = db.HeartDetails.Where(m => m.rowid == id).FirstOrDefault();
                return View(models);
            }
        }

        [HttpPost]
        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult Edit(HeartDetails models)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (goshoppingEntities db = new goshoppingEntities())
                {
                    if (models.rowid > 0)
                    {
                        //Edit 
                        var heartDetail = db.HeartDetails.Where(m => m.rowid == models.rowid).FirstOrDefault();
                        if (heartDetail != null)
                        {
                            heartDetail.Mno = UserAccount.UserNo;
                            heartDetail.Mname = models.Mname;
                            heartDetail.remark = models.remark;
                            heartDetail.heart_title = models.heart_title;
                            heartDetail.heart_name = models.heart_name;
                            heartDetail.heart_content = models.heart_content;
                            heartDetail.heart_url = models.heart_url;
                            heartDetail.deadline_end = models.deadline_end;
                            heartDetail.deadline_start = models.deadline_start;
                            heartDetail.discount = models.discount;
                            
                    
                        }
                    }
                    else
                    {
                        models.Mno = UserAccount.UserNo;
                        db.HeartDetails.Add(models);
                    }
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
            //return RedirectToAction("Index");
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult Delete(int id)
        {
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.HeartDetails.Where(m => m.rowid == id).FirstOrDefault();
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
        [LoginAuthorize(RoleNo = "Member")]
        public ActionResult DeleteData(int id)
        {
            bool status = false;
            using (goshoppingEntities db = new goshoppingEntities())
            {
                var model = db.HeartDetails.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    db.HeartDetails.Remove(model);
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
                var model = db.HeartDetails.Where(m => m.rowid == id).FirstOrDefault();
                if (model != null)
                {
                    Shop.HeartNo = model.heart_no;
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
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (file.ContentLength > 0)
                {
                    string str_folder = string.Format("~/Images/product/{0}", Shop.HeartNo);
                    string str_folder_path = Server.MapPath(str_folder);
                    if (!Directory.Exists(str_folder_path)) Directory.CreateDirectory(str_folder_path);
                    string str_file_name = Shop.HeartNo + ".jpg";
                    var path = Path.Combine(str_folder_path, str_file_name);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                    file.SaveAs(path);
                }
            }
            return RedirectToAction("Index");
        }
    }
}