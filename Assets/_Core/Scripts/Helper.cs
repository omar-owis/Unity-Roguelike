using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Helper
{
    public static int defaultInvSize = 5;

    private static Camera _mainCamera;
    public static Camera MainCamera
    {
        get
        {
            if (_mainCamera == null)
            {
                _mainCamera = Camera.main;
            }

            return _mainCamera;
        }
    }

    private static Matrix4x4 isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));

    public static Vector3 ToIso(this Vector3 input) => isoMatrix.MultiplyPoint3x4(input);

    public static Vector3 Abs(Vector3 v) => new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));

    public static string ItemTypeToString(this ItemType type)
    {

        switch (type)
        {
            case ItemType.Trinket:
                return "Trinket";

            case ItemType.PowerUp:
                return "Power-up";
            case ItemType.Resource:
                return "Resource";

            case ItemType.BackPack:
                return "Backpack";
            default:
                return "peepeepoopoo";
        }
    }
}
