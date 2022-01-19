using System;
using UnityEngine;


namespace Rescues
{
    [Serializable]
    public struct CameraData
    {
        [Header("General preferences")]
        [Tooltip("Current camera mode")]
        public CameraMode CameraMode;
        [Tooltip("Camera ortografic size")]
        public float CameraSize;
        public float CameraFreeMoveLimit;
        public int CameraDragSpeed;
        [Header("Moveable camera preferences")]
        [Tooltip("Camera offset by Y")]
        public float Position_Y_Offset;
        [Tooltip("Camera offset by X")]
        public float Position_X_Offset;
        [Tooltip("Left border of level")]
        public float MoveLeftXLimit;
        [Tooltip("Right border of level")]
        public float MoveRightXLimit;
        [Tooltip("Top border of level")]
        public float MoveUpYLimit;
        [Tooltip("bottom border of level")]
        public float MoveDownYLimit;
        [Range(1f, 50f)]
        [Tooltip("Camera movement acceleration")]
        public float CameraAccelerateStep;
        [Range(0f, 10f)]
        [Tooltip("How many steps you can make before camera start movement")]
        public float DeadZone;
    }
}