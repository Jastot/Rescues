using UnityEngine;


namespace Rescues
{
    public sealed class StandBehaviour : InteractableObjectBehavior
    {
        #region Fields

        public GameObject StandWindow;

        #endregion


        #region UnityMethods

        private void OnValidate()
        {
            Type = InteractableObjectType.Stand;
        }

        #endregion
    }
}
