using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShipManagement.Data;
using ShipManagement.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ShipManagementSystem.Controllers
{
    public class BasicDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BasicDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        // استفاده از Layout مخصوص
        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            ViewBag.Layout = "_BasicDataLayout";
            base.OnActionExecuting(context);
        }

        // مدیریت کشتی‌ها
        [HttpGet]
        public IActionResult Ships()
        {
            var ships = _context.Ships.ToList();
            return View(ships);
        }

        [HttpGet]
        public IActionResult CreateShip()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShip(Ship ship)
        {
            if (ModelState.IsValid)
            {
                _context.Ships.Add(ship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Ships));
            }
            return View(ship);
        }

        // مدیریت صاحبان کالا
        [HttpGet]
        public IActionResult CargoOwners()
        {
            var owners = _context.CargoOwners.ToList();
            return View(owners);
        }

        [HttpGet]
        public IActionResult CreateCargoOwner()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCargoOwner(CargoOwner owner)
        {
            if (ModelState.IsValid)
            {
                _context.CargoOwners.Add(owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CargoOwners));
            }
            return View(owner);
        }

        // GET: نمایش لیست انواع کالا
        public async Task<IActionResult> CargoTypes()
        {
            var cargoTypes = await _context.CargoTypes.ToListAsync();
            return View(cargoTypes);
        }

        // GET: فرم ایجاد نوع کالا
        public IActionResult CreateCargoType()
        {
            return View(new CargoType());
        }

        // POST: ایجاد نوع کالا
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCargoType(CargoType cargoType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // بررسی نام تکراری
                    bool isDuplicate = await _context.CargoTypes
                        .AnyAsync(ct => ct.Name == cargoType.Name);

                    if (isDuplicate)
                    {
                        ModelState.AddModelError("Name", "نام نوع کالا تکراری است");
                        return View(cargoType);
                    }

                    _context.CargoTypes.Add(cargoType);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"نوع کالا \"{cargoType.Name}\" با موفقیت ایجاد شد";
                    return RedirectToAction(nameof(CargoTypes));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "خطا در ایجاد نوع کالا: " + ex.Message);
                }
            }
            return View(cargoType);
        }

        // GET: ویرایش نوع کالا
        [HttpGet]
        public async Task<IActionResult> EditCargoType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoType = await _context.CargoTypes.FindAsync(id);
            if (cargoType == null)
            {
                return NotFound();
            }

            return View(cargoType);
        }

        // POST: به روزرسانی نوع کالا
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCargoType(int id, CargoType cargoType)
        {
            if (id != cargoType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // بررسی نام تکراری
                    bool isDuplicate = await _context.CargoTypes
                        .AnyAsync(ct => ct.Name == cargoType.Name && ct.Id != id);

                    if (isDuplicate)
                    {
                        ModelState.AddModelError("Name", "نام نوع کالا تکراری است");
                        return View(cargoType);
                    }

                    _context.Update(cargoType);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "ویرایش با موفقیت انجام شد";
                    return RedirectToAction(nameof(CargoTypes));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoTypeExists(cargoType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "خطای همزمانی در ویرایش اطلاعات");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "خطا در ویرایش اطلاعات: " + ex.Message);
                }
            }

            return View(cargoType);
        }

        // GET: حذف نوع کالا
        public async Task<IActionResult> DeleteCargoType(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoType = await _context.CargoTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cargoType == null)
            {
                return NotFound();
            }

            return View(cargoType);
        }

        // POST: تایید حذف نوع کالا
        [HttpPost, ActionName("DeleteCargoType")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCargoTypeConfirmed(int id)
        {
            try
            {
                var cargoType = await _context.CargoTypes.FindAsync(id);
                if (cargoType != null)
                {
                    _context.CargoTypes.Remove(cargoType);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"نوع کالا \"{cargoType.Name}\" با موفقیت حذف شد";
                }
                else
                {
                    TempData["ErrorMessage"] = "نوع کالا مورد نظر یافت نشد";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در حذف نوع کالا: " + ex.Message;
            }

            return RedirectToAction(nameof(CargoTypes));
        }

        private bool CargoTypeExists(int id)
        {
            return _context.CargoTypes.Any(e => e.Id == id);
        }

        // مدیریت نمایندگان کشتیرانی
        [HttpGet]
        public async Task<IActionResult> ShippingAgents()
        {
            var shippingAgents = await _context.ShippingAgents.ToListAsync();
            return View(shippingAgents);
        }

        [HttpGet]
        public IActionResult CreateShippingAgent()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShippingAgent(ShippingAgent shippingAgent)
        {
            if (ModelState.IsValid)
            {
                _context.ShippingAgents.Add(shippingAgent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ShippingAgents));
            }
            return View(shippingAgent);
        }

        // GET: ویرایش نماینده کشتیرانی
        public async Task<IActionResult> EditShippingAgent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingAgent = await _context.ShippingAgents.FindAsync(id);
            if (shippingAgent == null)
            {
                return NotFound();
            }
            return View(shippingAgent);
        }

        // POST: به روزرسانی نماینده کشتیرانی
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShippingAgent(int id, ShippingAgent shippingAgent)
        {
            if (id != shippingAgent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingAgent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingAgentExists(shippingAgent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ShippingAgents));
            }
            return View(shippingAgent);
        }

        // GET: حذف نماینده کشتیرانی
        public async Task<IActionResult> DeleteShippingAgent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippingAgent = await _context.ShippingAgents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shippingAgent == null)
            {
                return NotFound();
            }

            return View(shippingAgent);
        }

        // POST: تایید حذف نماینده کشتیرانی
        [HttpPost, ActionName("DeleteShippingAgent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteShippingAgentConfirmed(int id)
        {
            var shippingAgent = await _context.ShippingAgents.FindAsync(id);
            if (shippingAgent != null)
            {
                _context.ShippingAgents.Remove(shippingAgent);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(ShippingAgents));
        }

        private bool ShippingAgentExists(int id)
        {
            return _context.ShippingAgents.Any(e => e.Id == id);
        }

        // GET: ویرایش کشتی
        [HttpGet]
        public async Task<IActionResult> EditShip(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ship = await _context.Ships.FindAsync(id);
            if (ship == null)
            {
                return NotFound();
            }

            return View(ship);
        }

        // POST: ویرایش کشتی
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShip(int id, Ship ship)
        {
            if (id != ship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // بررسی تکراری نبودن نام کشتی (به جز خود رکورد)
                    bool nameExists = await _context.Ships
                        .AnyAsync(s => s.Name == ship.Name && s.Id != id);

                    if (nameExists)
                    {
                        ModelState.AddModelError("Name", "نام کشتی تکراری است");
                        return View(ship);
                    }

                    _context.Update(ship);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"کشتی \"{ship.Name}\" با موفقیت ویرایش شد";
                    return RedirectToAction(nameof(Ships));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipExists(ship.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "خطا در ویرایش اطلاعات. لطفاً مجدداً تلاش کنید.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"خطا در ویرایش اطلاعات: {ex.Message}");
                }
            }

            return View(ship);
        }

        private bool ShipExists(int id)
        {
            return _context.Ships.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> EditCargoOwner(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoOwner = await _context.CargoOwners.FindAsync(id);
            if (cargoOwner == null)
            {
                return NotFound();
            }

            return View(cargoOwner);
        }

        // POST: BasicData/EditCargoOwner/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCargoOwner(int id, CargoOwner cargoOwner)
        {
            if (id != cargoOwner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cargoOwner);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "اطلاعات صاحب کالا با موفقیت ویرایش شد";
                    return RedirectToAction(nameof(CargoOwners));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CargoOwnerExists(cargoOwner.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(cargoOwner);
        }
        // GET: BasicData/DeleteCargoOwner/5
        public async Task<IActionResult> DeleteCargoOwner(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cargoOwner = await _context.CargoOwners.FindAsync(id);
            if (cargoOwner == null)
            {
                return NotFound();
            }

            return View(cargoOwner);
        }

        // POST: BasicData/DeleteCargoOwner/5
        [HttpPost, ActionName("DeleteCargoOwner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCargoOwnerConfirmed(int id)
        {
            var cargoOwner = await _context.CargoOwners.FindAsync(id);
            if (cargoOwner != null)
            {
                _context.CargoOwners.Remove(cargoOwner);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(CargoOwners));
        }

        private bool CargoOwnerExists(int id)
        {
            return _context.CargoOwners.Any(e => e.Id == id);
        }
        // مدیریت خنکارها
        public IActionResult Dockworkers()
        {
            var dockworkers = _context.Dockworkers.ToList();
            return View(dockworkers);
        }

        public IActionResult CreateDockworker()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDockworker(Dockworker dockworker)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // تنظیم مقادیر پیش‌فرض برای فیلدهای اختیاری
                    if (string.IsNullOrEmpty(dockworker.Description))
                    {
                        dockworker.Description = string.Empty;
                    }

                    dockworker.CreatedDate = DateTime.Now;
                    dockworker.IsActive = true;

                    _context.Dockworkers.Add(dockworker);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "خنکار با موفقیت ایجاد شد";
                    return RedirectToAction(nameof(Dockworkers));
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException?.Message ?? ex.Message;
                    TempData["ErrorMessage"] = $"خطا در ذخیره داده: {innerException}";
                    Console.WriteLine($"Database error: {innerException}");
                }
            }

            return View(dockworker);
        }

        public async Task<IActionResult> Index()
        {
            var dockworkers = await _context.Dockworkers.ToListAsync();
            return View(dockworkers);
        }

        public async Task<IActionResult> EditDockworker(int id)
        {
            var dockworker = await _context.Dockworkers.FindAsync(id);
            if (dockworker == null)
            {
                return NotFound();
            }
            return View(dockworker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDockworker(int id, Dockworker dockworker)
        {
            if (id != dockworker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dockworker);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "خنکار با موفقیت ویرایش شد";
                    return RedirectToAction(nameof(Dockworkers));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DockworkerExists(dockworker.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            TempData["ErrorMessage"] = "لطفاً اطلاعات را به درستی وارد کنید";
            return View(dockworker);
        }

        public async Task<IActionResult> DeleteDockworker(int id)
        {
            var dockworker = await _context.Dockworkers.FindAsync(id);
            if (dockworker == null)
            {
                return NotFound();
            }
            return View(dockworker);
        }

        [HttpPost, ActionName("DeleteDockworker")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDockworkerConfirmed(int id)
        {
            var dockworker = await _context.Dockworkers.FindAsync(id);
            if (dockworker == null)
            {
                TempData["ErrorMessage"] = "خنکار مورد نظر یافت نشد";
                return RedirectToAction(nameof(Dockworkers));
            }

            try
            {
                _context.Dockworkers.Remove(dockworker);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "خنکار با موفقیت حذف شد";
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "امکان حذف این خنکار وجود ندارد (ممکن است در حال استفاده باشد)";
            }

            return RedirectToAction(nameof(Dockworkers));
        }

        private bool DockworkerExists(int id)
        {
            return _context.Dockworkers.Any(e => e.Id == id);
        }
        // GET: دریافت لیست انواع کالا برای DropDown
        public IActionResult GetCargoTypes()
        {
            var cargoTypes = _context.CargoTypes
                .Select(ct => new SelectListItem
                {
                    Value = ct.Id.ToString(),
                    Text = ct.Name // فرض می‌کنم فیلد نام نام کالا است
                })
                .ToList();

            return Json(cargoTypes);
        }
        // GET: حذف کشتی
        [HttpGet]
        public async Task<IActionResult> DeleteShip(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ship = await _context.Ships.FindAsync(id);
            if (ship == null)
            {
                return NotFound();
            }

            return View(ship);
        }

        // POST: تایید حذف کشتی
        [HttpPost, ActionName("DeleteShip")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteShipConfirmed(int id)
        {
            try
            {
                var ship = await _context.Ships.FindAsync(id);
                if (ship != null)
                {
                    _context.Ships.Remove(ship);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"کشتی \"{ship.Name}\" با موفقیت حذف شد";
                }
                else
                {
                    TempData["ErrorMessage"] = "کشتی مورد نظر یافت نشد";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در حذف کشتی: " + ex.Message;
            }

            return RedirectToAction(nameof(Ships));
        }
    }
}