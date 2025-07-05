using System.Numerics;
using BuyHelper.AtkReaders;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ECommons;
using ECommons.Automation;
using ECommons.DalamudServices;
using ECommons.UIHelpers;
using ECommons.UIHelpers.AddonMasterImplementations;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using TaskManager = ECommons.Automation.NeoTaskManager.TaskManager;

namespace BuyHelper.UI;

public abstract unsafe class BuyHelperWindow : Window
{
    protected AtkUnitBase* ShopWindow;
    protected readonly string AddonName;
    protected readonly TaskManager TaskManager;

    public BuyHelperWindow(string addonName, TaskManager taskManager) : base($"BuyHard##{addonName}", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar, false)
    {
        TaskManager = taskManager;
        AddonName = addonName;
        IsOpen = true;
    }

    public override void OnClose()
    {
        IsOpen = true;
    }

    private Dictionary<uint, int> QuantitiesByItemId = new();
    protected void DrawItem(PurchasableItem item)
    {
        ImGui.PushID(item.ItemId.ToString());
        var lookup = new GameIconLookup((uint)item.IconId);
        var iconTex = Svc.Texture.GetFromGameIcon(lookup);
        ImGui.Image(iconTex.GetWrapOrEmpty().ImGuiHandle, new Vector2(32, 32));
        ImGui.SameLine();
        ImGui.Text(item.Name);
        var quantity = QuantitiesByItemId.GetValueOrDefault(item.ItemId, 1);
        ImGui.SetNextItemWidth(110);
        if (ImGui.InputInt("", ref quantity))
        {
            if (quantity < 1)
            {
                Svc.Log.Warning($"Can't buy negative amount of {item.Name}. Correcting quantity.");
                quantity = 1;
            }
        }
        ImGui.SameLine();
        if (ImGui.Button("Buy"))
        {
            StartPurchaseTasks(item, quantity);
        }
        item.DrawCost();

        if (BuyHelper.Config.DebugMode) //Set Debug values later
        {
            ImGui.Text($"Item Index: {item.Index}");
        }

        QuantitiesByItemId[item.ItemId] = quantity;
        ImGui.Separator();
        ImGui.PopID();
    }

    protected abstract void StartPurchaseTasks(PurchasableItem item, int quantity);

    protected bool ConfirmPurchase()
    {
        if (GenericHelpers.TryGetAddonByName("SelectYesno", out AtkUnitBase* selectYesNoWindow))
        {
            var master = new AddonMaster.SelectYesno(selectYesNoWindow);
            master.Yes();
            return false;
        }
        return true;
    }

    protected uint GetMaxInventory(uint stackSize)
    {
        var freeSpaces = InventoryManager.Instance()->GetEmptySlotsInBag();
        return freeSpaces * stackSize;
    }

    public override void Draw()
    {
        if (BuyHelper.Config.DebugMode) // More Debug
        {
            ImGui.Text($"Task Manager: ({TaskManager.NumQueuedTasks}) {string.Join(",", TaskManager.Stack.Select(t => t.Name))}");
        }
    }

    protected Vector2 GetWindowPosition()
    {
        var x = ShopWindow->X + ShopWindow->GetScaledWidth(true);
        var y = ShopWindow->Y;
        return new Vector2(Math.Max(x, 300), y);
    }

    protected Vector2 GetWindowSize(ReaderShopExchangeItem readerShopExchangeItem)
    {
        var maxTextWidth = readerShopExchangeItem.Items.Max(x => x.NameSize.X);
        return GetWindowSize(maxTextWidth);
    }

    protected Vector2 GetWindowSize(ReaderShop readerShop)
    {
        var maxTextWidth = readerShop.Items.Max(x => x.NameSize.X);
        return GetWindowSize(maxTextWidth);
    }

    private Vector2 GetWindowSize(float maxTextWidth)
    {
        var y = ShopWindow->GetScaledHeight(true);
        return new Vector2(maxTextWidth + 100, y);
    }

    public override bool DrawConditions()
    {
        return GenericHelpers.TryGetAddonByName(AddonName, out ShopWindow);
    }
}