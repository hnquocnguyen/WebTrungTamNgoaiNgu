using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTrungTamNgoaiNgu.Models;

namespace WebTrungTamNgoaiNgu.Controllers
{
    public class ClassController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: Class
        public ActionResult ListClasses()
        {
            var all_classes = from c in data.Classes select c;
            return View(all_classes);
        }

        //public ActionResult DetailClass(int id)
        //{
        //    var d_class = data.Classes.Where(m => m.class_id == id).First();
        //    return View(d_class);
        //}

        public ActionResult CreateClass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateClass(FormCollection collection, Class _class)
        {
            var c_name = collection["Name"];
            var c_start_date = Convert.ToDateTime(collection["Start_Date"]);
            var c_end_date = Convert.ToDateTime(collection["End_Date"]);

            if (string.IsNullOrEmpty(c_name))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                _class.name = c_name.ToString();
                _class.start_date = c_start_date;
                _class.end_date = c_end_date;

                data.Classes.InsertOnSubmit(_class);
                data.SubmitChanges();
                return RedirectToAction("ListClasses");
            }
            return this.CreateClass();
        }

        public ActionResult EditClass(int id)
        {
            var e_class = data.Classes.First(m => m.class_id == id);
            return View(e_class);
        }
        [HttpPost]
        public ActionResult EditClass(int id, FormCollection collection)
        {
            var e_class = data.Classes.First(m => m.class_id == id);
            var e_name = collection["Name"];
            var e_start_date = Convert.ToDateTime(collection["Start_Date"]);
            var e_end_date = Convert.ToDateTime(collection["End_Date"]);
            e_class.class_id = id;
            if (string.IsNullOrEmpty(e_name))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                e_class.name = e_name;
                e_class.start_date = e_start_date;
                e_class.end_date = e_end_date;
                UpdateModel(e_class);
                data.SubmitChanges();
                return RedirectToAction("ListClasses");
            }
            return this.EditClass(id);
        }
        //-----------------------------------------
        public ActionResult DeleteClass(int id)
        {
            var d_class = data.Classes.First(m => m.class_id == id);
            return View(d_class);
        }
        [HttpPost]
        public ActionResult DeleteClass(int id, FormCollection collection)
        {
            var d_class = data.Classes.Where(m => m.class_id == id).First();
            data.Classes.DeleteOnSubmit(d_class);
            data.SubmitChanges();
            return RedirectToAction("ListClasses");
        }
    }
}