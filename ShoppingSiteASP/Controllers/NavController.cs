using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sports_Store.Domain.Abstract;

namespace ShoppingSiteASP.Controllers
{
    public class NavController : Controller
    {
        private IProductRepository productrepo;
        public NavController(IProductRepository productRepoParam)
        {
            productrepo = productRepoParam;
        }
        // GET: Nav
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = productrepo.Products
                .Select(p => p.Category)
                .Distinct()
                .OrderBy(x => x);

            return PartialView(categories);
        }
    }
}