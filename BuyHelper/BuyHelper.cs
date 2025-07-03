using BuyHelper.UI;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ECommons;
using ECommons.DalamudServices;

namespace BuyHelper;

public class BuyHelper : IDalamudPlugin
{
    public WindowSystem WindowSystem;
    public BuyHelperWindow BuyHelperWindow;
    public BuyHelper(IDalamudPluginInterface pluginInterface)
    {
        ECommonsMain.Init(pluginInterface, this, Module.All);
        WindowSystem = new WindowSystem();
        BuyHelperWindow = new BuyHelperWindow();
        WindowSystem.AddWindow(BuyHelperWindow);

        Svc.PluginInterface.UiBuilder.Draw += WindowSystem.Draw;
    }
    public void Dispose()
    {
        ECommonsMain.Dispose();
    }
}