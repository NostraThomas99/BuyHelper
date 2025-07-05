using BuyHelper.UI;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ECommons;
using ECommons.Automation.NeoTaskManager;
using ECommons.DalamudServices;

namespace BuyHelper;

public class BuyHelper : IDalamudPlugin
{
    public WindowSystem WindowSystem;
    public GilHelperWindow GilHelperWindow;
    public ExchangeHelperWindow ExchangeHelperWindow;
    public ConfigWindow ConfigWindow;
    public TaskManager TaskManager;
    public static Configuration Config;
    public BuyHelper(IDalamudPluginInterface pluginInterface)
    {
        ECommonsMain.Init(pluginInterface, this, Module.All);
        Config = Configuration.Load();
        TaskManager = new TaskManager();
        WindowSystem = new WindowSystem();
        ConfigWindow = new ConfigWindow();
        GilHelperWindow = new GilHelperWindow(TaskManager);
        ExchangeHelperWindow = new ExchangeHelperWindow(TaskManager);
        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(GilHelperWindow);
        WindowSystem.AddWindow(ExchangeHelperWindow);

        Svc.PluginInterface.UiBuilder.Draw += WindowSystem.Draw;
        Svc.PluginInterface.UiBuilder.OpenConfigUi += OpenConfigUi;
    }

    private void OpenConfigUi()
    {
        ConfigWindow.Toggle();
    }

    public void Dispose()
    {
        ECommonsMain.Dispose();
    }
}