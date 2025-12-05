using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.DAL.DTOs.Request
{
    public class CategoriesRequest
    {
        public List<CategoryTranslationRequest> Translations {  get; set; }
    }
}
