using UnityEngine;


namespace Rescues
{
    public sealed class CameraController : IExecuteController, IInitializeController, ITearDownController
    {
        #region Fields

        private const float ACCELERATION_COEFFICIENT = 0.1f;

        private readonly GameContext _context;
        private readonly CameraServices _cameraServices;
        private CameraData _activeCamera;
        private float _deadZone;
        private float _cameraAccelerateStep;
        private float _cameraAcceleration;
        private float _targetPositionX;
        private float _targetPositionY;
        private bool _isMoveableCameraMode;
        private int _activeLocationHash;
        private bool _isFocusedCameraMode;

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

            if (_cameraServices.IsCameraFocused && !_isFocusedCameraMode)
            {
                _isFocusedCameraMode = true;
            }

            if (!_cameraServices.IsCameraFocused && _isFocusedCameraMode)
            {
                _isFocusedCameraMode = false;
            }

            if (_isFocusedCameraMode && _context.character.IsMoving)
            {
                _cameraServices.ResetFocus();
            }

            if (_isMoveableCameraMode && !_isFocusedCameraMode)
            {
                MoveCameraToCharacter();
            }

            if (_isFocusedCameraMode)
            {
                MoveCameraToTarget();
            }

            if (_cameraServices.IsCameraFree)
            {
                _cameraServices.MoveCameraWithMouse();
            }
        }

        #endregion

        #region IInitializeController

        public void Initialize()
        {
            var camTriggers = _context.GetTriggers(InteractableObjectType.CameraTrigger);
            foreach (var trigger in camTriggers)
            {
                var behavior = trigger as InteractableObjectBehavior;
                behavior.OnFilterHandler += OnFilterHandler;
                behavior.OnTriggerEnterHandler += OnTriggerEnterHandler;
                behavior.OnTriggerExitHandler += OnTriggerExitHandler;

                var cameraTrigger = trigger as CameraTrigger;
                cameraTrigger.Init(_cameraServices);
            }
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var camTriggers = _context.GetTriggers(InteractableObjectType.CameraTrigger);
            foreach (var trigger in camTriggers)
            {
                var behavior = trigger as InteractableObjectBehavior;
                behavior.OnFilterHandler -= OnFilterHandler;
                behavior.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                behavior.OnTriggerExitHandler -= OnTriggerExitHandler;
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

            _targetPositionX = _context.character.Transform.position.x + _activeCamera.Position_X_Offset;
            var x = Mathf.Clamp(_targetPositionX, _activeCamera.MoveLeftXLimit, _activeCamera.MoveRightXLimit);
            _cameraServices.CameraMain.transform.position = new Vector3(x, _activeCamera.Position_Y_Offset,
                _cameraServices.CameraDepthConst);

            _isMoveableCameraMode = true;
        }

        private void MoveCameraToCharacter()
        {
            _targetPositionX = _context.character.Transform.position.x + _activeCamera.Position_X_Offset;
            _targetPositionY = _activeCamera.Position_Y_Offset;

            if (_context.character.IsMoving == false)
            {
                _deadZone = _activeCamera.DeadZone;
            }

            MoveCamera();
        }

        

        private void MoveCameraToTarget()
        {
            _targetPositionX = _cameraServices.OverridePosition.x + _activeCamera.Position_X_Offset;
            _targetPositionY = _cameraServices.OverridePosition.y;

            MoveCamera();
        }

        private void MoveCamera()
        {
            var cameraPositionX = _cameraServices.CameraMain.transform.position.x;
            var cameraPositionY = _cameraServices.CameraMain.transform.position.y;

            if (Mathf.Abs(_cameraServices.CameraMain.transform.position.x - _targetPositionX) > _deadZone ||
                !Mathf.Approximately(cameraPositionY, _activeCamera.Position_Y_Offset))
            {
                _cameraAcceleration = _cameraAccelerateStep * Time.deltaTime;
                _deadZone = 0;
                _cameraAcceleration = Mathf.Clamp(_cameraAcceleration, 0f, 1f);
                cameraPositionX = Mathf.Lerp(cameraPositionX, _targetPositionX, _cameraAcceleration);
                cameraPositionY = Mathf.Lerp(cameraPositionY, _targetPositionY, _cameraAcceleration);
            }

            cameraPositionX = Mathf.Clamp(cameraPositionX, _activeCamera.MoveLeftXLimit, _activeCamera.MoveRightXLimit);
            cameraPositionY = Mathf.Clamp(cameraPositionY, _activeCamera.MoveDownYLimit, _activeCamera.MoveUpYLimit);

            _cameraServices.CameraMain.transform.position = new Vector3(cameraPositionX, cameraPositionY,
                _cameraServices.CameraDepthConst);
        }

        private bool OnFilterHandler(Collider2D obj)
        {
            return obj.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = true;
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
        }

        #endregion
    }
}
