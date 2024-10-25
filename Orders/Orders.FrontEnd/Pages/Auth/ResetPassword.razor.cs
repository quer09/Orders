using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.DTOs;

namespace Orders.FrontEnd.Pages.Auth
{
    public partial class ResetPassword
    {
        private ResetPasswordDTO resetPasswordDTO = new();
        private bool loading;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Parameter, SupplyParameterFromQuery] public string Token { get; set; } = string.Empty;

        private async Task ChangePasswordAsync()
        {
            resetPasswordDTO.Token = Token;
            loading = true;
            var responseHttp = await Repository.PostAsync("api/v1/accounts/RestPassword", resetPasswordDTO);
            loading = false;
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await SweetAlertService.FireAsync("Confirmación", "Contraseña cambiada con éxito, ahora puede ingresar con su nueva contraseña.", SweetAlertIcon.Info);
            NavigationManager.NavigateTo("/Login");
        }
    }
}