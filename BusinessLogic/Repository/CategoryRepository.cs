using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FurniDbContext context;

        public CategoryRepository(FurniDbContext furniDbContext)
        {
            context = furniDbContext;
        }

        public void Add(Category entity)
        {
            context.Categories.Add(entity);
        }

        public void Delete(int id)
        {
            var category = GetByID(id);
            if (category != null)
            {
                context.Categories.Remove(category);
            }
        }

        public List<Category> GetAll()
        {
            return context.Categories.ToList();
        }

        public Category GetByID(int id)
        {
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public void Update(Category entity)
        {
            context.Categories.Update(entity);
        }
    }
}
