using HaneenStore2.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaneenStore2.DAL.DTOs.Response
{
    public class CategoriesResponse
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        public List<CategoryTranslationResponse> Translations { get; set; }


    }
}
