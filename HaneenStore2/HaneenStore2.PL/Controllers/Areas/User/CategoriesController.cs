using HaneenStore2.BLL;
using HaneenStore2.BLL.Service;
using HaneenStore2.DAL.DTOs.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace HaneenStore2.PL.Controllers.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer _localizer;

      public  CategoriesController(ICategoryService categoryService,IStringLocalizer<SharedResource> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }
        [HttpGet("")] 

        public IActionResult Index()
        {
            var response = _categoryService.GetAllCategories();
            return Ok(new { meesage = _localizer["success"].Value, response });
        }
       
    }
}
