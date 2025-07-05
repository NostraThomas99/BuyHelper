using ECommons.UIHelpers;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace BuyHelper.AtkReaders;

public unsafe class ReaderShopExchangeItem(AtkUnitBase* shopWindow, int offset = 0) : ShopAtkReader<ExchangePurchasableItem>(shopWindow, offset)
{
    public override uint PurchasableItemCount => ReadUInt(3).GetValueOrDefault();
    public override List<ExchangePurchasableItem> Items => Loop<ExchangePurchasableItem>(87, 1, (int)PurchasableItemCount);
}