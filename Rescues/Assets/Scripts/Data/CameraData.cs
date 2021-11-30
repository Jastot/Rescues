using System;
using UnityEngine;


namespace Rescues
{
    [Serializable]
    public struct CameraData
    {
        [Header("General preferences")]
        public CameraMode CameraMode;
        public float CameraSize;
        public float CameraFreeMoveLimit;
        public int CameraDragSpeed;
        [Header("Moveable camera preferences")]
        public float Position_Y_Offset;
        public float Position_X_Offset;
        public float MoveLeftXLimit;
        public float MoveRightXLimit;
        public float CameraAccelerateStep;
        public float DeadZone;
    } 
}