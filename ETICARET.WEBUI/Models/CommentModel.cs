using ETICARET.ENTITY;

namespace ETICARET.WEBUI.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ProductID { get; set; }
        public Product Product { get; set; }
        public string UserID { get; set; }
    }
}
