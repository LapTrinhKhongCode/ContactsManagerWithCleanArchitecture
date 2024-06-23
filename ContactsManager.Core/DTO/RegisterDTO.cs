using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Name can't be blank")]
        public string PersonName { get; set; }

        [EmailAddress(ErrorMessage ="Invalid email")]
        [Required(ErrorMessage = "Email can't be blank")]
        public string Email { get; set; }

        [RegularExpression("^[0-9]*$",ErrorMessage ="Invalid phone number")]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Phone can't be blank")]
        public string Phone { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "ConfirmPassword can't be blank")]
        public string ConfirmPassword { get; set; }   
    }
}
