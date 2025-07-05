using ECommons.Automation;
using ECommons.UIHelpers;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;

namespace BuyHelper.AtkReaders;

public unsafe class ExchangePurchasableItem(IntPtr shopWindow, int beginOffset = 0) : PurchasableItem(shopWindow, beginOffset)
{
    private readonly int _beginOffset = beginOffset;
    private int ItemIndex => _beginOffset - 87;
    public override string Name => ReadSeString(0).TextValue;
    public override int IconId => ReadInt(122).GetValueOrDefault();
    public override uint QuantityInInventory => ReadUInt(610).GetValueOrDefault();
    public override uint ItemId => ReadUInt(976).GetValueOrDefault();
    public override uint Index => ReadUInt(1220).GetValueOrDefault();
    public override void DrawCost()
    {
        ImGui.Text($"Exchange Items:");
        var currencies = new[] { CurrencyItem1, CurrencyItem2, CurrencyItem3 };
        foreach (var currency in currencies)
        {
            if (currency.ItemId == 0) continue;

            ImGui.Text($"{currency.QuantityRequired} {currency.Name}");
        }
    }

    public CurrencyItem CurrencyItem1 => new CurrencyItem(ShopWindow, 1674 + ItemIndex * 3 + 0);
    public CurrencyItem CurrencyItem2 => new CurrencyItem(ShopWindow, 1674 + ItemIndex * 3 + 1);
    public CurrencyItem CurrencyItem3 => new CurrencyItem(ShopWindow, 1674 + ItemIndex * 3 + 2);

    public class CurrencyItem(AtkUnitBase* unitBasePtr, int beginOffset = 0) : AtkReader(unitBasePtr, beginOffset)
    {
        public string Name => ReadString(0);
        public int IconId => ReadInt(366).GetValueOrDefault();
        public uint QuantityRequired => ReadUInt(1098).GetValueOrDefault();
        public uint ItemId => ReadUInt(1464).GetValueOrDefault();
    }
}