using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoppingSiteASP.Models;
using Sports_Store.Domain.Abstract;

namespace ShoppingSiteASP.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository productRepo;
        public int pageSize = 4;

        public ProductController(IProductRepository productRepoParam)
        {
            productRepo = productRepoParam;
        }

        // GET: Product
        public ViewResult List(string category,int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = productRepo.Products
                        .Where(p => category == null || p.Category == category)
                        .OrderBy(p => p.ProductID)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                        productRepo.Products.Count():
                        productRepo.Products.Where(e=> e.Category == category).Count(),
                    CurrentPage = page
                },
                CurrentCategory = category
            };

            return View(model);
        }
    }
}