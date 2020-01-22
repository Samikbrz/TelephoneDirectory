using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using TelephoneDirectory.Models.Entity;

namespace TelephoneDirectory.Controllers
{
    public class PublicUIController : Controller
    {
        InternShipProjectEntitiesOne InternShipProjectEntities = new InternShipProjectEntitiesOne();        
        // GET: Telephone
        public ActionResult Index(int sayfa=1)
        {
            var result=InternShipProjectEntities.Working.ToList().ToPagedList(sayfa,5);
            return View(result);
        }         
    }
}