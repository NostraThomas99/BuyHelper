using ECommons.UIHelpers;
using FFXIVClientStructs.FFXIV.Component.GUI;
using InteropGenerator.Runtime;

namespace BuyHelper.AtkReaders;

public abstract unsafe class ShopAtkReader<TItem>(AtkUnitBase* shopWindow, int offset = 0) : AtkReader(shopWindow, offset) where TItem : PurchasableItem
{
    protected AtkUnitBase* ShopWindow { get; } = shopWindow;
    public abstract uint PurchasableItemCount { get; }
    public abstract List<TItem> Items { get; }
}