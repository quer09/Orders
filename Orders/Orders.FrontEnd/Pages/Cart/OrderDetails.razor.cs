using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;
using Orders.Shared.Enums;
using System.Net;

namespace Orders.FrontEnd.Pages.Cart
{
    [Authorize(Roles = "Admin, User")]
    public partial class OrderDetails
    {
        private Order? order;

        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Parameter] public int OrderId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadAsyn();
        }

        private async Task LoadAsyn()
        {
            var responseHttp = await Repository.GetAsync<Order>($"api/v1/orders/{OrderId}");
            if (responseHttp.Error)
            {
                if (responseHttp.HttpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                {
                    NavigationManager.NavigateTo("/orders");
                    return;
                }
                var message = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            order = responseHttp.Response;
        }

        private async Task CancelOrderAsync()
        {
            await ModifyTemporalOrder("cancelar", OrderStatus.Cancelled);
        }

        private async Task DispatchOrderAsync()
        {
            await ModifyTemporalOrder("despachar", OrderStatus.Dispatched);
        }

        private async Task SendOrderAsync()
        {
            await ModifyTemporalOrder("enviar", OrderStatus.Sent);
        }

        private async Task ConfirmOrderAsync()
        {
            await ModifyTemporalOrder("confirmar", OrderStatus.Confirmed);
        }

        private async Task ModifyTemporalOrder(string message, OrderStatus status)
        {
            var result = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirmación",
                Text = $"¿Estas seguro que quieres {message} el pedido?",
                Icon = SweetAlertIcon.Question,
                ShowCancelButton = true
            });

            var confirm = string.IsNullOrEmpty(result.Value);
            if (confirm)
            {
                return;
            }

            var orderDto = new OrderDTO
            {
                Id = OrderId,
                OrderStatus = status
            };

            var responseHttp = await Repository.PutAsync("api/v1/orders", orderDto);
            if (responseHttp.Error)
            {
                var messageError = await responseHttp.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", messageError, SweetAlertIcon.Error);
                return;
            }

            NavigationManager.NavigateTo("/orders");
        }
    }
}