using UnityEngine;

namespace DungeonMan.Terrain
{
    public class UpdateObject : ScriptableObject
    {
        public event System.Action OnUpdate;
        public bool autoUpdate;
#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            if (autoUpdate)
            {
                UnityEditor.EditorApplication.update += ActionOfUpdatedValues;
            }
        }

        public void ActionOfUpdatedValues()
        {
            UnityEditor.EditorApplication.update -= ActionOfUpdatedValues;
            if (OnUpdate != null) OnUpdate();
        }
#endif

    }
}
