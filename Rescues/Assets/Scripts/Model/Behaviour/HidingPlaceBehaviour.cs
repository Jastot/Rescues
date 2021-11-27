using UnityEngine;


namespace Rescues
{
    public sealed class HidingPlaceBehaviour: InteractableObjectBehavior
    {
        #region Fields
        
        public HidingPlaceData HidingPlaceData;
        public SpriteRenderer HidedSprite;

        #endregion


        #region UnityMethods

        private void OnValidate()
        {
            Type = InteractableObjectType.HidingPlace;
        }

        #endregion
    }
}
