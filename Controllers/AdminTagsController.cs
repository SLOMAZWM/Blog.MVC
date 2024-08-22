using Blog.MVC.Data;
using Blog.MVC.Models.Domain;
using Blog.MVC.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            // Mapping AddTagRequest to Tag Domain Model
             await _bloggieDbContext.Tags.AddAsync(new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            });
            await _bloggieDbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await _bloggieDbContext.Tags.ToListAsync();

            return View(tags);
        }

        [HttpGet]
        [ActionName("Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            // 1st method
            //var tag = _bloggieDbContext.Tags.Find(id);

            // 2nd method
            var tag = await _bloggieDbContext.Tags.FirstOrDefaultAsync(x => x.Id == id);

            if (tag != null)
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

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var existingTag = await _bloggieDbContext.Tags.FindAsync(tag.Id);

            if (existingTag != null)
            {
                existingTag.Name = tag.Name;
                existingTag.DisplayName = tag.DisplayName;

                // save changes

                await _bloggieDbContext.SaveChangesAsync();

                //Show success notification
                return RedirectToAction("Edit", new { id = editTagRequest.Id });
            }
            //Show error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var tag = await _bloggieDbContext.Tags.FindAsync(editTagRequest.Id);

            if(tag != null)
            {
                _bloggieDbContext.Tags.Remove(tag);
                await _bloggieDbContext.SaveChangesAsync();

                //Show success notification

                return RedirectToAction("List");
            }

            //Show error notification
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }
    }
}
