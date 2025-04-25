using BusinessLogic.Repository;
using DataAccess.Models;
using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public class AddressService:IAddressService
    {
      public  IAddressRepository addressRepository;
        public AddressService( IAddressRepository addressRepository)
        {
            this.addressRepository = addressRepository;
            
        }

        public async Task<AddressData> GetAddressById(int id)
        {
         var address= await   addressRepository.GetAddressByIdAsc(id);
            var addressData = new AddressData()
            {
                City = address.City,
                Country = address.Country,
                State = address.State,
                Street = address.Street,
                ZipCode = address.ZipCode
            };
            return addressData;
        }

        public async Task<Address> SaveAddressData(CheckoutViewModel addressVM, string userID)
        {
            Address address = new Address()
            {
                State = addressVM.ShippingAddress.State,
                City = addressVM.ShippingAddress.City,
                ZipCode = addressVM.ShippingAddress.ZipCode,
                Country = addressVM.ShippingAddress.Country,
                UserId = userID,
                Street = addressVM.ShippingAddress.Street
            };
           await addressRepository.SaveAddressAsync(address);
            return address;
           
        }
       
    }
}
