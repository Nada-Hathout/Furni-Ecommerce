using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface IAddressService
    {
        public Task<Address> SaveAddressData(CheckoutViewModel addressVM, string userID);
        public Task<AddressData> GetAddressById(int id);
    }
}
