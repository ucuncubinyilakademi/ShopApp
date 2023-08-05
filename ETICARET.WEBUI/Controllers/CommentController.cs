using ETICARET.BLL.Abstract;
using ETICARET.ENTITY;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WEBUI.Controllers
{
    public class CommentController : Controller
    {
        private IProductService _productService;
        private ICommentService _commentService;

        public CommentController(IProductService productService, ICommentService commentService)
        {
            _commentService = commentService;
            _productService = productService;
        }
        public IActionResult ShowProductComment(int? id)
        {
            if (id == null) { return BadRequest(); }

            Product product = _productService.GetProductDetails(id.Value);

            if (product == null) { return NotFound(); }

            return View("_PartialComments",product.Comments);
        }
    }
}
