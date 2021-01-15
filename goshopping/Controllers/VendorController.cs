using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using goshopping.Models;
using DevStudio.Securitys;

namespace goshopping.Controllers
{
    public class VendorController : Controller
    {
        goshoppingEntities db = new goshoppingEntities();

       
        // GET: Vendor

        [HttpPost]
        public ActionResult RegisterVendor(cvmRegister model)
        {
            if (!ModelState.IsValid) return View(model);

            //自定義檢查
            bool bln_error = false;
            var check = db.Users.Where(m => m.mno == model.mno).FirstOrDefault();
            if (check != null) { ModelState.AddModelError("", "帳號重覆註冊!"); bln_error = true; }
            check = db.Users.Where(m => m.email == model.email).FirstOrDefault();
            if (check != null) { ModelState.AddModelError("", "電子信箱重覆註冊!"); bln_error = true; }
            if (bln_error) return View(model);

            //密碼加密
            using (Cryptographys cryp = new Cryptographys())
            {
                model.password = cryp.SHA256Encode(model.password);
                model.ConfirmPassword = model.password;
            }

            Users user = new Users();
            user.mno = model.mno;
            user.mname = model.mname;
            user.password = model.password;
            user.email = model.email;
            user.birthday = model.birthday;
            user.remark = model.remark;
            user.role_no = "Vendor";  //設定角色代號為 User
            user.varify_code = UserAccount.GetNewVarifyCode(); //產生驗證碼
            user.isvarify = 0;

            //寫入資料庫
            try
            {
                db.Configuration.ValidateOnSaveEnabled = false;
                db.Users.Add(user);
                db.SaveChanges();
                db.Configuration.ValidateOnSaveEnabled = true;
            }
            catch (Exception ex)
            {
                string str_message = ex.Message;
            }

            //寄出驗證信
            SendVerifyMail(model.email, user.varify_code);
            return RedirectToAction("SendEmailResult");
        }

        private string SendVerifyMail(string userEmail, string varifyCode)
        {
            string str_app_name = "一起購購物商城";
            var str_url = string.Format("/User/VerifyEmail/{0}", varifyCode);
            var str_link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, str_url);
            string str_subject = str_app_name + " - 帳號成功建立通知!!";
            string str_body = "<br/><br/>";
            str_body += "很高興告訴您，您的 " + str_app_name + " 帳戶已經成功建立. <br/>";
            str_body += "請按下下方連結完成驗證您的帳號程序!!<br/><br/>";
            str_body += "<a href='" + str_link + "'>" + str_link + "</a> ";
            str_body += "<br/><br/>";
            str_body += "本信件由電腦系統自動寄出,請勿回信!!<br/><br/>";
            str_body += string.Format("{0} 系統開發團隊敬上", str_app_name);

            GmailService gmail = new GmailService();
            gmail.ReceiveEmail = userEmail;
            gmail.Subject = str_subject;
            gmail.Body = str_body;
            gmail.Send();
            return gmail.MessageText;
        }

        [HttpGet]
        public ActionResult SendEmailResult()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerifyEmail(string id)
        {
            bool Status = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            var user = db.Users.Where(m => m.varify_code == id).FirstOrDefault();
            if (user == null)
            { ViewBag.Message = "驗證碼錯誤!!"; }
            else
            {
                if (user.isvarify == 1)
                { ViewBag.Message = "此電子信箱已完成驗證, 請勿重覆執行!!"; }
                else
                {
                    user.isvarify = 2;
                    db.SaveChanges();
                    Status = true;
                }
            }
            ViewBag.Status = Status;
            return View();
        }





    }
}