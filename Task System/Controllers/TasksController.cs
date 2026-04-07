using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Task_System.Models;

namespace Task_System.Controllers
{
    public class TasksController : Controller
    {
        private static List<TaskItem> _tasks = new List<TaskItem>();
        private static int _nextId = 1;

        // Sample employee list for assignment dropdown
        private static List<string> Employees = new List<string> { "Alice", "Bob", "Charlie" };

        public IActionResult Index(string search)
        {
            var tasks = _tasks.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                tasks = tasks.Where(t =>
    t.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
    (t.AssignedTo != null && t.AssignedTo.Contains(search, StringComparison.OrdinalIgnoreCase))
);

            return View(tasks.ToList() ?? new List<TaskItem>());
        }

        public IActionResult Create()
        {
            ViewBag.Employees = Employees;
            return View(new TaskItem()); // ✅ prevents null
        }

        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees = Employees; // ✅ re-add this
                return View(task);
            }

            task.Id = _nextId++;
            _tasks.Add(task);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();

            ViewBag.Employees = Employees;
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskItem task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask == null) return NotFound();

            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority;
            existingTask.AssignedTo = task.AssignedTo;
            existingTask.Status = task.Status;
            existingTask.Progress = task.Progress;

            ViewBag.Employees = Employees; // ✅ add this if returning view again (optional safety)

            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
                _tasks.Remove(task);

            return RedirectToAction("Index");
        }

        public IActionResult Complete(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = true;
                task.Status = "Completed";
                task.Progress = 100;
            }

            return RedirectToAction("Index");
        }

        // Add comment to task
        [HttpPost]
        public IActionResult AddComment(int id, string comment)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null && !string.IsNullOrEmpty(comment))
            {
                task.Comments.Add(comment);
            }
            return RedirectToAction("Edit", new { id = id });
        }
    }
}
