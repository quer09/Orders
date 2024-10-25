using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.DTOs
{
    public class ResetPasswordDTO
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "EL campo {0} es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debes ingresar un correo válido.")]
        public string Email { get; set; } = null!;

        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe de tener entre {2} y {1} carácteres.")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "La nueva contraseña y la confimación no son iguales.")]
        [Display(Name = "Confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe de tener entre {2} y {1} carácteres.")]
        public string ConfirmPassword { get; set; } = null!;

        public string Token { get; set; } = null!;
    }
}