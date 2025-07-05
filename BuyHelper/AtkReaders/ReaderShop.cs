using ECommons.UIHelpers;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace BuyHelper.AtkReaders;

public unsafe class ReaderShop(AtkUnitBase* shopWindow, int offset = 0) : ShopAtkReader<GilPurchasableItem>(shopWindow, offset)
{
    public override uint PurchasableItemCount => ReadUInt(2).GetValueOrDefault();
    public override List<GilPurchasableItem> Items => Loop<GilPurchasableItem>(14, 1, (int)PurchasableItemCount);
}