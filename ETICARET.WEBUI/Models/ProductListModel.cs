using ETICARET.ENTITY;

namespace ETICARET.WEBUI.Models
{
    public class ProductListModel
    {
        public List<Product> Products { get; set; }
        public PageInfo PageModel { get; set; }
    }
    public class PageInfo
    {
        public int TotalItems { get; set; } //Toplam ürün sayısı
        public int ItemsPerPage { get; set; } // Her sayfada kaç ürün görünecek
        public int CurrentPage { get; set; } //Hangi sayfadayız
        public string CurrentCategory { get; set; } //Hangi kategorideyiz

        public int TotalPages()
        {
            return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
        }
    }
}
