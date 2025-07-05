using BuyHelper.AtkReaders;
using ECommons.Automation;
using ECommons.DalamudServices;
using TaskManager = ECommons.Automation.NeoTaskManager.TaskManager;

namespace BuyHelper.UI;

public unsafe class ExchangeHelperWindow : BuyHelperWindow
{
    public ExchangeHelperWindow(TaskManager taskManager) : base("ShopExchangeItem", taskManager)
    {
    }

    public override void Draw()
    {
        var reader = new ReaderShopExchangeItem(ShopWindow);
        Size = GetWindowSize(reader);
        Position = GetWindowPosition();

        foreach (var item in reader.Items)
        {
            DrawItem(item);
        }

        base.Draw();
    }

    protected override void StartPurchaseTasks(PurchasableItem item, int quantity)
    {
        TaskManager.Enqueue(IpcSubscribers.YesAlready.Lock);
        var maxInventory = GetMaxInventory(item.StackSize);
        if (quantity > maxInventory)
        {
            Svc.Chat.PrintError($"You can't buy more than {maxInventory} of {item.Name} due to free inventory space.");
            quantity = (int)maxInventory;
        }

        TaskManager.Enqueue(() => Callback.Fire(ShopWindow, true, 0, item.Index, (uint)quantity, 0));
        TaskManager.Enqueue(ConfirmPurchase);

        TaskManager.Enqueue(IpcSubscribers.YesAlready.Unlock);
    }
}