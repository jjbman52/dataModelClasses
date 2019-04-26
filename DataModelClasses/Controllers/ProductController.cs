using System.Linq;
using System.Net;
using System.Web.Mvc;
using DataModelClasses.DataLayer;
using Northwind.Configuration;

namespace DataModelClasses.Controllers
{
    public class ProductController : ControllerAutoMapperBase
    {
        public ActionResult ProductSearch()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(int? id, string SearchString)
        {
            ViewBag.Filter = "Products";

            using (var db = new NorthwindEntities())
            {
                var products = db.Products.Where(p => !p.Discontinued);
                
                if (!string.IsNullOrEmpty(SearchString))
                {
                    ViewBag.Filter += " Matching " + SearchString;
                    products = products.Where(p => p.ProductName.Contains(SearchString));
                }
                
                if (id != null)
                {
                    products = products.Where(p => p.ProductID == id);
                }

                // retrieve list of products
                return View(products.OrderBy(p => p.ProductName).ToList());
            }
        }

        public ActionResult Products()
        {
            using (var db = new NorthwindEntities())
            { 

                return View(db.Products.Where(p => !p.Discontinued).ToList());
            }
        }

        public JsonResult SortProducts(string order)
        {
            using (var db = new NorthwindEntities())
            {
                var Products = db.Products.Where(p => p.Discontinued == false);

                if (order == "ascending")
                {
                    Products = Products.OrderBy(p => p.UnitsInStock);
                }
                if (order == "decending")
                {
                    Products = Products.OrderByDescending(p => p.UnitsInStock);

                }

                var ProductDTOs = (from p in Products select new
                {
                    p.ProductID,
                    p.ProductName,
                    p.QuantityPerUnit,
                    p.UnitPrice,
                    p.UnitsInStock
                }).ToList();
                
                return Json(ProductDTOs, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProductByCategory(int? id, string SearchString)
        {
            ViewBag.Filter = "Products";

            using (var db = new NorthwindEntities())
            {
                var products = db.Products
                    .Where(p => !p.Discontinued);

                // apply searchstring
                if (!string.IsNullOrEmpty(SearchString))
                {
                    ViewBag.Filter += " Matching " + SearchString;
                    products = products.Where(p => p.ProductName.Contains(SearchString));
                }

                // apply id
                if (id != null)
                {
                    ViewBag.Filter += " in Category " + db.Categories.Find(id)?.CategoryName;
                    products = products.Where(p => p.CategoryID == id);
                }

                // retrieve list of products
                return View("Index", products.OrderBy(p => p.ProductName).ToList());
            }
        }
    }
}