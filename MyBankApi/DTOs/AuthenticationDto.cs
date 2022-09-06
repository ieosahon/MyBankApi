using System.ComponentModel.DataAnnotations;

namespace MyBankApi.DTOs
{
    public class AuthenticationDto
    {
        private const string V = "@^[2][0-9]/d{9}*$";
        private const string U = "@^[0-9]/d{4}*$";

        [Required]
        [RegularExpression(V, ErrorMessage = "Account number must be 10 digits")]
        public string AccountNumber { get; set; }
        [Required]
        [RegularExpression(U, ErrorMessage = "Pin must be 4 digits")]
        public string Pin { get; set; }
    }
}
