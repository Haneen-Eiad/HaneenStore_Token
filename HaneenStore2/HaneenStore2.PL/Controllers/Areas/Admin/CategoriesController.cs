using HaneenStore2.BLL.Service;
using HaneenStore2.DAL.DTOs.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace HaneenStore2.PL.Controllers.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer _localizer;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer<SharedResource> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }
        [HttpPost("")]
        public IActionResult Create(CategoriesRequest request)
        {
            var response = _categoryService.CreateCategory(request);
            return Ok(new { meesage = _localizer["success"].Value });
        }
    }
}
