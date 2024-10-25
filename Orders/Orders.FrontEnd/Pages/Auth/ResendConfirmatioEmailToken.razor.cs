using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.DTOs;

namespace Orders.FrontEnd.Pages.Auth
{
    public partial class ResendConfirmatioEmailToken
    {
        private EmailDTO emailDTO = new();
        private bool loading;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;

        private async Task ResendConfirmationEmailTokenAsync()
        {
            loading = true;
            var responseHtpp = await Repository.PostAsync("/api/v1/accounts/ResedToken", emailDTO);
            loading = false;
            if (responseHtpp.Error)
            {
                var message = await responseHtpp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            await SweetAlertService.FireAsync("Confirmación", "Se te ha enviado un correo electrónico con las instrucciones para activar tu usario.", SweetAlertIcon.Info);
            NavigationManager.NavigateTo("/");
        }
    }
}