using System;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;

namespace CodeRunner.Helpers
{
    public class TableHeaderView : ContentView<string>
    {
        public TableHeaderView(string value) : base(value)
        {
        }

        public override void Render(ConsoleRenderer renderer, Region region)
        {
            renderer.RenderToRegion($"{StyleSpan.UnderlinedOn()}{StyleSpan.BoldOn()}{Value}{StyleSpan.BoldOff()}{StyleSpan.UnderlinedOff()}", region);
        }
    }
}
