using UnityEngine;
using UnityEditor;
using DungeonMan.Terrain;

namespace DungeonMan.Editors
{
    [CustomEditor(typeof(MapPreview))]
    public class MapGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            MapPreview mapGen = (MapPreview)target;

            if (DrawDefaultInspector())
            {
                if (mapGen.autoUpdate)
                {
                    mapGen.DrawMapinEditor();
                }
            }

            if (GUILayout.Button("Generate"))
            {
                mapGen.DrawMapinEditor();
            }
        }
    }
}
