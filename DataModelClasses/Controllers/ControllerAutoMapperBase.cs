using System.Web.Mvc;
using AutoMapper;
using DataModelClasses.Models;
using DataModelClasses.DataLayer;

namespace Northwind.Configuration
{
    public abstract class ControllerAutoMapperBase : Controller
    {
        protected readonly IMapper Mapper; protected ControllerAutoMapperBase()
        {
            var config = new MapperConfiguration(x => { x.CreateMap<Customer, CustomerEdit>(); x.CreateMap<CustomerEdit, Customer>(); x.CreateMap<CustomerEdit, Customer>(); });

            Mapper = config.CreateMapper();
        }
    }
}