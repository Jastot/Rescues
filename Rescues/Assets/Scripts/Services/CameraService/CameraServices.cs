using UnityEngine;


namespace Rescues
{
    public sealed class CameraServices : Service
    {
        #region Fields

        private const int CAMERA_DEPTH = -45;
        public Camera CameraMain;
        public bool IsCameraFree = false;
        public bool IsCameraFocused = false;
        public Vector3 OverridePosition;
        private Vector3 _mouseOriginalClickPosition;
        private Vector3 _moveLimit;
        private float _cameraFreeMoveLimit;
        private int _cameraDragSpeed;
        private int _currentFocusOverrideID = 0;

        #endregion


        #region ClassLifeCycles

        public CameraServices(Contexts contexts) : base(contexts)
        {
            CameraMain = Camera.main;
            var context = contexts as GameContext;
            _cameraDragSpeed = context.activeLocation.CameraData.CameraDragSpeed;
            _cameraFreeMoveLimit = context.activeLocation.CameraData.CameraFreeMoveLimit;
        }

        #endregion


        #region Properties

        public int CameraDepthConst => CAMERA_DEPTH;

        #endregion


        #region Methods

        public void FreeCamera()
        {
            IsCameraFree = true;
            _mouseOriginalClickPosition = Input.mousePosition;           
            return;
        }

        public void FreeCameraMovement()
        {          
            Vector3 direction = CameraMain.ScreenToViewportPoint(Input.mousePosition - _mouseOriginalClickPosition);
            Vector3 move = new Vector3(direction.x * _cameraDragSpeed, direction.y * _cameraDragSpeed);
            _moveLimit = new Vector3(Mathf.Clamp(move.x, -_cameraFreeMoveLimit, _cameraFreeMoveLimit),
                Mathf.Clamp(move.y, -_cameraFreeMoveLimit, _cameraFreeMoveLimit));          
        }

        public void MoveCameraWithMouse()
        {
            CameraMain.transform.Translate(_moveLimit, Space.World);
        }

        public void LockCamera()
        {
            IsCameraFree = false;
        }    

        public void SetCameraFocus(Vector3 targetPoint)
        {
            IsCameraFocused = true;
            OverridePosition = targetPoint;
        }

        public void ResetFocus()
        {
            IsCameraFocused = false;
            _currentFocusOverrideID = 0;
        }

        public void SetCameraFocusWithID(Vector3 targetPoint, int id)
        {
            _currentFocusOverrideID = id;
            SetCameraFocus(targetPoint);
        }

        public void ResetFocusWithID(int id)
        {
            if (id == _currentFocusOverrideID)
            {
                ResetFocus();
            }
        }

        #endregion
    }
}
