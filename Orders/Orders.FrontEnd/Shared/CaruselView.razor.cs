using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Orders.FrontEnd.Shared
{
    public partial class CaruselView
    {
        private bool arrows = true;
        private bool bullets = true;
        private bool enableSwipeGesture = true;
        private bool autocycle = true;
        private Transition transition = Transition.Slide;

        [Parameter, EditorRequired] public List<string> Images { get; set; } = null!;
    }
}