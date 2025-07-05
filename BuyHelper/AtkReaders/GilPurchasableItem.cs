using ECommons.Automation;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;

namespace BuyHelper.AtkReaders;

public unsafe class GilPurchasableItem(IntPtr shopWindow, int beginOffset = 0) : PurchasableItem(shopWindow, beginOffset)
{
    public override string Name => ReadSeString(0).TextValue;
    public override int IconId => (int)ReadUInt(183).GetValueOrDefault();
    public override uint QuantityInInventory => ReadUInt(122).GetValueOrDefault();
    public override uint ItemId => ReadUInt(427).GetValueOrDefault();
    public override uint Index { get; } = (uint)beginOffset - 14;
    public override void DrawCost()
    {
        ImGui.Text($"Cost: {GilCost}");
    }
    public uint GilCost => ReadUInt(61).GetValueOrDefault();

}