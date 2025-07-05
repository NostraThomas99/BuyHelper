using Dalamud.Configuration;
using ECommons.DalamudServices;
using Newtonsoft.Json;

namespace BuyHelper;

public class Configuration : IPluginConfiguration
{
    private static string ConfigPath => Svc.PluginInterface.ConfigFile.FullName;
    public void Save()
    {
        var json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(ConfigPath, json);
    }

    public static Configuration Load()
    {
        if (!File.Exists(ConfigPath))
        {
            return new Configuration();
        }
        var json = File.ReadAllText(ConfigPath);
        return JsonConvert.DeserializeObject<Configuration>(json) ?? new Configuration();
    }
    public int Version { get; set; } = 1;
    public bool DebugMode { get; set; } = false;
}