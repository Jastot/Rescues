using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Rescues
{
    public sealed class InventoryBehaviour : MonoBehaviour
    {
        #region Fields

        public List<ItemSlot> itemSlots;
        public List<ItemRecipe> craftableItemsList;
        public Image draggableImage;
        public InventoryTooltip inventoryTooltip;

        #endregion


        #region Methods

        public bool AddItem(ItemData item)
        {
            bool canAddItem = false;
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].Item == null)
                {
                    itemSlots[i].Item = item;
                    canAddItem = true;
                    break;
                }
            }
            return canAddItem;
        }

        public bool RemoveItem(ItemData item)
        {
            bool canRemoveItem = false;
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].Item == item)
                {
                    itemSlots[i].Item = null;
                    canRemoveItem = true;
                    break;
                }
            }
            return canRemoveItem;
        }

        public bool Contains(ItemData item)
        {
            bool isContain = false;
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].Item == item)
                {
                    isContain = true;
                    break;
                }
            }
            return isContain;
        }

        public bool IsFull()
        {
            bool isFull = true;
            for (int i = 0; i < itemSlots.Count; i++)
            {
                if (itemSlots[i].Item == null)
                {
                    isFull = false;
                    break;
                }
            }
            return isFull;
        }

        #endregion
    }
}

