using BusinessLogic.Repository;
using System.ComponentModel.DataAnnotations;

namespace Furni_Ecommerce_Website.Validation
{
    public class UniquePhoneAttribute:ValidationAttribute
    {
        public IUsersRepository UserRepository;
        public UniquePhoneAttribute(IUsersRepository usersRepository)
        {
            this.UserRepository = usersRepository;
            
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var phone=UserRepository.GetUniquPhone(value as string);
            if(string.IsNullOrWhiteSpace(phone)) 
                return ValidationResult.Success;
            return new ValidationResult(errorMessage: "This phone number is already in use.");
        }
    }
}
