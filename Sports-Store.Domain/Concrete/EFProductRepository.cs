using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sports_Store.Domain.Abstract;
using Sports_Store.Domain.Entities;

namespace Sports_Store.Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Product> Products
        {
            get
            {
                return context.Products;
            }
        }
    }
}
