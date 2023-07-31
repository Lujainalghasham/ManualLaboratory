using ManualLaboratory.Data;
using ManualLaboratory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManualLaboratory.Controllers
{
    public class ManagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ManagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var limitationCountResult = _context.Manage.Where(x => x.Name == "limitationDays").FirstOrDefault();
            return View(limitationCountResult == null ? 0 : limitationCountResult.Value);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int limitationDays)
        {
            var limitationDaysObject = _context.Manage.Where(x => x.Name == "limitationDays").FirstOrDefault();
            if (limitationDaysObject == null)
            {
                limitationDaysObject = new Manage();
                limitationDaysObject.Name = "limitationDays";
                limitationDaysObject.Value = limitationDays;
                _context.Add(limitationDaysObject);
            }
            else
            {
                limitationDaysObject.Value = limitationDays;
            }
            _context.SaveChanges();
            //return RedirectToAction(nameof(Index), "Requests");
            return View(limitationDays);
        }
    }
}
