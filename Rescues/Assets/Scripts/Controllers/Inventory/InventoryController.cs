using UnityEngine;


namespace Rescues
{
    public sealed class InventoryController : IInitializeController
    {
        #region Fields

        private readonly GameContext _context;
        private InventoryBehaviour _inventory;
        private ItemSlot _draggedSlot;

        #endregion


        #region ClassLifeCycles

        public InventoryController(GameContext context)
        {
            _context = context;
        }

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            _inventory = Object.FindObjectOfType<InventoryBehaviour>();
            _context.inventory = _inventory;
            _inventory.gameObject.SetActive(false);

            if (_inventory.itemSlots != null)
            {
                for (int i = 0; i < _inventory.itemSlots.Count; i++)
                {
                    _inventory.itemSlots[i].OnBeginDragEvent += BeginDrag;
                    _inventory.itemSlots[i].OnEndDragEvent += EndDrag;
                    _inventory.itemSlots[i].OnDragEvent += Drag;
                    _inventory.itemSlots[i].OnDropEvent += Drop;
                    _inventory.itemSlots[i].OnPointerEnterEvent += ShowTooltip;
                    _inventory.itemSlots[i].OnPointerExitEvent += HideTooltip;
                }
            }
        }

        #endregion


        #region Methods

        private void BeginDrag(ItemSlot itemSlot)
        {
            if (itemSlot.Item != null)
            {
                _draggedSlot = itemSlot;
                _inventory.draggableImage.sprite = itemSlot.Item.Icon;
                _inventory.draggableImage.transform.position = Input.mousePosition;
                _inventory.draggableImage.enabled = true;
            }
        }

        private void EndDrag(ItemSlot itemSlot)
        {
            if (_draggedSlot != null)
            {
                TryToUseItem();
            }
            _inventory.draggableImage.enabled = false;
            _draggedSlot = null;
        }

        private void Drag(ItemSlot itemSlot)
        {
            if (_inventory.draggableImage.enabled)
            {
                _inventory.draggableImage.transform.position = Input.mousePosition;
            }
        }

        private void Drop(ItemSlot dropSlot)
        {
            if (_draggedSlot == null) return;

            ItemData draggedItem = _draggedSlot.Item;
            bool isSomethingCrafted = false;

            foreach (ItemRecipe itemRecipe in _inventory.craftableItemsList)
            {
                if (itemRecipe.CanCraft(_draggedSlot.Item, dropSlot.Item))
                {
                    if (dropSlot.Item.IsDestructuble == false)
                    {
                        _draggedSlot.Item = itemRecipe.Craft(_inventory);
                        isSomethingCrafted = true;
                    }
                    else
                    {
                        dropSlot.Item = itemRecipe.Craft(_inventory);
                        isSomethingCrafted = true;
                    }
                    break;
                }
            }

            if (isSomethingCrafted == false)
            {
                _draggedSlot.Item = dropSlot.Item;
                dropSlot.Item = draggedItem;
            }
        }

        private void ShowTooltip(ItemSlot itemSlot)
        {
            ItemData item = itemSlot.Item;
            if (item != null)
            {
                _inventory.inventoryTooltip.ShowTooltip(item);
            }
        }

        private void HideTooltip(ItemSlot itemSlot)
        {
            _inventory.inventoryTooltip.HideTooltip();
        }

        private void TryToUseItem()
        {
            Vector2 CurMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(CurMousePos, Vector2.zero);
            InventoryDependsBehaviour component;
            for (int j = 0; j < hits.Length; j++)
            {
                if (hits[j].collider.TryGetComponent(out component))
                {
                    for (int i = 0; i < component.itemDependsEvents.Count; i++)
                    {
                        if (component.itemDependsEvents[i].ItemData == _draggedSlot.Item)
                        {
                            for (int k = 0; k < component.itemDependsEvents[i].Events.Count; k++)
                            {
                                TimeRemainingExtensions.AddTimeRemaining(
                                                    new TimeRemaining(component.itemDependsEvents[i].Events[k]));
                            }
                            if (component.itemDependsEvents[i].ItemData.IsDestructuble)
                            {
                                _inventory.RemoveItem(component.itemDependsEvents[i].ItemData);
                            }
                        }
                    }
                }
            }
        }

        #endregion
    }
}
