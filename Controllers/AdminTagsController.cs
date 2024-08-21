using Blog.MVC.Data;
using Blog.MVC.Models.Domain;
using Blog.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly BloggieDbContext _bloggieDbContext;
        public AdminTagsController(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public IActionResult Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag Domain Model
            _bloggieDbContext.Tags.Add(new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            });
            _bloggieDbContext.SaveChanges();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public IActionResult List()
        {
            var tags = _bloggieDbContext.Tags.ToList();

            return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public IActionResult Edit(Guid id)
        {
            // 1st method
            //var tag = _bloggieDbContext.Tags.Find(id);

            // 2nd method
            var tag = _bloggieDbContext.Tags.FirstOrDefault(x => x.Id == id);

            if(tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }
    }
}
