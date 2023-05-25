using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTrungTamNgoaiNgu.Models;

namespace WebTrungTamNgoaiNgu.Controllers
{
    public class CourseController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        // GET: Course
        public ActionResult ListCourses()
        {
            var all_courses = from c in data.Courses select c;
            return View(all_courses);
        }

        //public ActionResult DetailCourse(int id)
        //{
        //    var d_course = data.Courses.Where(m => m.course_id == id).First();
        //    return View(d_course);
        //}

        public ActionResult CreateCourse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCourse(FormCollection collection, Course _course)
        {
            var c_lessons = Convert.ToInt32(collection["Lessons"]);
            var c_description = collection["Description"];
            var c_term = collection["Term"];

            if ((string.IsNullOrEmpty(c_lessons.ToString())) || string.IsNullOrEmpty(c_term))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                _course.lessons = c_lessons;
                _course.description = c_description.ToString();
                _course.term = c_term.ToString();
                
                data.Courses.InsertOnSubmit(_course);
                data.SubmitChanges();
                return RedirectToAction("ListCourses");
            }
            return this.CreateCourse();
        }

        public ActionResult EditCourse(int id)
        {
            var e_course = data.Courses.First(m => m.course_id == id);
            return View(e_course);
        }
        [HttpPost]
        public ActionResult EditCourse(int id, FormCollection collection)
        {
            var e_course = data.Courses.First(m => m.course_id == id);
            var e_lessons = Convert.ToInt32(collection["Lessons"]);
            var e_description = collection["Description"];
            var e_term = collection["Term"];
            e_course.course_id = id;
            if ((string.IsNullOrEmpty(e_lessons.ToString())) || string.IsNullOrEmpty(e_term))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                e_course.lessons = e_lessons;
                e_course.description = e_description;
                e_course.term = e_term;
                UpdateModel(e_course);
                data.SubmitChanges();
                return RedirectToAction("ListCourses");
            }
            return this.EditCourse(id);
        }
        //-----------------------------------------
        public ActionResult DeleteCourse(int id)
        {
            var d_course = data.Courses.First(m => m.course_id == id);
            return View(d_course);
        }
        [HttpPost]
        public ActionResult DeleteCourse(int id, FormCollection collection)
        {
            var d_course = data.Courses.Where(m => m.course_id == id).First();
            data.Courses.DeleteOnSubmit(d_course);
            data.SubmitChanges();
            return RedirectToAction("ListCourses");
        }
    }
}