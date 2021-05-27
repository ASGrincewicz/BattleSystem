using UnityEngine;

public class Utilities : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }

    public static Ray ScreenPointToRay(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenPointToRay(position);
    }
}