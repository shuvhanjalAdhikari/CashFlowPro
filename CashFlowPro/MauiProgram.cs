using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using CashFlowPro.Services;
using CashFlowPro.Services.Interface;
using MudBlazor;

namespace CashFlowPro
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            // service inject 
            builder.Services.AddScoped<IUser, UserService>();
            builder.Services.AddScoped<DebtService>();
            builder.Services.AddScoped<TransactionService>();

            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Services.AddMudServices();
            builder.Logging.AddDebug();
#endif      

            return builder.Build();
        }
    }
}
