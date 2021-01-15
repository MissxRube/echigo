using DevStudio;
using goshopping.Models;
using System.Web.Mvc;


namespace goshopping.Controllers
{
    public class HeartController : Controller
    {
        // GET: Heart
        public ActionResult Heart_List()
        {
            cvmHomeIndex model = new cvmHomeIndex()
            {
                HeartProducts = Shop.GetHeartProducts(),
                HeartNewProducts = Shop.HeartNew()
            };
            return View(model);
        }


        goshoppingEntities db = new goshoppingEntities();
       
        public ActionResult InputHeart()
        {
            HeartDetails model = new HeartDetails();

            return View(model);
        }

        [HttpPost]
        public ActionResult InputHeart(HeartDetails model)
        {
            if (!ModelState.IsValid) return View(model);

            db.Configuration.ValidateOnSaveEnabled = false;
            db.HeartDetails.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index", "Home");

        }

      

    }
}