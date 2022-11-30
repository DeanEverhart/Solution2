using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Controllers
{
    public class ModelController : Controller
    {
        private readonly ProjectContext _context;

        public ModelController(ProjectContext context)
        {
            _context = context;
        }

        //GET: Model

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)

        // ********************* Previous **************************

        //public async Task<IActionResult> Index(string sortOrder, string searchString)

        {
            ViewData["Text"] = String.IsNullOrEmpty(sortOrder) ? "text_desc" : "";
            ViewData["Number"] = sortOrder == "Number" ? "number_desc" : "Number";
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            var models = from s in _context.Models
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                models = models.Where(s => s.Text.Contains(searchString));

                                     // || s.Number.Contains(searchString)
            }

            switch (sortOrder)
            {
                case "text_desc":
                    models = models.OrderByDescending(s => s.Text);
                    break;
                case "Number":
                    models = models.OrderBy(s => s.Number);
                    break;
                case "number_desc":
                    models = models.OrderByDescending(s => s.Number);
                    break;
                default:
                    models = models.OrderBy(s => s.Text);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Model>.CreateAsync(models.AsNoTracking(), pageNumber ?? 1, pageSize));

            // **************** Previous *******************
            //
            //return View(await models.AsNoTracking().ToListAsync());

            // **************** Previous *******************
            //
            //public async Task<IActionResult> Index()
            //{
            //    return View(await _context.Model.ToListAsync());
            //}      
        }



        // GET: Model/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.ID == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Model/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Model/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Text,Number,Bool,Date")] Model model)
        {
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Model/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Model/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Text,Number,Bool,Date")] Model model)
        {
            if (id != model.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModelExists(model.ID))
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
            return View(model);
        }

        // GET: Model/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Models == null)
            {
                return NotFound();
            }

            var model = await _context.Models
                .FirstOrDefaultAsync(m => m.ID == id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // POST: Model/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Models == null)
            {
                return Problem("Entity set 'ProjectContext.Model'  is null.");
            }
            var model = await _context.Models.FindAsync(id);
            if (model != null)
            {
                _context.Models.Remove(model);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModelExists(int id)
        {
          return _context.Models.Any(e => e.ID == id);
        }
    }
}
