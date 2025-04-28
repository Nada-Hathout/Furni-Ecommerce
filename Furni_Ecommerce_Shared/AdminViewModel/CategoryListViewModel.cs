using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Furni_Ecommerce_Shared.AdminViewModel
{
    public class CategoryListViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public string StatusMessage { get; set; }
    }
}
