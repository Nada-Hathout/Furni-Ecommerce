using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public interface IAddressRepository:IRepository<Address>
    {
        public  Task<Address> SaveAddressAsync(Address address);
        public Task<Address> GetAddressByIdAsc(int id);
    }
}
