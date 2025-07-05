using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace BuyHelper.UI;

public class ConfigWindow : Window
{
    public ConfigWindow() : base("BuyHard Configuration", ImGuiWindowFlags.None, false)
    {
        Size = new Vector2(200, 100);
        SizeCondition = ImGuiCond.FirstUseEver;
    }

    public override void Draw()
    {
        var debug = BuyHelper.Config.DebugMode;
        if (ImGui.Checkbox("Debug Mode", ref debug))
        {
            BuyHelper.Config.DebugMode = debug;
            BuyHelper.Config.Save();
        }
    }
}