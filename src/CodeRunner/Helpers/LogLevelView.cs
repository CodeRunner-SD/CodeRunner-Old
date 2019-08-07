using CodeRunner.Loggings;
using System.CommandLine.Rendering;
using System.CommandLine.Rendering.Views;

namespace CodeRunner.Helpers
{
    public class LogLevelView : ContentView<LogLevel>
    {
        public LogLevelView(LogLevel value) : base(value)
        {
        }

        public override void Render(ConsoleRenderer renderer, Region region)
        {
            switch (Value)
            {
                case LogLevel.Information:
                    renderer.RenderToRegion($"{ForegroundColorSpan.Cyan()}{Value.ToString()}{ForegroundColorSpan.Reset()}", region);
                    break;
                case LogLevel.Warning:
                    renderer.RenderToRegion($"{ForegroundColorSpan.Yellow()}{Value.ToString()}{ForegroundColorSpan.Reset()}", region);
                    break;
                case LogLevel.Error:
                    renderer.RenderToRegion($"{ForegroundColorSpan.Red()}{Value.ToString()}{ForegroundColorSpan.Reset()}", region);
                    break;
                case LogLevel.Fatal:
                    renderer.RenderToRegion($"{BackgroundColorSpan.Red()}{Value.ToString()}{BackgroundColorSpan.Reset()}", region);
                    break;
                case LogLevel.Debug:
                    renderer.RenderToRegion($"{ForegroundColorSpan.Magenta()}{Value.ToString()}{ForegroundColorSpan.Reset()}", region);
                    break;
            };
        }
    }
}
