using CloudSupportSystem.Data;
using CloudSupportSystem.Models;
using CloudSupportSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudSupportSystem.Controllers
{
    public class SupportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ISqsService _sqsService;

        public SupportController(AppDbContext context, ISqsService sqsService)
        {
            _context = context;
            _sqsService = sqsService;
        }

       
        public async Task<IActionResult> Index()
        {
            var requests = await _context.SupportRequests
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return View(requests);
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var request = await _context.SupportRequests
                .FirstOrDefaultAsync(m => m.Id == id);

            if (request == null) return NotFound();

            return View(request);
        }

        
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SupportRequest supportRequest)
        {
            if (ModelState.IsValid)
            {
                supportRequest.CreatedAt = DateTime.UtcNow;
                _context.Add(supportRequest);
                await _context.SaveChangesAsync();

                
                await _sqsService.SendMessageAsync(
                    supportRequest.Id,
                    supportRequest.Subject,
                    supportRequest.Priority.ToString()
                );

                return RedirectToAction(nameof(Index));
            }
            return View(supportRequest);
        }

        
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, Status status)
        {
            var request = await _context.SupportRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = status;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}