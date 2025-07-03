using BuyHelper.Utils;
using Dalamud.Interface.Windowing;
using ECommons;
using ECommons.Automation;
using ECommons.UIHelpers.AddonMasterImplementations;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using TaskManager = ECommons.Automation.NeoTaskManager.TaskManager;

namespace BuyHelper.UI;

public unsafe class BuyHelperWindow : Window
{
    AtkUnitBase* _shopWindow;
    public TaskManager TaskManager;

    public BuyHelperWindow() : base("Buy Helper", ImGuiWindowFlags.None, false)
    {
        IsOpen = true;
        TaskManager = new TaskManager();
    }

    public override void Draw()
    {
        if (ImGui.Button("Buy Max"))
        {
            BuyMax();
        }
    }

    private void BuyMax()
    {
        var reader = new ReaderShopExchangeItem(_shopWindow);

        long remainingInventory = InventoryManager.Instance()->GetEmptySlotsInBag();

        foreach (var item in reader.Items)
        {
            long maxToBuy = uint.MaxValue;

            var currencies = new [] { item.CurrencyItem1, item.CurrencyItem2, item.CurrencyItem3 };
            foreach (var currency in currencies)
            {
                if (currency.ItemId != 0 && currency.Quantity > 0)
                {
                    int playerHas = InventoryManager.Instance()->GetInventoryItemCount(currency.ItemId);
                    long canAfford = playerHas / currency.Quantity;
                    maxToBuy = Math.Min(maxToBuy, canAfford);
                }
            }

            long toBuy = Math.Min(maxToBuy, remainingInventory);

            if (toBuy <= 0) continue;

            TaskManager.Enqueue(() => Callback.Fire(_shopWindow, true, 0, item.Index, (uint)toBuy, 0));
            TaskManager.EnqueueDelay(1000);
            remainingInventory -= toBuy;
            if (remainingInventory <= 0) break;
        }
    }

    public override void OnClose()
    {
        IsOpen = true;
    }

    public override bool DrawConditions()
    {
        return GenericHelpers.TryGetAddonByName("ShopExchangeItem", out _shopWindow);
    }
}