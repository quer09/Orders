using Microsoft.AspNetCore.Components;
using Orders.FrontEnd.Repositories;
using Orders.Shared.Entities;

namespace Orders.FrontEnd.Pages.Countries
{
    public partial class CountriesIndex
    {
        [Inject] private IRepository Repositry { get; set; } = null!;
        public List<Country>? Countries { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            var responseHttp = await Repositry.GetAsync<List<Country>>("api/v1/countries");
            Countries = responseHttp.Response;
        }
    }
}