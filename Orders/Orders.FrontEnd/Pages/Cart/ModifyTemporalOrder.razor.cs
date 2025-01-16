using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Cart
{
    [Authorize(Roles = "Admin, User")]
    public partial class ModifyTemporalOrder
    {
        private List<string>? categories;
        private List<string>? images;
        private bool loading = true;
        private Product? product;
        private TemporalOrderDTO? temporalOrderDTO;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Parameter] public int TemporalOrderId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadTempotalOrderAsync();
        }

        private async Task LoadTempotalOrderAsync()
        {
            loading = true;
            var responseHttp = await Repository.GetAsync<TemporalOrder>($"api/v1/temporalOrders/{TemporalOrderId}");
            if (responseHttp.Error)
            {
                loading = false;
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            var temporalOrder = responseHttp.Response!;
            temporalOrderDTO = new TemporalOrderDTO
            {
                Id = temporalOrder.Id,
                ProductId = temporalOrder.ProductId,
                Remarks = temporalOrder.Remarks!,
                Quantity = temporalOrder.Quantity
            };
            product = temporalOrder.Product;
            categories = product!.ProductCategories!.Select(x => x.Category!.Name).ToList();
            images = product.ProductImages!.Select(x => x.Image).ToList();
            loading = false;
        }

        public async Task UpdateCartAsync()
        {
            var responseHttp = await Repository.PutAsync("api/v1/temporalOrders/full", temporalOrderDTO);
            if (responseHttp.Error)
            {
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Producto modificado en el carro de compras.");
            NavigationManager.NavigateTo("/");
        }
    }
}