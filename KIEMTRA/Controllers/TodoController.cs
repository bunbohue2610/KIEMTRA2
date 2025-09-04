using KIEMTRA.Models;
using Microsoft.AspNetCore.Mvc;

namespace KIEMTRA.Controllers
{
    public class TodoController : Controller
    {
        private readonly AppDbContext _context;

        public TodoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string tag)
        {
            var items = _context.TodoItems.AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                items = items.Where(t => t.Tag == tag);
            }

            return View(items.ToList());
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            if (ModelState.IsValid)
            {
                _context.TodoItems.Add(item);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Index", _context.TodoItems.ToList());
        }

        [HttpPost]
        public IActionResult ToggleComplete(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item != null)
            {
                item.IsCompleted = !item.IsCompleted;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item != null)
            {
                _context.TodoItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
