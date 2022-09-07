using System.ComponentModel.DataAnnotations;

namespace MyBankApi.DTOs
{
    public class AuthenticationDto
    {
        private const string V = "@^[0-9]{10}$*";
        private const string U = "@^[0-9]{4}$*";

        [Required]
        //[RegularExpression(V, ErrorMessage = "Account number must be 10 digits")]
        public string AccountNumber { get; set; }
        [Required]
        //[RegularExpression(U, ErrorMessage = "Pin must be 4 digits")]
        public string Pin { get; set; }
    }
}
