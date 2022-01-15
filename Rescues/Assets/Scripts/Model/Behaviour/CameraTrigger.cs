using UnityEngine;

namespace Rescues
{
    public class CameraTrigger : InteractableObjectBehavior
    {
        #region Fields

        public Vector2 TargetPoint;
        public bool IsReturningOnTimer;
        public float FocusTime;
        private CameraServices _cameraServices;

        #endregion


        #region UnityMethods

        private void OnValidate()
        {
            Type = InteractableObjectType.CameraTrigger;
        }

        #endregion


        #region Methods

        public void Init(CameraServices cameraServices)
        {
            _cameraServices = cameraServices;
        }


        public void ResetCamera()
        {
            _cameraServices?.ResetFocus();
        }

        #endregion
    }
}