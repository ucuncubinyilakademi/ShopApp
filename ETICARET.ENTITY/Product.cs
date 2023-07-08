using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.ENTITY
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Image> Images { get; set; }
        public decimal Price { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}
