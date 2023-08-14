using UnityEngine;

namespace DungeonMan.Terrain
{
    public class HideOnPlay : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            gameObject.SetActive(false);
        }
    }
}