using System.Numerics;
using ECommons.DalamudServices;
using ECommons.UIHelpers;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using Lumina.Excel.Sheets;

namespace BuyHelper.AtkReaders;

public abstract unsafe class PurchasableItem(IntPtr shopWindow, int beginOffset = 0) : AtkReader(shopWindow, beginOffset)
{
    protected AtkUnitBase* ShopWindow { get; } = (AtkUnitBase*)shopWindow;
    public abstract string Name { get; }
    public abstract int IconId { get; }
    public abstract uint QuantityInInventory { get; }
    public abstract uint ItemId { get; }
    public abstract uint Index { get; }
    public abstract void DrawCost();
    public uint StackSize => Svc.Data.GetExcelSheet<Item>().GetRow(ItemId).StackSize;
    public Vector2 NameSize => ImGui.CalcTextSize(Name);
}