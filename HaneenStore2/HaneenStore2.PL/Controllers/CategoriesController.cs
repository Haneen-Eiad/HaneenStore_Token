using HaneenStore2.DAL.Data;
using HaneenStore2.DAL.DTOs.Request;
using HaneenStore2.DAL.DTOs.Response;
using HaneenStore2.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HaneenStore2.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResource> localizer) {
            _context = context;
            _localizer = localizer;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var categories = _context.Categories.Include(c=>c.Translations).ToList();
            var Response = categories.Adapt<List<CategoriesResponse>>();
            return Ok( new { _localizer["success"].Value, Response });
        }
        [HttpPost("")]

        public IActionResult Create (CategoriesRequest request)
        {
            var categories = request.Adapt<Category>();
            _context.Categories.Add(categories);
            _context.SaveChanges();
            return Ok(new { _localizer["success"].Value });
        }
    }
}