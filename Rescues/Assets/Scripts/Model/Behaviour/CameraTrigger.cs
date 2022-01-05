using UnityEngine;

namespace Rescues
{
    public class CameraTrigger : InteractableObjectBehavior
    {
        #region Fields

        public Vector3 TargetPoint;
        [Min(1)] public float FocusTime;

        #endregion


        #region UnityMethods

        private void OnValidate()
        {
            Type = InteractableObjectType.CameraTrigger;
        }

        #endregion
    }
}