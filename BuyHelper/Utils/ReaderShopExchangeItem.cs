using ECommons.UIHelpers;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace BuyHelper.Utils;

public unsafe class ReaderShopExchangeItem(AtkUnitBase* shopWindow, int offset = 0) : AtkReader(shopWindow, offset)
{
    public uint PurchasableItemCount => ReadUInt(3).GetValueOrDefault();
    public List<PurchasableItem> Items => Loop<PurchasableItem>(87, 1, (int)PurchasableItemCount);

    public class PurchasableItem(nint unitBasePtr, int beginOffset = 0) : AtkReader(unitBasePtr, beginOffset)
    {
        private int ItemIndex => beginOffset - 87;
        public string Name => ReadString(0);
        public int Unk0 => ReadInt(122).GetValueOrDefault();
        public uint Quantity => ReadUInt(610).GetValueOrDefault();
        public uint ItemId => ReadUInt(976).GetValueOrDefault();
        public uint Index => ReadUInt(1220).GetValueOrDefault();

        public CurrencyItem CurrencyItem1 => new CurrencyItem(unitBasePtr, 1674 + ItemIndex * 3 + 0);
        public CurrencyItem CurrencyItem2 => new CurrencyItem(unitBasePtr, 1674 + ItemIndex * 3 + 1);
        public CurrencyItem CurrencyItem3 => new CurrencyItem(unitBasePtr, 1674 + ItemIndex * 3 + 2);

        public class CurrencyItem(nint unitBasePtr, int beginOffset = 0) : AtkReader(unitBasePtr, beginOffset)
        {
            public string Name => ReadString(0);
            public int IconId => ReadInt(366).GetValueOrDefault();
            public uint Quantity => ReadUInt(1098).GetValueOrDefault();
            public uint ItemId => ReadUInt(1464).GetValueOrDefault();
        }
    }
}