using Blazored.Modal.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Orders.FrontEnd.Pages.Auth;
using Orders.FrontEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages
{
    public partial class Home
    {
        private int currentPage = 1;
        private int totalPages;
        private int counter = 0;
        private bool isAuthenticated;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [CascadingParameter] private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;
        [CascadingParameter] private IModalService Modal { get; set; } = default!;
        public List<Product>? Products { get; set; }
        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 8;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            await CheckIsAuthenticatedAsync();
            await LoadCounterAsync();
        }

        private async Task CheckIsAuthenticatedAsync()
        {
            var authenticationState = await AuthenticationStateTask;
            isAuthenticated = authenticationState.User.Identity!.IsAuthenticated;
        }

        private async Task LoadCounterAsync()
        {
            if (!isAuthenticated)
            {
                return;
            }

            var responseHtpp = await Repository.GetAsync<int>("api/v1/tempotalOrders/count");
            if (responseHtpp.Error)
            {
                return;
            }
            counter = responseHtpp.Response;
        }

        private async Task SelectRecordsNumberAsync(int recordsnumber)
        {
            RecordsNumber = recordsnumber;
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        private async Task SelectedPageAsync(int page)
        {
            currentPage = page;
            await LoadAsync(page);
        }

        private async Task LoadAsync(int page = 1)
        {
            if (!string.IsNullOrWhiteSpace(Page))
            {
                page = Convert.ToInt32(Page);
            }

            var ok = await LoadListAsync(page);
            if (ok)
            {
                await LoadPagesAsync();
            }
        }

        private void ValidateRecordsNumber(int recordsnumber)
        {
            if (recordsnumber == 0)
            {
                RecordsNumber = 8;
            }
        }

        private async Task<bool> LoadListAsync(int page)
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/v1/products/?page={page}&RecordsNumber={RecordsNumber}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filer={Filter}";
            }

            var responseHttp = await Repository.GetAsync<List<Product>>(url);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return false;
            }
            Products = responseHttp.Response;
            return true;
        }

        private async Task LoadPagesAsync()
        {
            ValidateRecordsNumber(RecordsNumber);
            var url = $"api/v1/products/totalPages/?RecordsNumber={RecordsNumber}";
            if (!string.IsNullOrEmpty(Filter))
            {
                url += $"&filter={Filter}";
            }

            var responseHtpp = await Repository.GetAsync<int>(url);
            if (responseHtpp.Error)
            {
                var message = await responseHtpp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }
            totalPages = responseHtpp.Response;
        }

        private async Task FilterCallBack(string filter)
        {
            Filter = filter;
            await ApplyFilterAsync();
            StateHasChanged();
        }

        private async Task ApplyFilterAsync()
        {
            int page = 1;
            await LoadAsync(page);
            await SelectedPageAsync(page);
        }

        private async void AddToCartAsync(int productId)
        {
            if (!isAuthenticated)
            {
                Modal.Show<Login>();
                var toast1 = SweetAlertService.Mixin(new SweetAlertOptions
                {
                    Toast = true,
                    Position = SweetAlertPosition.BottomEnd,
                    ShowConfirmButton = true,
                    Timer = 3000
                });
                await toast1.FireAsync(icon: SweetAlertIcon.Error, message: "Debes haber iniciado sesión para poder agregar productos al carro de compras.");
                return;
            }

            var tempotalOrderDTO = new TemporalOrderDTO
            {
                ProductId = productId
            };

            var responseHttp = await Repository.PostAsync("api/v1/temporalOrders/full", tempotalOrderDTO);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            await LoadCounterAsync();

            var toast2 = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast2.FireAsync(icon: SweetAlertIcon.Success, message: "Producto agregado al carro de compras.");
        }
    }
}