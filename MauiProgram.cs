using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace StoredManageHelper
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.ConfigureLifecycleEvents(events =>
            {
#if WINDOWS
                events.AddWindows(windows => windows.OnWindowCreated(window =>
                {
                    if (window is Microsoft.UI.Xaml.Window mauiWindow)
                    {
                        var windowId = mauiWindow.AppWindow.Id;
                        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

                        if (appWindow.Presenter is Microsoft.UI.Windowing.OverlappedPresenter presenter)
                        {
                            presenter.SetBorderAndTitleBar(false, false); // 禁用标题栏和边框
                   
                            appWindow.SetPresenter(presenter);
                        }
                      

                    }
              
                }));
#endif
            });


            return builder.Build();
        }
    }
}
