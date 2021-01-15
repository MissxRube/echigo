using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using goshopping.Models;

namespace goshopping.Areas.Admin.Controllers
{
    public class HeartDetailController : Controller
    {
        // GET: Admin/Heart
        public ActionResult Index()
        {
            return View();
        }


        [LoginAuthorize(RoleNo = "Admin")]
        public ActionResult GetDataList()
        {
            using (Models.goshoppingEntities db = new goshoppingEntities())
            {
                var models = db.HeartDetails.OrderBy(m => m.rowid).ToList();
                return Json(new { data = models }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
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
        [LoginAuthorize(RoleNo = "Admin")]
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
                            heartDetail.Mno = models.Mno;
                            heartDetail.Mname = models.Mname;
                            heartDetail.remark = models.remark;
                            heartDetail.heart_title = models.heart_title;
                            heartDetail.heart_name = models.heart_name;
                            heartDetail.heart_content = models.heart_content;
                            heartDetail.heart_url = models.heart_url;
                            heartDetail.deadline_end = models.deadline_end;
                            heartDetail.deadline_start = models.deadline_start;
                            heartDetail.discount = models.discount;
                            heartDetail.flag = models.flag;
                            heartDetail.interest = models.interest;


                        }
                    }
                    else
                    {
                        //Save
                        db.HeartDetails.Add(models);
                    }
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        [LoginAuthorize(RoleNo = "Admin")]
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
        [LoginAuthorize(RoleNo = "Admin")]
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
    }
}