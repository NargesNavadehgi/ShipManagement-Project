using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipManagement.Data;
using ShipManagement.Models;
using System.Diagnostics;
using System.Globalization;

namespace ShipManagementSystem.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ShipmentController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Ships()
        {
            var ships = await _context.Ships.ToListAsync();
            return View(ships);
        }
        public async Task<IActionResult> Create()
        {
            await PopulateViewBags();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Shipment shipment)
        {
            try
            {
                // ابتدا تاریخ ثبت را تنظیم کنید
                shipment.RegistrationDate = DateTime.Now;

                // سپس تاریخ‌های دیگر را تبدیل کنید
                if (!string.IsNullOrEmpty(shipment.ArrivalDateString))
                {
                    shipment.ArrivalDate = ConvertPersianToGregorian(shipment.ArrivalDateString);
                }
                else
                {
                    shipment.ArrivalDate = null;
                }

                if (!string.IsNullOrEmpty(shipment.UnloadingStartDateString))
                {
                    shipment.UnloadingStartDate = ConvertPersianToGregorian(shipment.UnloadingStartDateString);
                }
                else
                {
                    shipment.UnloadingStartDate = null;
                }

                if (!string.IsNullOrEmpty(shipment.UnloadingCompletionDateString))
                {
                    shipment.UnloadingCompletionDate = ConvertPersianToGregorian(shipment.UnloadingCompletionDateString);
                }
                else
                {
                    shipment.UnloadingCompletionDate = null;
                }

                if (!string.IsNullOrEmpty(shipment.DepartureDateString))
                {
                    shipment.DepartureDate = ConvertPersianToGregorian(shipment.DepartureDateString);
                }
                else
                {
                    shipment.DepartureDate = null;
                }

                // حالا مدل را validate کنید
                if (ModelState.IsValid)
                {
                    _context.Add(shipment);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "اطلاعات کشتی با موفقیت ثبت شد";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "خطا در ذخیره اطلاعات: " + ex.Message);
            }

            await PopulateViewBags();

            // اضافه کردن مجدد Operation Types در صورت خطا
            ViewBag.OperationTypes = new List<SelectListItem>
    {
        new SelectListItem { Value = "تخلیه", Text = "تخلیه" },
        new SelectListItem { Value = "حمل", Text = "حمل" },
        new SelectListItem { Value = "نظافت", Text = "نظافت" },
        new SelectListItem { Value = "بارگیری", Text = "بارگیری" },
        new SelectListItem { Value = "انبارداری", Text = "انبارداری" }
    };

            return View(shipment);
        }
        private async Task PopulateViewBags()
        {
            try
            {
                ViewBag.ShipNames = new SelectList(await _context.Ships.Select(s => s.Name).Distinct().ToListAsync());
                ViewBag.CargoOwners = new SelectList(await _context.CargoOwners.Select(co => co.Name).ToListAsync());
                ViewBag.ShippingAgents = new SelectList(await _context.ShippingAgents.Select(sa => sa.Name).ToListAsync());
                ViewBag.Dockworkers = new SelectList(await _context.Dockworkers.Select(d => d.Name).ToListAsync());
                ViewBag.CargoTypes = new SelectList(await _context.CargoTypes.Select(d => d.Name).ToListAsync());


                ViewBag.PaymentMethods = new SelectList(new List<string>
        {
            "نقدی", "فیش", "سایر"
        });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error populating ViewBags: {ex.Message}");

                // Fallback به لیست‌های پیش‌فرض
                ViewBag.ShipNames = new SelectList(new List<string> { "کشتی ۱", "کشتی ۲", "کشتی ۳" });
                ViewBag.CargoOwners = new SelectList(new List<string> { "مالک ۱", "مالک ۲", "مالک ۳" });
                ViewBag.ShippingAgents = new SelectList(new List<string> { "کشتیرانی ایران", "کشتیرانی خلیج فارس" });
                ViewBag.Dockworkers = new SelectList(new List<string> { "خنکار ۱", "خنکار ۲", "خنکار ۳" });
                ViewBag.CargoTypes = new SelectList(new List<string> { "کالای عمومی", "کالای حساس", "کالای خطرناک" });
                ViewBag.PaymentMethods = new SelectList(new List<string> { "نقدی", "فیش", "سایر" });
            }
        }

        private DateTime? ConvertPersianToGregorian(string persianDate)
        {
            try
            {
                if (string.IsNullOrEmpty(persianDate))
                    return null;

                // حذف فاصله‌های اضافی
                persianDate = persianDate.Trim();

                // اگر رشته خالی است
                if (string.IsNullOrEmpty(persianDate))
                    return null;

                // جدا کردن تاریخ و زمان
                var parts = persianDate.Split(' ');
                var datePart = parts[0];

                // جدا کردن سال، ماه، روز
                var dateParts = datePart.Split('/');
                if (dateParts.Length != 3)
                    return null;

                int year = int.Parse(dateParts[0]);
                int month = int.Parse(dateParts[1]);
                int day = int.Parse(dateParts[2]);

                var pc = new PersianCalendar();
                return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            {
                return null;
            }
        }
        // GET: Shipment/Success
        public IActionResult Success()
        {
            return View();
        }

        // GET: Shipment
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shipments.ToListAsync());
        }

        public async Task<IActionResult> Report()
        {
            var shipments = await _context.Shipments.ToListAsync();
            return View("~/Views/Reports/ShipmentsReport.cshtml", shipments);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .FirstOrDefaultAsync(m => m.Id == id);

            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }
        // GET: Shipment/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            // پر کردن ViewBag برای dropdownها
            await PopulateViewBags();

            // تبدیل تاریخ‌ها به رشته شمسی برای نمایش در فرم
            shipment.ArrivalDateString = ConvertToPersianDateString(shipment.ArrivalDate);
            shipment.UnloadingStartDateString = ConvertToPersianDateString(shipment.UnloadingStartDate);
            shipment.UnloadingCompletionDateString = ConvertToPersianDateString(shipment.UnloadingCompletionDate);
            shipment.DepartureDateString = ConvertToPersianDateString(shipment.DepartureDate);

            return View(shipment);
        }

        // POST: Shipment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShipName,ShippingAgent,ArrivalDateString,Dockworker,CargoWeight,CargoType,CargoOwner,Tariff,AddedValue,TotalAmount,PaymentMethod,EquivalentAmount,OtherPaymentMethod,UnloadingStartDateString,UnloadingCompletionDateString,UnloadedAmount,UnloadingPermitNumber,VoyageNumber,DepartureDateString")] Shipment shipment)
        {
            if (id != shipment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // تبدیل تاریخ‌های شمسی به میلادی
                    shipment.ArrivalDate = ConvertPersianStringToDate(shipment.ArrivalDateString);
                    shipment.UnloadingStartDate = ConvertPersianStringToDate(shipment.UnloadingStartDateString);
                    shipment.UnloadingCompletionDate = ConvertPersianStringToDate(shipment.UnloadingCompletionDateString);
                    shipment.DepartureDate = ConvertPersianStringToDate(shipment.DepartureDateString);

                    // تاریخ ثبت تغییر نمی‌کند
                    var existingShipment = await _context.Shipments.AsNoTracking()
                        .FirstOrDefaultAsync(s => s.Id == id);
                    if (existingShipment != null)
                    {
                        shipment.RegistrationDate = existingShipment.RegistrationDate;
                    }

                    _context.Update(shipment);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "اطلاعات کشتی با موفقیت ویرایش شد.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            // اگر مدل معتبر نبود، مجدداً ViewBagها را پر کنید
            await PopulateViewBags();
            return View(shipment);
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipments.Any(e => e.Id == id);
        }

        private string ConvertToPersianDateString(DateTime? date)
        {
            if (!date.HasValue)
                return string.Empty;

            var pc = new System.Globalization.PersianCalendar();
            return $"{pc.GetYear(date.Value)}/{pc.GetMonth(date.Value):00}/{pc.GetDayOfMonth(date.Value):00}";
        }

        private DateTime? ConvertPersianStringToDate(string persianDate)
        {
            if (string.IsNullOrEmpty(persianDate))
                return null;

            try
            {
                var parts = persianDate.Split('/');
                if (parts.Length < 3) return null;

                var pc = new System.Globalization.PersianCalendar();
                int year = int.Parse(parts[0]);
                int month = int.Parse(parts[1]);
                int day = int.Parse(parts[2]);

                return pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            }
            catch
            {
                return null;
            }
        }
     

    

    }
}