using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface ICartItemRepository:IRepository<CartItem>
    {
        //void Add(CartItem entity);
        //void Delete(int id);
        //void Update(CartItem entity);
        //CartItem GetByID(int id);
        //List<CartItem> GetAll();
        //int Save();
        List<CartItem> GetAllByuserID(string userID);
       public Task< List<CartItem>> GetAllByuserIDASC(string userID);
        public Task<int>RemoveRangeCartItemRepoAsc(List<CartItem> items);


    }
}
