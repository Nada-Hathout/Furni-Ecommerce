using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class AddressRepository : IAddressRepository
    {
        public FurniDbContext context;
        public AddressRepository(FurniDbContext context)
        {
            
            this.context = context;
        }
        public void Add(Address entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Address> GetAll()
        {
            throw new NotImplementedException();
        }

        public Address GetByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Address entity)
        {
            throw new NotImplementedException();
        }
    }
}
