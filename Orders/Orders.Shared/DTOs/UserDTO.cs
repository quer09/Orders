using Orders.Shared.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Shared.DTOs
{
    public class UserDTO : User
    {
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe de tener entre {2} y {1} carácteres.")]
        public string Password { get; set; } = null!;

        [Compare("Password", ErrorMessage = "La contraseña y la confimación no son iguales.")]
        [Display(Name = "Confirmación de contraseña")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe de tener entre {2} y {1} carácteres.")]
        public string PasswordConfirm { get; set; } = null!;
    }
}