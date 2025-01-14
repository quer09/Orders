using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages
{
    public partial class Home
    {
        private int currentPage = 1;
        private int totalPages;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        public List<Product>? Products { get; set; }
        [Parameter, SupplyParameterFromQuery] public string Page { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string Filter { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public int RecordsNumber { get; set; } = 8;

        protected override async Task OnInitializedAsync()
        {
            await LoadAsync();
        }

        private async Task SelectRecordsNumberAsync(int recordsNumber)
        {
            RecordsNumber = recordsNumber;
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

        private void ValidateRecordsNumber(int recordsNumber)
        {
            if (recordsNumber == 0)
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

        private void AddToCartAsync(int productId)
        {
        }
    }
}