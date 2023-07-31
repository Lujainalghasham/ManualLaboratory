using ManualLaboratory.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManualLaboratory.Models;
using Microsoft.AspNetCore.Authorization;

namespace ManualLaboratory.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Recep,Admin")]
        public async Task<IActionResult> Index(string? college, string? studentstatus)
        {
            if (!string.IsNullOrEmpty(college) && !string.IsNullOrEmpty(studentstatus))
            {
                return View(await _context.Request.Where(r => r.College == college && r.StudentStatus == studentstatus).ToListAsync());
            }
            else if (!string.IsNullOrEmpty(college) || !string.IsNullOrEmpty(studentstatus))
            {
                return View(await _context.Request.Where(r => r.College == college || r.StudentStatus == studentstatus).ToListAsync());
            }
            else
            {
                return _context.Request != null ?
                    View(await _context.Request.ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.Requests' is null");
            }
        }

        //GET
        public IActionResult Create()
        {
            var manage = _context.Manage.Where(x => x.Name == "limitationDays").FirstOrDefault();
            if (manage is null)
            {
                ViewBag.ErrorMessage = "You Need to Set the Limit in Manage Page";
                return View();
            }
            var limitDays = manage.Value;
            var dateTo = DateTime.Now.AddDays(30);
            List<DateTime> avalibleDates = new List<DateTime>();
            for (var date = DateTime.Now; date <= dateTo; date = date.AddDays(1))
            {
                if (date.DayOfWeek.ToString() == "Friday" || date.DayOfWeek.ToString() == "Saturday")
                {
                    continue;
                }
                var requestCount = _context.Request.Where(x => x.DateSelected.Date == date.Date).Count();
                if (requestCount >= limitDays)
                {
                    continue;
                }
                avalibleDates.Add(date);
            }
            ViewBag.AvalibleDates = avalibleDates;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NationalId,UniversityNo,StudentStatus,College,FirstNameEn,FatherNameEn,GrandfatherNameEn,FamilyNameEn,FirstNameAr,FatherNameAr,GrandfatherNameAr,FamilyNameAr,Email,PhoneNo,BirthDate,MidecalfileNo,DateSelected")] Request request)
        {
            var manage = _context.Manage.Where(x => x.Name == "limitationDays").FirstOrDefault();
            if (manage is null)
            {
                ViewBag.ErrorMessage = "You Need to Set the Limit in Manage Page";
                return View();
            }
            var limitDays = manage.Value;
            var requestCount = _context.Request.Where(x => x.DateSelected == request.DateSelected).Count();
            if (requestCount >= limitDays)
            {
                ViewBag.ErrorMessage = "Sorry, The Limit of Request for this day is Reached";
                return View();
            }

            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Message");
            }
            return View(request);
        }
        public IActionResult Message()
        {
            return View();
        }
    }
}
