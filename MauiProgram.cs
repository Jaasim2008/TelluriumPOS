using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace TelluriumPOS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Verdana.tff", "Verdana");
                    fonts.AddFont("Verdana-Bold.ttf", "VerdanaBold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
