using UnityEngine;


namespace Rescues
{
    [RequireComponent(typeof(ItemData))]
    public sealed class ItemBehaviour: InteractableObjectBehavior
    {
        #region Fields
        
        public ItemData ItemData;
        public float PickUpTime = 1f;

        #endregion

        #region UnityMethods

        private void OnValidate()
        {
            Type = InteractableObjectType.Item;
        }

        #endregion
    }
}
