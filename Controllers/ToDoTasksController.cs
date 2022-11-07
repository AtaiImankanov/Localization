using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using lavAspMvclast.Models;
using lavAspMvclast.Enums;
using lavAspMvclast.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using lavAspMvclast.Services;

namespace lavAspMvclast.Controllers
{
    public class ToDoTasksController : Controller
    {
        private readonly MobileContext _context;
        private readonly UserManager<User> _userManager;
        private IMemoryCache cache;
        private TaskService taskService;
        public ToDoTasksController(MobileContext context, UserManager<User> userManager, IMemoryCache memoryCache, TaskService _taskService)
        {
            taskService = _taskService;
            _context = context;
            _userManager = userManager;
            cache = memoryCache;
        }

        // GET: ToDoTasks
        public async Task<IActionResult> Index( SortState sortOrder,DateTime From,DateTime To,string Name,string WordInDescription, int Priority, int Status  ,int page = 1) {
            IQueryable<ToDoTask> tasks = _context.ToDoTasks;
            DateTime tNull= new DateTime(0001,01,01,00,00,00);
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            int idUser = Convert.ToInt32(_userManager.GetUserId(currentUser));
            var user  = _userManager.Users.FirstOrDefault(x => x.Id == idUser);
            ViewBag.UserId = idUser;
            int adminIdUser = 0;
            var roles =await  _userManager.IsInRoleAsync(user, "admin");
            if (roles == true)
            {
                adminIdUser = 1;
            }
            ViewBag.AdminUserId = adminIdUser;
            if (From != tNull && From != null)
            { 
                tasks = tasks.Where(t => t.DateCreated > From);
            }
            if (To != tNull && To != null)
            {
                tasks = tasks.Where(t => t.DateCreated < To);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                tasks = tasks.Where(t => t.Name == Name);
            }
            if (!string.IsNullOrEmpty(WordInDescription))
            {
                tasks = tasks.Where(t => t.Description.Contains(WordInDescription));
            }
            if (Priority!=0)
            {
                tasks = tasks.Where(t => t.Priority == Priority);
            }
            if (Status != 0)
            {
                tasks = tasks.Where(t => t.Status == Status);
            }

            ViewBag.NameSort=sortOrder==SortState.NameAsc ?SortState.NameDesc :SortState.NameAsc;
       ViewBag.DateSort =sortOrder==SortState.DateAsc ? SortState.DateDesc : SortState.DateAsc;
        ViewBag.StatusSort =sortOrder==SortState.StatusAsc ? SortState.StatusDesc : SortState.StatusAsc;
        ViewBag.PrioritySort =sortOrder==SortState.PriorityAsc ? SortState.PriorityDesc : SortState.PriorityAsc;
            switch (sortOrder)
            {
                case SortState.NameAsc:
                    tasks = tasks.OrderBy(t => t.Name);
                    break;
                case SortState.NameDesc:
                    tasks = tasks.OrderByDescending(t => t.Name);
                    break;
                case SortState.DateAsc:
                    tasks = tasks.OrderBy(t => t.DateCreated);
                    break;
                case SortState.DateDesc:
                    tasks = tasks.OrderByDescending(t => t.DateCreated);
                    break;
                case SortState.StatusAsc:
                    tasks = tasks.OrderBy(t => t.Status);
                    break;
                case SortState.StatusDesc:
                    tasks = tasks.OrderByDescending(t => t.Status);
                    break;
                case SortState.PriorityAsc:
                    tasks = tasks.OrderBy(t => t.Priority);
                    break;
                case SortState.PriorityDesc:
                    tasks = tasks.OrderByDescending(t => t.Priority);
                    break;

            }

            var test = tasks.ToList();
            int pagesize = 10;
            var count = _context.ToDoTasks.Count();
            var items = tasks.Skip((page - 1) * pagesize).Take(pagesize).ToList();
            var pvm = new PageViewModel(count, page, pagesize);
            var model = new TaskPageModel
            {
                ToDoTasks = items,
                PageViewModel = pvm
            };
            return View(model);
        }

        // GET: ToDoTasks/Details/5
      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ToDoTask toDoTask = await taskService.GetUser(id);

            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        // GET: ToDoTasks/Create
     
