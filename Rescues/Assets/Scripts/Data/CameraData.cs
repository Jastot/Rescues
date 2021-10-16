using System;


[Serializable]
public struct CameraData
{   
    public Rescues.CameraMode CameraMode;
    public float CameraSize;
    public float Position_Y_Offset;
    public float Position_X_Offset;
    public float MoveLeftXLimit;
    public float MoveRightXLimit;
    public float CameraFreeMoveLimit;
    public int CameraDragSpeed;
}