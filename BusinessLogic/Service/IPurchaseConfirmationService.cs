using Furni_Ecommerce_Shared.UserViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Service
{
    public interface IPurchaseConfirmationService
    {
        public void PurchaseConfirmationEmail(OrderConfirmationViewModel orderConfirmationViewModel);
    }
}
