using UnityEngine;


namespace Rescues
{
    public sealed class CameraController : IExecuteController
    {
        #region Fields

        private readonly GameContext _context;
        private readonly CameraServices _cameraServices;
        private bool _iSMoveableCameraMode;
        private int _activeLocationHash = 0;
        
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
            if ( _activeLocationHash != _context.activeLocation.GetHashCode())
                SetCamera();
            
            if (_iSMoveableCameraMode)
                MoveCameraToCharacter();
            
            if(_cameraServices.IsCameraFree) 
                _cameraServices.MoveCameraWithMouse();
        }

        #endregion IExecuteController


        #region Methods

        private void SetCamera()
        {
            _iSMoveableCameraMode = false;

            switch (_context.activeLocation.CameraData.CameraMode)
            {
                case CameraMode.None:
                    return;

                case CameraMode.Moveable:
                    _iSMoveableCameraMode = true;
                    break; 
                
                case CameraMode.Static:
                    PlaceCameraOnLocation();
                    break;
            }
            
            _cameraServices.CameraMain.backgroundColor = _context.activeLocation.BackgroundColor;
            _activeLocationHash = _context.activeLocation.GetHashCode();
        }

        private void PlaceCameraOnLocation()
        {
            var position = _context.activeLocation.LocationInstance.CameraPosition;
            _cameraServices.CameraMain.transform.position = new Vector3(position.x, position.y,
                _cameraServices.CameraDepthConst);
            _cameraServices.CameraMain.orthographicSize = _context.activeLocation.CameraData.CameraSize;
        }

        private void MoveCameraToCharacter()
        {
            var x = _context.character.Transform.position.x + _context.activeLocation.CameraData.Position_X_Offset;
            if (x < _context.activeLocation.CameraData.MoveLeftXLimit)
            {
                x = _context.activeLocation.CameraData.MoveLeftXLimit;
            }
            else if (x > _context.activeLocation.CameraData.MoveRightXLimit)
            {
                x = _context.activeLocation.CameraData.MoveRightXLimit;
            }
        
            var y = _context.activeLocation.CameraData.Position_Y_Offset;
            _cameraServices.CameraMain.transform.position = new Vector3(x, y, _cameraServices.CameraDepthConst);
            _cameraServices.CameraMain.orthographicSize = _context.activeLocation.CameraData.CameraSize;
        }
      
        #endregion Methods
    }
}
