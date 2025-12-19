using HaneenStore2.DAL.DTOs.Request;
using HaneenStore2.DAL.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.BLL.Service
{
    public interface ICategoryService
    {
        List<CategoriesResponse> GetAllCategories();
        CategoriesResponse CreateCategory(CategoriesRequest request);
    }
}
