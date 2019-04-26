using System;
using System.Collections.Generic;
using System.Web.Mvc;
using DataModelClasses.Models;
using Northwind.Configuration;

namespace DataModelClasses.Controllers
{
    public class OrderController : ControllerAutoMapperBase
    {
        public ActionResult Process(FormCollection form)
        {
            List<Order> orders = new List<Order>();
            Int16 qty;

            ProductContext productContext = new ProductContext();

            foreach (var key in form.AllKeys)
            {
                if (Int16.TryParse(form[key], out qty) && qty > 0)
                {
                    var p = productContext.Find(key);
                    orders.Add(new Order { Prod = p, Qty = qty });
                }
            }

            return View(orders);
        }
    }
}