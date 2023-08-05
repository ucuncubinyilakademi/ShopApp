using ETICARET.ENTITY;

namespace ETICARET.WEBUI.Models
{
    public class CategoryListViewModel
    {
        public string SelectedCategory { get; set; }
        public List<Category> Categories { get; set; }
    }
}