        public IActionResult Create()
        {

            return View();
        }
        public IActionResult Home()
        {

            return View();
        }

        // POST: ToDoTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
   
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Priority,DateCreated,DateClosed,Status")] ToDoTask toDoTask)
        {
            if (ModelState.IsValid)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var id = _userManager.GetUserId(currentUser);
                toDoTask.CreaterId = Convert.ToInt32(id);
                toDoTask.Status = 1;
                toDoTask.DateCreated= DateTime.Now;
                
                _context.Add(toDoTask);
                await _context.SaveChangesAsync();
                taskService.AddUser(toDoTask);
                return RedirectToAction(nameof(Index));
            }
            return View(toDoTask);
        }

        // GET: ToDoTasks/Edit/5

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ToDoTask toDoTask = await taskService.GetUser(id);

            if (toDoTask == null)
            {
                return NotFound();
            }
            return View(toDoTask);
        }

        // POST: ToDoTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Priority,DateCreated,DateClosed,Status")] ToDoTask toDoTask)
        {
            if (id != toDoTask.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoTaskExists(toDoTask.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(toDoTask);
        }

        // GET: ToDoTasks/Delete/5

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoTask = await _context.ToDoTasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoTask == null)
            {
                return NotFound();
            }

            return View(toDoTask);
        }

        // POST: ToDoTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoTask = await _context.ToDoTasks.FindAsync(id);
            _context.ToDoTasks.Remove(toDoTask);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoTaskExists(int id)
        {
            return _context.ToDoTasks.Any(e => e.Id == id);
        }

        public IActionResult Close(int id)
        {
            var toDoTask =_context.ToDoTasks.Find(id);
            toDoTask.Status = 3;
            _context.ToDoTasks.Update(toDoTask);
            _context.SaveChanges();
            return Redirect("/ToDoTasks/Index");
        }

        public IActionResult Open(int id)
        {
            var toDoTask = _context.ToDoTasks.Find(id);
            toDoTask.Status = 2;
            _context.ToDoTasks.Update(toDoTask);
            _context.SaveChanges();
            return Redirect("/ToDoTasks/Index");
        }

        public IActionResult Take(int id)
        {
            var toDoTask = _context.ToDoTasks.Find(id);
            toDoTask.Status = 4;

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            var idUser = _userManager.GetUserId(currentUser);
            toDoTask.CompliterId = Convert.ToInt32(idUser);

            _context.ToDoTasks.Update(toDoTask); 
            _context.SaveChanges();
            return Redirect("/ToDoTasks/Index");
        }

        public IActionResult Filter(int Id)
        {
            TaskPageModel model = null;
            if(Id == 3)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var idUser = Convert.ToInt32(_userManager.GetUserId(currentUser));
                IQueryable<ToDoTask> tasks = _context.ToDoTasks;
                var tasks1 = new List<ToDoTask>();
                foreach (var task in tasks)
                {
                    if (task.CreaterId == idUser)
                    {
                        tasks1.Add(task);
                    }
                }
                model = new TaskPageModel
                {
                    ToDoTasks = tasks1
                };
            }
            if (Id == 2)
            {
                IQueryable<ToDoTask> tasks = _context.ToDoTasks;
                var tasks1 = new List<ToDoTask>();
                foreach (var task in tasks)
                {
                    if (task.Status == 2)
                    {
                        tasks1.Add(task);
                    }
                }
                 model = new TaskPageModel
                {
                    ToDoTasks = tasks1
                };
            }
            if (Id == 1)
            {
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;
                var idUser = Convert.ToInt32(_userManager.GetUserId(currentUser));
                IQueryable<ToDoTask> tasks = _context.ToDoTasks;
                var tasks1 = new List<ToDoTask>();
                foreach (var task in tasks)
                {
                    if (task.CompliterId == idUser)
                    {
                        tasks1.Add(task);
                    }
                }
                model = new TaskPageModel
                {
                    ToDoTasks = tasks1
                };
            }

            return View(model);
        }
        [AcceptVerbs("GET", "POST")]
        public bool EmailValid(string email)
        {
           foreach(var u in _userManager.Users.ToList())
            {
                if (u.Email == email)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
