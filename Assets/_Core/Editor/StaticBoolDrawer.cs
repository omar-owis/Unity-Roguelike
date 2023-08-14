using UnityEditor;
using DungeonMan.CustomAttributes;
using UnityEngine;

namespace DungeonMan
{
    [CustomPropertyDrawer(typeof(StaticBoolAttribute))]
    public class StaticBoolDrawer : PropertyDrawer
    {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, 9 * position.width / 10, position.height)
                , property, label);

            string mainPropertyName = property.name;


            while (property.name != (attribute as StaticBoolAttribute).BooleanValueName)
            {
                property.Next(false);
            }


            EditorGUI.PropertyField(new Rect(position.x + 9 * position.width / 10 + 5, position.y, position.width, position.height)
                , property, new GUIContent(""));
            EditorGUI.LabelField(new Rect(position.x + 9 * position.width / 10 + 6, position.y, position.width, position.height)
                , new GUIContent("", "When set to true, effectiveness factor in the charge ability is taken into account for" +
                " this parameter. Otherwise, effectiveness factor does not effect this parameter"));
        }
    }
}
