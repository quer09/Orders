using Microsoft.AspNetCore.Components;

namespace Orders.FrontEnd.Shared
{
    public partial class Filter
    {
        [Parameter] public Func<string, Task> Callback { get; set; } = async (text) => await Task.CompletedTask;
        [Parameter] public string PlaceHolder { get; set; } = string.Empty;
        [Parameter, SupplyParameterFromQuery] public string TextToFilter { get; set; } = string.Empty;

        private async Task CleanFilterAsync()
        {
            await Callback(string.Empty);
        }

        private async Task ApplyFilterAsync()
        {
            await Callback(TextToFilter);
        }
    }
}