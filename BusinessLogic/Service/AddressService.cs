using BusinessLogic.Repository;
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
    }
}
