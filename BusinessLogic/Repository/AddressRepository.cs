using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
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
            context.Addresses.Add(entity);
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Address> GetAddressByIdAsc(int id)
        {
            return await context.Addresses.Where(a=>a.Id==id).FirstOrDefaultAsync();
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
          return context.SaveChanges();
        }
        public async Task<Address> SaveAddressAsync(Address address)
        {
            context.Addresses.Add(address);
            await context.SaveChangesAsync();
            return address;
        }

        public void Update(Address entity)
        {
            throw new NotImplementedException();
        }
    }
}
