using ECommons.DalamudServices;
using ECommons.EzSharedDataManager;

namespace BuyHelper;

public class IpcSubscribers
{
    internal static class YesAlready
    {
        private static bool _locked = false;
        internal static void Lock()
        {
            if (!_locked && EzSharedData.TryGet<HashSet<string>>("YesAlready.StopRequests", out var stopRequests))
            {
                stopRequests.Add(Svc.PluginInterface.InternalName);
                _locked = true;
            }
        }
        internal static void Unlock()
        {
            if (_locked && EzSharedData.TryGet<HashSet<string>>("YesAlready.StopRequests", out var stopRequests))
            {
                stopRequests.Remove(Svc.PluginInterface.InternalName);
                _locked = false;
            }
        }
    }
}