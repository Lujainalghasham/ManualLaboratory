using ManualLaboratory.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ManualLaboratory.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;

namespace ManualLaboratory.Controllers
{
    public class RequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<Request> Requests = new List<Request>();   
        public RequestsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Recep,Admin")]
        public async Task<IActionResult> Index(string searchSelect, string searchLabel)
        {
            var searchelement = from c in _context.Request select c;
            if (!string.IsNullOrEmpty(searchLabel))
            {
                if (searchSelect == "College")
                {
                    searchelement = searchelement.Where(s => s.College.Contains(searchLabel));
                }
                else if (searchSelect == "Status")
                {
                    searchelement = searchelement.Where(s => s.StudentStatus.Contains(searchLabel));
                }
            }
            return View(await searchelement.ToListAsync());
            }
        [HttpGet]
        public async Task<FileResult> ExportInExcel()
        {
            var Requests = await _context.Request.ToListAsync();
            var FileName = "Requests.xlsx";
            return GenerateExcel(FileName, Requests);
        }
        private FileResult GenerateExcel(string FileName, IEnumerable<Request> requests)
        {
            DataTable dataTabel = new DataTable("Requests");
            dataTabel.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("NationalId"),
                new DataColumn("UniversityNo"),
                new DataColumn("StudentStatus"),
                new DataColumn("College"),
                new DataColumn("FirstNameEn"),
                new DataColumn("FatherNameEn"),
                new DataColumn("GrandfatherNameEn"),
                new DataColumn("FamilyNameEn"),
                new DataColumn("FirstNameAr"),
                new DataColumn("FatherNameAr"),
                new DataColumn("GrandfatherNameAr"),
                new DataColumn("FamilyNameAr"),
                new DataColumn("Email"),
                new DataColumn("PhoneNo"),
                new DataColumn("BirthDate"),
                new DataColumn("MidecalfileNo"),
                new DataColumn("DateSelected")
            });
            foreach (var Request in Requests)
            {
                dataTabel.Rows.Add(
                    Request.Id,
                    Request.NationalId,
                    Request.UniversityNo,
                    Request.StudentStatus,
                    Request.College,
                    Request.FirstNameEn,
                    Request.FatherNameEn,
                    Request.GrandfatherNameEn,
                    Request.FamilyNameEn,
                    Request.FirstNameAr,
                    Request.FatherNameAr,
                    Request.GrandfatherNameAr,
                    Request.FamilyNameAr,
                    Request.Email,
                    Request.PhoneNo,
                    Request.BirthDate,
                    Request.MidecalfileNo,
                    Request.DateSelected);
            }
            using (XLWorkbook workbook = new XLWorkbook())
            {
                workbook.Worksheets.Add(dataTabel);
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    return File(stream.ToArray(),
                         "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", FileName);
                }
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
            
            VMCollege vMCollege = new VMCollege();
            var colleges = _context.College.ToList();
            vMCollege.CollegesSelectList = new SelectList(colleges, "CollegeName", "CollegeName");
            return View(vMCollege);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NationalId,UniversityNo,StudentStatus,College,FirstNameEn,FatherNameEn,GrandfatherNameEn,FamilyNameEn,FirstNameAr,FatherNameAr,GrandfatherNameAr,FamilyNameAr,Email,PhoneNo,BirthDate,MidecalfileNo,DateSelected")] Request request)
        {
            VMCollege vMCollege = new VMCollege();
            var colleges = _context.College.ToList();
            vMCollege.CollegesSelectList = new SelectList(colleges, "CollegeName", "CollegeName");
            vMCollege.request = request;
            var manage = _context.Manage.Where(x => x.Name == "limitationDays").FirstOrDefault();
            if (manage is null)
            {
                ViewBag.ErrorMessage = "You Need to Set the Limit in Manage Page";
                return View(vMCollege);
            }
            var limitDays = manage.Value;
            var requestCount = _context.Request.Where(x => x.DateSelected == request.DateSelected).Count();
            if (requestCount >= limitDays)
            {
                ViewBag.ErrorMessage = "Sorry, The Limit of Request for this day is Reached";
                return View(vMCollege);
            }

            if (ModelState.IsValid)
            {
                _context.Add(request);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Message");
            }
            return View(vMCollege);
        }
        public IActionResult Message()
        {
            return View();
        }
    }
}
