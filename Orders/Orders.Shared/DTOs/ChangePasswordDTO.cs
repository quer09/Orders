using System.ComponentModel.DataAnnotations;

namespace Orders.Shared.DTOs
{
    public class ChangePasswordDTO
    {
        [Display(Name = "Contraseña actual")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe de tener entre {2} y {1} carácteres.")]
        public string CurrentPassword { get; set; } = null!;

        [Display(Name = "Nueva Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe de tener entre {2} y {1} carácteres.")]
        public string NewPassword { get; set; } = null!;

        [Compare("NewPassword", ErrorMessage = "La nueva contraseña y la confimación no son iguales.")]
        [Display(Name = "Confirmación nueva contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe de tener entre {2} y {1} carácteres.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}