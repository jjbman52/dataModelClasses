using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataModelClasses.DataLayer;
using DataModelClasses.Models;
using DataModelClasses.Models.Security;
using Northwind.Configuration;

namespace DataModelClasses.Controllers
{
    public class CustomerController : ControllerAutoMapperBase
    {
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Account()
        {
            if (Request.Cookies["role"].Value != "customer")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CustomerID = UserAccount.GetUserID();

            using (var db = new NorthwindEntities())
            {
                Customer customer = db.Customers.Find(UserAccount.GetUserID());

                CustomerEdit EditCustomer = new CustomerEdit()
                {
                    CompanyName = customer.CompanyName,
                    ContactName = customer.ContactName,
                    ContactTitle = customer.ContactTitle,
                    Address = customer.Address,
                    City = customer.City,
                    Region = customer.Region,
                    PostalCode = customer.PostalCode,
                    Country = customer.Country,
                    Phone = customer.Phone,
                    Fax = customer.Fax,
                    Email = customer.Email
                };

                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer)
        {
            using (var db = new NorthwindEntities())
            {
                if (db.Customers.Any(c => c.CompanyName == customer.CompanyName))
                {
                    return View();
                }
                customer.UserGuid = System.Guid.NewGuid();
                customer.Password = UserAccount.HashSHA1(customer.Password + customer.UserGuid);
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult SignIn()
        {
            using (var db = new NorthwindEntities())
            {
                ViewBag.CustomerId = new SelectList(db.Customers.OrderBy(c => c.CompanyName), "CustomerID", "CompanyName").ToList();
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn([Bind(Include = "CustomerId,Password")] CustomerSignIn customerSignIn, string ReturnUrl)
        {

            if (ModelState.IsValid)
            {
                using (var db = new NorthwindEntities())
                {
                    Customer customer = db.Customers.Find(customerSignIn.CustomerId);

                    var pass = UserAccount.HashSHA1(customerSignIn.Password + customer.UserGuid);

                    if (pass == customer.Password)
                    {
                        FormsAuthentication.SetAuthCookie(customer.CustomerID.ToString(), false);

                        HttpCookie cookie = new HttpCookie("role");

                        if (customer.Role == 0)
                        {
                            cookie.Value = "customer";
                        }
                        else if (customer.Role == 1)
                        {
                            cookie.Value = "vendor";
                        }

                        Response.Cookies.Add(cookie);

                        if (ReturnUrl != null)
                        {
                            return Redirect(ReturnUrl);
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Incorrect password");
                    }

                    ViewBag.CustomerId = new SelectList(db.Customers.OrderBy(c => c.CompanyName), "CustomerID", "CompanyName").ToList();
                }
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        // POST: Customer/Account
        [ValidateAntiForgeryToken]
        public ActionResult Account([Bind(Include = "CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,Email")] CustomerEdit UpdatedCustomer)
        {
            // For future version, make sure that an authenticated user is a customer
            if (Request.Cookies["role"].Value != "customer")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var db = new NorthwindEntities())
            {
                Customer customer = db.Customers.Find(UserAccount.GetUserID());
                //customer.CompanyName = UpdatedCustomer.CompanyName;
                // if the customer is changing their CompanyName
                if (customer.CompanyName.ToLower() != UpdatedCustomer.CompanyName.ToLower())
                {
                    // Ensure that the CompanyName is unique
                    if (db.Customers.Any(c => c.CompanyName == UpdatedCustomer.CompanyName))
                    {
                        // duplicate CompanyName
                        return View(UpdatedCustomer);
                    }
                    customer.CompanyName = UpdatedCustomer.CompanyName;
                }
                customer.Address = UpdatedCustomer.Address;
                customer.City = UpdatedCustomer.City;
                customer.ContactName = UpdatedCustomer.ContactName;
                customer.ContactTitle = UpdatedCustomer.ContactTitle;
                customer.Country = UpdatedCustomer.Country;
                customer.Email = UpdatedCustomer.Email;
                customer.Fax = UpdatedCustomer.Fax;
                customer.Phone = UpdatedCustomer.Phone;
                customer.PostalCode = UpdatedCustomer.PostalCode;
                customer.Region = UpdatedCustomer.Region;

                db.SaveChanges();
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            }
        }
    }
}