using BusinessLogic.Repository;
using DataAccess.Models;
using Furni_Ecommerce_Shared.AdminViewModel;
using Furni_Ecommerce_Shared.UserViewModel;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public void AddCategory(CategoryViewModel viewModel)
        {
            var category = new Category
            {
                Name = viewModel.Name,
                Description = viewModel.Description
            };
            _categoryRepository.Add(category);
            _categoryRepository.Save();
        }

        public void UpdateCategory(CategoryViewModel viewModel)
        {
            var category = _categoryRepository.GetByID(viewModel.Id);
            if (category != null)
            {
                category.Name = viewModel.Name;
                category.Description = viewModel.Description;
                _categoryRepository.Update(category);
                _categoryRepository.Save();
            }
        }

        public void DeleteCategory(int id)
        {
            _categoryRepository.Delete(id);
            _categoryRepository.Save();
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        public CategoryListViewModel GetAllCategoryViewModels()
        {
            var categories = _categoryRepository.GetAll();
            return new CategoryListViewModel
            {
                Categories = categories.Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
            };
        }

        public Category GetCategoryById(int id)
        {
            return _categoryRepository.GetByID(id);
        }

        public CategoryViewModel GetCategoryViewModelById(int id)
        {
            var category = _categoryRepository.GetByID(id);
            return category == null ? null : new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public ProductsAndCommentsViewModel GetCategory(int id)
        {
            throw new NotImplementedException();
        }
    }
}