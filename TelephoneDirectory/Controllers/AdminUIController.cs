using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PagedList;
using PagedList.Mvc;
using TelephoneDirectory.Models.Entity;

namespace TelephoneDirectory.Controllers
{
    public class AdminUIController : Controller
    {
        InternShipProjectEntitiesOne InternShipProjectEntities = new InternShipProjectEntitiesOne();
        public ActionResult Index(int sayfa = 1)
        {
            var result = InternShipProjectEntities.Working.ToList().ToPagedList(sayfa, 5);
            return View(result);
        }

        [HttpGet]
        public ActionResult Login()
        {                      
            return View();
        }

        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            var result = InternShipProjectEntities.Admin.Where(i => i.UserName == admin.UserName && i.Password == admin.Password);            
            if (result != null && result.Count() > 0)
            {
                Session["UserName"] = result;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Kullanıcı adı veya şifre yanlış.";
            }
            return View();
        }

        [HttpGet]
        public ActionResult AddNewWorking()
        {
            List<SelectListItem> Values1 = (from i in InternShipProjectEntities.Department.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.DepartmentName,
                                                Value = i.DepartmentId.ToString(),
                                            }).ToList();
            ViewBag.Value1 = Values1;
            List<SelectListItem> Values2 = (from i in InternShipProjectEntities.Administrator.ToList()
                                            select new SelectListItem
                                            {
                                                Text = i.AdministratorName,
                                                Value = i.AdministratorId.ToString(),
                                            }).ToList();
            ViewBag.Value2 = Values2;
            return View();
        }

        [HttpPost]
        public ActionResult AddNewWorking(Working NewWorking)
        {
            if (NewWorking.Name != null && NewWorking.Surname != null && NewWorking.TelephoneNumber != null)
            {
                var NewWorkingDepartmentID = InternShipProjectEntities.Department.Where(i => i.DepartmentId == NewWorking.Department).FirstOrDefault();
                var NewWorkingAdministratorId = InternShipProjectEntities.Administrator.Where(i => i.AdministratorId == NewWorking.Administrator).FirstOrDefault();
                NewWorking.Department = NewWorkingDepartmentID.DepartmentId;
                NewWorking.Administrator = NewWorkingAdministratorId.AdministratorId;
                InternShipProjectEntities.Working.Add(NewWorking);
                InternShipProjectEntities.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DepartmentInformation()
        {
            var result = InternShipProjectEntities.Department.ToList();
            return View(result);
        }

        [HttpPost]
        public ActionResult DepartmentInformation(Department department)
        {
            var result = InternShipProjectEntities.Department.Where(i => i.DepartmentName == department.DepartmentName);
            if (!(result.Count() > 0))
            {
                if (department.DepartmentName != null)
                {
                    InternShipProjectEntities.Department.Add(department);
                    InternShipProjectEntities.SaveChanges();
                }
            }
            return RedirectToAction("DepartmentInformation");
        }

        public ActionResult DeleteDepartment(int Id)
        {
            var result = InternShipProjectEntities.Department.Find(Id);
            var Control = InternShipProjectEntities.Working.Where(i => i.Department1.DepartmentName == result.DepartmentName);
            if (!(Control.Count() > 0))
            {
                InternShipProjectEntities.Department.Remove(result);
                InternShipProjectEntities.SaveChanges();
            }
            else
            {
                ViewBag.Message = "Bu departmanda çalışan personeller bulunmaktadır.";
            }
            return RedirectToAction("DepartmentInformation");
        }

        public ActionResult UpdateDepartment(int Id)
        {
            var result = InternShipProjectEntities.Department.Find(Id);
            return View(result);
        }

        public ActionResult UpdateDataDepartment(Department department)
        {
            var result = InternShipProjectEntities.Department.Find(department.DepartmentId);
            result.DepartmentName = department.DepartmentName;
            InternShipProjectEntities.SaveChanges();
            return RedirectToAction("DepartmentInformation");
        }

        public ActionResult Delete(int Id)
        {
            var Working = InternShipProjectEntities.Working.Find(Id);
            InternShipProjectEntities.Working.Remove(Working);
            InternShipProjectEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Update(int Id)
        {
            var result = InternShipProjectEntities.Working.Find(Id);
            return View(result);
        }

        public ActionResult UpdateData(Working working)
        {
            var value = InternShipProjectEntities.Working.Find(working.WorkingId);
            value.Name = working.Name;
            value.Surname = working.Surname;
            value.TelephoneNumber = working.TelephoneNumber;
            value.Department1.DepartmentName = working.Department1.DepartmentName;
            InternShipProjectEntities.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AdminOperation()
        {
            var result = InternShipProjectEntities.Admin.ToList();
            return View(result);
        }

        [HttpPost]
        public ActionResult AdminOperation(Admin admin)
        {
            var result = InternShipProjectEntities.Admin.Where(i => i.UserName == admin.UserName);
            if (admin.UserName != null && admin.Password != null)
            {
                if (!(result.Count() > 0))
                {
                    InternShipProjectEntities.Admin.Add(admin);
                    InternShipProjectEntities.SaveChanges();
                }
            }
            return RedirectToAction("AdminOperation");
        }

        public ActionResult AdminDelete(int Id)
        {
            var admin = InternShipProjectEntities.Admin.Find(Id);
            if (admin != null)
            {
                InternShipProjectEntities.Admin.Remove(admin);
                InternShipProjectEntities.SaveChanges();
            }
            return RedirectToAction("AdminOperation");
        }       

        public ActionResult AdminUpdate(int Id)
        {
            var result = InternShipProjectEntities.Admin.Find(Id);
            return View(result);
        }

        public ActionResult AdminDataUpdate(Admin admin)
        {
            var value = InternShipProjectEntities.Admin.Find(admin.AdminId);
            value.UserName = admin.UserName;
            value.Password = admin.Password;
            InternShipProjectEntities.SaveChanges();
            return RedirectToAction("AdminOperation");
        }

        [HttpGet]
        public ActionResult AdministratorOperation()
        {
            var result = InternShipProjectEntities.Administrator.ToList();
            return View(result);
        }

        [HttpPost]
        public ActionResult AdministratorOperation(Administrator administrator)
        {
            var result = InternShipProjectEntities.Administrator.Where(i => i.AdministratorName == administrator.AdministratorName);
            if (!(result.Count() > 0))
            {
                InternShipProjectEntities.Administrator.Add(administrator);
                InternShipProjectEntities.SaveChanges();
            }
            return RedirectToAction("AdministratorOperation");
        }

        public ActionResult AdministratorDelete(int Id)
        {
            var administrator = InternShipProjectEntities.Administrator.Find(Id);
            var control = InternShipProjectEntities.Working.Where(i => i.Administrator1.AdministratorName == administrator.AdministratorName);
            if (!(control.Count()>0))
            {
                if (administrator != null)
                {
                    InternShipProjectEntities.Administrator.Remove(administrator);
                    InternShipProjectEntities.SaveChanges();
                }
            }
            else
            {
                ViewBag.Message = "Bu yönetici altında çalışanlar mevcut silme işlemi başarısız";
            }
            return RedirectToAction("AdministratorOperation");
        }

        public ActionResult AdministratorUpdate(int Id)
        {
            var result = InternShipProjectEntities.Administrator.Find(Id);
            return View(result);
        }

        public ActionResult AdministratorDataUpdate(Administrator administrator)
        {
            var value = InternShipProjectEntities.Administrator.Find(administrator.AdministratorId);
            if (value!=null)
            {
                value.AdministratorName = administrator.AdministratorName;
                InternShipProjectEntities.SaveChanges();
            }            
            return RedirectToAction("AdministratorOperation");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Login", "AdminUI");
        }
    }
}