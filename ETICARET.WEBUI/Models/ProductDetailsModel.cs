using ETICARET.ENTITY;

namespace ETICARET.WEBUI.Models
{
    public class ProductDetailsModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }       
        public List<Comment> Comments { get; set; }       
    }
  
}
