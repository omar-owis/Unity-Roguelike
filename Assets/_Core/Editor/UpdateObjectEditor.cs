using UnityEngine;
using UnityEditor;
namespace DungeonMan.Terrain
{
    [CustomEditor(typeof(UpdateObject), true)]
    public class UpdateObjectEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UpdateObject data = (UpdateObject)target;

            if (GUILayout.Button("Update"))
            {
                data.ActionOfUpdatedValues();
                EditorUtility.SetDirty(target);
            }
        }
    }
}
