using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebTrungTamNgoaiNgu.Models;

namespace WebTrungTamNgoaiNgu.Controllers
{
    public class StudentController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        [HttpGet]
        // GET: NguoiDung
        public bool ValidatePassword(string password)
        {

            Regex regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");

            return regex.IsMatch(password);
        }
        public static bool ValidateVNPhoneNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Replace("+84", "0");
            Regex regex = new
            Regex(@"^(0)(86|96|97|98|32|33|34|35|36|37|38|39|91|94|83|84|85|81|82|90|93|70|79|77|76|78|92|56|58|99|59|55|87)\d{7}$");
            return regex.IsMatch(phoneNumber);
        }
        public bool ValidateEmail(string email)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return regex.IsMatch(email);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, Student _student)
        {
            var name = collection["Name"];
            var birthday = Convert.ToDateTime(collection["Birthday"]);
            var email = collection["Email"];
            var phone = collection["Phone"];
            var street = collection["Street"];
            var login = collection["Login"];
            var password = collection["Password"];
            var re_enter_password = collection["ReEnter_Password"];
            //var idcv = collection["IdCV"];
            var checkEmail = data.Students.FirstOrDefault(x => x.email == email);
            var checkLogin = data.Students.FirstOrDefault(x => x.login == login);
            var checkPhone = data.Students.FirstOrDefault(x => x.phone == phone);
            if (String.IsNullOrEmpty(re_enter_password))
            {
                TempData["Error"] = "Phải nhập mật khẩu xác nhận!";
            }
            else
            {
                if (!password.Equals(re_enter_password))
                {
                    TempData["Error"] = "Mật khẩu và mật khẩu xác nhận phải giống nhau";
                }
                else if (ValidatePassword(password) == false)
                {
                    TempData["Error"] = "Mật khẩu ít nhất 8 kí tự (Có 1 chữ hoa + thường + số + kí tự đặc biệt)";
                }
                else if (checkEmail != null)
                {
                    TempData["Error"] = "Email đã tồn tại";
                }
                else if (checkLogin != null)
                {
                    TempData["Error"] = "Tên đăng nhập đã tồn tại";
                }
                else if (ValidateVNPhoneNumber(phone) == false)
                {
                    TempData["Error"] = "Số điện thoại không hợp lệ";
                }
                else if (checkPhone != null)
                {
                    TempData["Error"] = "Số điện thoại đã tồn tại";
                }
                else if (ValidateEmail(email) == false)
                {
                    TempData["Error"] = "Email không hợp lệ";
                }

                else
                {
                    _student.name = name;
                    _student.birthday = birthday;
                    _student.email = email;
                    _student.phone = phone;
                    _student.street = street;
                    _student.login = login;
                    _student.password = password;
                    //_student.IdCV = 4;
                    data.Students.InsertOnSubmit(_student);
                    data.SubmitChanges();
                    return RedirectToAction("Login");
                }
            }
            return this.Register();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        // GET: NguoiDung
        public ActionResult Login(FormCollection collection)
        {
            var login = collection["Login"];
            var password = collection["Password"];
            Student _student = data.Students.SingleOrDefault(n => n.login == login & n.password == password);
            if (_student != null)
            {
                Session["Login"] = _student;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "Tên dăng nhập hoặc mật khẩu không đúng";
            }
            return this.Login();

        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("Login");
            return RedirectToAction("Login", "Student");
        }
    }
}