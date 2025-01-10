using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.DTOs;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public partial class ProductEdit
    {
        private ProductDTO productDTO = new()
        {
            ProductCategotyIds = new List<int>(),
            ProductImages = new List<string>()
        };

        private ProductForm? productForm;
        private List<Category> selectedCategories = new();
        private List<Category> nonSelectedCategories = new();
        private bool loading = true;
        private Product? product;

        [Inject] private IRepository Repository { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
        [Parameter] public int ProductId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadProductAsync();
            await LoadCategoriesAsync();
        }

        private async Task AddImageAsync()
        {
            if(productDTO.ProductImages is null || productDTO.ProductImages.Count == 0)
            {
                return;
            }

            var imageDto = new ImageDTO
            {
                ProductId = ProductId,
                Images = productDTO.ProductImages
            };

            var htppResponse = await Repository.PostAsync<ImageDTO, ImageDTO>("api/products/addImages", imageDto);
            if(htppResponse.Error)
            {
                var message = await htppResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            productDTO.ProductImages = htppResponse.Response!.Images;
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Imagenes agregadas con éxito.");
        }

        private async Task RemoveImageAsync()
        {
            if(productDTO.ProductImages is null || productDTO.ProductImages.Count == 0)
            {
                return;
            }

            var imageDto = new ImageDTO
            {
                ProductId = ProductId,
                Images = productDTO.ProductImages
            };

            var httpResponse = await Repository.PostAsync<ImageDTO, ImageDTO>("api/v1/products/removeLastImage", imageDto);
            if(httpResponse.Error)
            {
                var message = await httpResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            productDTO.ProductImages = httpResponse.Response!.Images;
            var toast = SweetAlertService.Mixin(new SweetAlertOptions
            {
                Toast = true,
                Position = SweetAlertPosition.BottomEnd,
                ShowConfirmButton = true,
                Timer = 3000
            });
            await toast.FireAsync(icon: SweetAlertIcon.Success, message: "Imagén eliminada con éxito.");
        }

        private async Task LoadProductAsync()
        {
            loading = true;
            var httpResponse = await Repository.GetAsync<Product>($"api/v1/products/{ProductId}");
            if (httpResponse.Error)
            {
                loading = false;
                var message = await httpResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            product = httpResponse.Response!;
            productDTO = ToProductDTO(product);
            loading = false;
        }

        private ProductDTO ToProductDTO(Product product)
        {
            return new ProductDTO
            {
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                ProductCategotyIds = product.ProductCategories!.Select(x => x.CategoryId).ToList(),
                ProductImages = product.ProductImages!.Select(x => x.Image).ToList()
            };
        }

        private async Task LoadCategoriesAsync()
        {
            loading = true;
            var httpResponse = await Repository.GetAsync<List<Category>>("api/v1/categories/combo");

            if (httpResponse.Error)
            {
                loading = false;
                var message = await httpResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            var categories = httpResponse.Response!;
            foreach (var category in categories)
            {
                var found = product!.ProductCategories!.FirstOrDefault(x => x.CategoryId == category.Id);
                if (found == null)
                {
                    nonSelectedCategories.Add(category);
                }
                else
                {
                    selectedCategories.Add(category);
                }
            }
            loading = false;
        }

        private async Task SaveChangesAsync()
        {
            var httpResponse = await Repository.PutAsync("api/v1/products/full", productDTO);
            if (httpResponse.Error)
            {
                var message = await httpResponse.GetErrorMessageAsync();
                await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
                return;
            }

            Return();
        }

        private void Return()
        {
            productForm!.FormPostedSuccessfully = true;
            NavigationManager.NavigateTo("/products");
        }
    }
}