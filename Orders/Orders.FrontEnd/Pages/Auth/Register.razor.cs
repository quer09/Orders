using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Enums;

namespace Orders.FrontEnd.Pages.Auth
{
    public partial class Register
    {
        private UserDTO userDTO = new();
        private List<Country>? countries;
        private List<State>? states;
        private List<City>? cities;
        private bool loading;
        private string? imageUrl;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Parameter, SupplyParameterFromQuery] public bool IsAdmin { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadCountriesAsync();
        }

        private void ImageSelected(string imagenBase64)
        {
            userDTO.Photo = imagenBase64;
            imageUrl = null;
        }

        private async Task LoadCountriesAsync()
        {
            var responseHttp = await Repository.GetAsync<List<Country>>("/api/v1/countries/combo");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            countries = responseHttp.Response;
        }

        private async Task CountryChangedAsync(ChangeEventArgs e)
        {
            var selectedCountry = Convert.ToInt32(e.Value!);
            states = null;
            cities = null;
            userDTO.CityId = 0;
            await LoadStatesAsync(selectedCountry);
        }

        private async Task LoadStatesAsync(int countryId)
        {
            var responseHttp = await Repository.GetAsync<List<State>>($"/api/v1/states/combo/{countryId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            states = responseHttp.Response;
        }

        private async Task StateChangedAsync(ChangeEventArgs e)
        {
            var selectedState = Convert.ToInt32(e.Value!);
            cities = null;
            userDTO.CityId = 0;
            await LoadCitiesAsync(selectedState);
        }

        private async Task LoadCitiesAsync(int stateId)
        {
            var responseHttp = await Repository.GetAsync<List<City>>($"/api/v1/cities/combo/{stateId}");
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            cities = responseHttp.Response;
        }

        private async Task CreateUserAsync()
        {
            userDTO.UserName = userDTO.Email;
            userDTO.UserType = UserType.User;

            if (IsAdmin)
            {
                userDTO.UserType = UserType.Admin;
            }

            loading = true;
            var responseHtpp = await Repository.PostAsync<UserDTO>("/api/v1/accounts/reateUser", userDTO);
            loading = false;
            if (responseHtpp.Error)
            {
                var message = await responseHtpp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await SweetAlertService.FireAsync("Confirmation", "Su cuenta ha sido creada con éxito. Se te ha enviado un correo electrónico con las instrucciones para activar tu usuario.", SweetAlertIcon.Info);
            NavigationManager.NavigateTo("/");
        }
    }
}