using BuyHelper.AtkReaders;
using ECommons.Automation;
using ECommons.Automation.NeoTaskManager;
using ECommons.DalamudServices;

namespace BuyHelper.UI;

public unsafe class GilHelperWindow : BuyHelperWindow
{
    private const int MaxBuyBatch = 99;

    public GilHelperWindow(TaskManager taskManager) : base("Shop", taskManager)
    {
    }

    public override void Draw()
    {
        var reader = new ReaderShop(ShopWindow);
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
        int toBuy = quantity;
        while (toBuy > 0)
        {
            int batch = Math.Min(toBuy, MaxBuyBatch);
            TaskManager.Enqueue(() => Callback.Fire(ShopWindow, true, 0, item.Index, (uint)batch, 0));
            TaskManager.EnqueueDelay(500);
            TaskManager.Enqueue(ConfirmPurchase);
            TaskManager.EnqueueDelay(1000);
            toBuy -= batch;
        }
        TaskManager.Enqueue(IpcSubscribers.YesAlready.Unlock);
    }
}