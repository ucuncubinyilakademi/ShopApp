using ETICARET.ENTITY;
using System.ComponentModel.DataAnnotations;

namespace ETICARET.WEBUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Ürün adı boş geçilemez.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Açıklama geçilemez.")]
        [MaxLength(100)]
        public string Description { get; set; }
        public List<Image> Images { get; set; }
        public decimal Price { get; set; }
        public List<Category> SelectedCategories { get; set; }


    }
}
