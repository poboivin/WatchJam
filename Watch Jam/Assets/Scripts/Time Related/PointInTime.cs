
using UnityEngine;

public class PointInTime
{

    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Scale;

    public PointInTime (Vector3 _position, Quaternion _rotation, Vector3 _scale )
    {
        Position = _position;
        Rotation = _rotation;
        Scale = _scale;
    }
}
