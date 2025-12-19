using HaneenStore2.BLL.Service;
using HaneenStore2.DAL.DTOs.Request;
using HaneenStore2.DAL.DTOs.Response;
using HaneenStore2.DAL.Models;
using HaneenStore2.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.BLL
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public CategoriesResponse CreateCategory(CategoriesRequest request)
        {
            var categoris = request.Adapt<Category>();
            _categoryRepository.Create(categoris);
            return categoris.Adapt<CategoriesResponse>();
        }

        public List<CategoriesResponse> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll();
            var response = categories.Adapt<List<CategoriesResponse>>();
            return response;
        }
    }
}
