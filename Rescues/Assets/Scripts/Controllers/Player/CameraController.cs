using UnityEngine;


namespace Rescues
{
    public sealed class CameraController : IExecuteController
    {
        #region Fields

        private const float ACCELERATION_COEFFICIENT = 0.1f;

        private readonly GameContext _context;
        private readonly CameraServices _cameraServices;
        private CameraData _activeCamera;
        private float _deadZone;
        private float _cameraAccelerateStep;
        private float _cameraAcceleration;
        private float _characterPositionX;
        private bool _isMoveableCameraMode;
        private int _activeLocationHash;

        #endregion


        #region ClassLifeCycles

        public CameraController(GameContext context, Services services)
        {
            _context = context;
            _cameraServices = services.CameraServices;
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            if (_activeLocationHash != _context.activeLocation.GetHashCode())
            {
                SetCamera();
            }

            if (_isMoveableCameraMode)
            {
                MoveCameraToCharacter();
            }

            if (_cameraServices.IsCameraFree)
            {
                _cameraServices.MoveCameraWithMouse();
            }
        }

        #endregion


        #region Methods

        private void SetCamera()
        {
            _isMoveableCameraMode = false;
            _activeCamera = _context.activeLocation.CameraData;

            switch (_activeCamera.CameraMode)
            {
                case CameraMode.None:
                    return;

                case CameraMode.Moveable:
                    PresetMovableCamera();
                    break;

                case CameraMode.Static:
                    PlaceCameraOnLocation();
                    break;
            }

            _cameraServices.CameraMain.orthographicSize = _activeCamera.CameraSize;
            _cameraServices.CameraMain.backgroundColor = _context.activeLocation.BackgroundColor;
            _activeLocationHash = _context.activeLocation.GetHashCode();
        }

        private void PlaceCameraOnLocation()
        {
            var position = _context.activeLocation.LocationInstance.CameraPosition;
            _cameraServices.CameraMain.transform.position = new Vector3(position.x, position.y,
                _cameraServices.CameraDepthConst);
        }

        private void PresetMovableCamera()
        {
            _cameraAccelerateStep = _activeCamera.CameraAccelerateStep * ACCELERATION_COEFFICIENT;
            _cameraAcceleration = 1f;
            _deadZone = _activeCamera.DeadZone;

            _characterPositionX = _context.character.Transform.position.x + _activeCamera.Position_X_Offset;
            var x = Mathf.Clamp(_characterPositionX, _activeCamera.MoveLeftXLimit, _activeCamera.MoveRightXLimit);
            _cameraServices.CameraMain.transform.position = new Vector3(x, _activeCamera.Position_Y_Offset,
                _cameraServices.CameraDepthConst);

            _isMoveableCameraMode = true;
        }

        private void MoveCameraToCharacter()
        {
            _characterPositionX = _context.character.Transform.position.x + _activeCamera.Position_X_Offset;
            var cameraPositionX = _cameraServices.CameraMain.transform.position.x;
            if (_context.character.IsMoving == false)
            {
                _deadZone = _activeCamera.DeadZone;
            }

            if (Mathf.Abs(_cameraServices.CameraMain.transform.position.x - _characterPositionX) > _deadZone)
            {
                _deadZone = 0;
                _cameraAcceleration = Mathf.Clamp(_cameraAcceleration, 0f, 1f);
                cameraPositionX = Mathf.Lerp(cameraPositionX, _characterPositionX, _cameraAcceleration);
                if (cameraPositionX >= _activeCamera.MoveLeftXLimit && cameraPositionX <= _activeCamera.MoveRightXLimit)
                {
                    _cameraAcceleration = _cameraAccelerateStep * Time.deltaTime;
                }
                else
                {
                    cameraPositionX = Mathf.Clamp(cameraPositionX, _activeCamera.MoveLeftXLimit, _activeCamera.
                        MoveRightXLimit);
                    _cameraAcceleration = _cameraAccelerateStep;
                }
            }

            _cameraServices.CameraMain.transform.position = new Vector3(cameraPositionX, _activeCamera.Position_Y_Offset,
                _cameraServices.CameraDepthConst);
        }

        #endregion
    }
}
