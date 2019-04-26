using System.Linq;
using System.Web.Mvc;
using DataModelClasses.DataLayer;
using Northwind.Configuration;

namespace DataModelClasses.Controllers
{
    public class CategoryController : ControllerAutoMapperBase
    {
        // GET: Product/Category
        public ActionResult Index()
        {
            // retrieve a list of all categories
            using (NorthwindEntities db = new NorthwindEntities())
            {
                return View(db.Categories.OrderBy(c => c.CategoryName).ToList());
            }
        }
    }
}