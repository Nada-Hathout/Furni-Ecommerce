using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface ICategoryService
    {
        void AddCategory(CategoryViewModel viewModel);
        void UpdateCategory(CategoryViewModel viewModel);
        void DeleteCategory(int id);
        List<Category> GetAllCategories();
        CategoryListViewModel GetAllCategoryViewModels();
        Category GetCategoryById(int id);
        CategoryViewModel GetCategoryViewModelById(int id);
        ProductsAndCommentsViewModel GetCategory(int id);
    }
}
