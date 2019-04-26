using System;
using System.Linq;
using System.Web.Mvc;
using DataModelClasses.DataLayer;
using Northwind.Configuration;

namespace DataModelClasses.Controllers
{
    public class DiscountController : ControllerAutoMapperBase
    {
        // GET: Product/Discount
        public ActionResult Index()
        {
            // retrieve a list of discounts 
            using (NorthwindEntities db = new NorthwindEntities())
            {
                // Filter by date
                DateTime now = DateTime.Now;
                return View(db.Discounts.Where(s => s.StartTime <= now && s.EndTime > now).ToList());
            }
        }
    }
}