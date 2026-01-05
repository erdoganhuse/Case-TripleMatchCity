using Cysharp.Threading.Tasks;
using Library.ServiceLocatorSystem;

namespace Modules.Gameplay
{
    public class ItemCollectCommand
    {
        private ItemCollectController ItemCollectController => ServiceLocator.Get<ItemCollectController>();

        private readonly BaseItem _item;
        
        public ItemCollectCommand(BaseItem item)
        {
            _item = item;
        }
        
        public void Execute()
        {
            ExecuteAsync().Forget();
        }

        private async UniTask ExecuteAsync()
        {
            if (!_item.IsCollectable())
            {
                return;
            }
            
            bool isItemBooster = _item is BoosterItem;
            if (isItemBooster)
            {
                //ToDo: handle in level booster operations
            }
            else if(ItemCollectController.CanCollect(_item))
            {
                await ItemCollectController.CollectAsync(_item);
            }
        }
    }
}