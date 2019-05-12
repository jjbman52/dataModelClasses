using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataModelClasses.DataLayer;
using DataModelClasses.Models;
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

        [Authorize]
        public ActionResult Products()
        {
            using (var db = new NorthwindEntities())
            {

                return View(db.Products.Where(p => !p.Discontinued).ToList());
            }
        }

        [Authorize]
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

                var ProductDTOs = (from p in Products
                                   select new
                                   {
                                       p.ProductID,
                                       p.ProductName,
                                       p.QuantityPerUnit,
                                       p.UnitPrice,
                                       p.UnitsInStock,
                                       p.UnitsOnOrder
                                   }).ToList();

                return Json(ProductDTOs, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
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
        
        [Authorize]
        public ActionResult Order(ProductDTO productDTO)
        {
                using (var db = new NorthwindEntities())
                {
                    var product = db.Products.SingleOrDefault(p => p.ProductID == productDTO.ProductID);

                    product.UnitsOnOrder = productDTO.Quantity;

                    db.SaveChanges();

                    return Json(productDTO, JsonRequestBehavior.AllowGet);
                }
        }
    }
}