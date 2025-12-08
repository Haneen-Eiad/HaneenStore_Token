using HaneenStore2.DAL.Data;
using HaneenStore2.DAL.DTOs.Request;
using HaneenStore2.DAL.DTOs.Response;
using HaneenStore2.DAL.Models;
using HaneenStore2.DAL.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace HaneenStore2.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
      
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController( IStringLocalizer<SharedResource> localizer,ICategoryRepository categoryRepository ) {
           
            _localizer = localizer;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("")]
        public IActionResult Index()    
        {
            var categories = _categoryRepository.GetAll();
            
            var Response = categories.Adapt<List<CategoriesResponse>>();
            return Ok( new {message = _localizer["success"].Value, Response });
        }
        [HttpPost("")]

        public IActionResult Create (CategoriesRequest request)
        {
            var categories = request.Adapt<Category>();
            _categoryRepository.Create(categories);
           
            return Ok(new { message = _localizer["success"].Value });
        }
    }
}