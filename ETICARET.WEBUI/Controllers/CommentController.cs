using ETICARET.BLL.Abstract;
using ETICARET.ENTITY;
using ETICARET.WEBUI.Identity;
using ETICARET.WEBUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WEBUI.Controllers
{
    public class CommentController : Controller
    {
        private IProductService _productService;
        private ICommentService _commentService;
        private UserManager<ApplicationUser> _userManager;

        public CommentController(IProductService productService, ICommentService commentService,UserManager<ApplicationUser> userManager)
        {
            _commentService = commentService;
            _productService = productService;
            _userManager = userManager;
        }
        public IActionResult ShowProductComment(int? id)
        {
            if (id == null) { return BadRequest(); }

            Product product = _productService.GetProductDetails(id.Value);

            if (product == null) { return NotFound(); }

            return View("_PartialComments",product.Comments);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(int? productId, CommentModel model)
        {
            if (productId == null)
            {
                return BadRequest();
            }
            Product product = _productService.GetById(productId.Value);
            if (product == null)
            {
                return NotFound();
            }

            Comment comment = new Comment();
            comment.Text = model.Text;
            comment.ProductId = product.Id;
            comment.UserId = _userManager.GetUserId(User);
            comment.CreateOn = DateTime.Now;

            _commentService.Create(comment);

            return Json(new { result = true });
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(int? id,string text)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Comment comment = _commentService.GetById(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            comment.Text = text.Trim('\n').Trim(' ');
            comment.CreateOn = DateTime.Now;

            _commentService.Update(comment);

            return Json(new { result = true });
        }

        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            Comment comment = _commentService.GetById(id.Value);
            if (comment == null)
            {
                return NotFound();
            }
                       
            _commentService.Delete(comment);

            return Json(new { result = true });
        }
    }
}
