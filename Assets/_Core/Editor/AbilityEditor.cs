using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DungeonMan.Editors
{
    [CustomEditor(typeof(Ability))]
    public class AbilityEditor : Editor
    {
        float lineHeight;
        float lineHeightSpace;
        List<Type> AbilityBehaviorClasses;
        SerializedProperty behaviors;
        Ability ability;

        string[] options;

        ReorderableList abilityBehaviorsList;

        private void OnEnable()
        {
            lineHeight = EditorGUIUtility.singleLineHeight;
            lineHeightSpace = lineHeight + 4;

            ability = (Ability)target;
            behaviors = serializedObject.FindProperty(nameof(ability._behaviors));
            abilityBehaviorsList = new ReorderableList(serializedObject, behaviors, true, true, true, true);

            if (AbilityBehaviorClasses == null)
            {
                FindAbilityAttributes();

            }

            foreach (var ability in ability._behaviors)
            {
                ability.SelectedID = Array.IndexOf(options, ability.GetType().Name);
            }

            abilityBehaviorsList.drawElementCallback = OnDrawElement;

            abilityBehaviorsList.elementHeightCallback = OnElementHeight;

            abilityBehaviorsList.drawHeaderCallback = DrawHeader;

            abilityBehaviorsList.onAddCallback = (ReorderableList list) =>
            {
                CreateNewBehavior(typeof(Wait), behaviors.arraySize);
            };

            abilityBehaviorsList.onRemoveCallback = (ReorderableList list) =>
            {
                int index = abilityBehaviorsList.index;
                if (behaviors.arraySize > 0)
                {
                    SerializedProperty child = behaviors.GetArrayElementAtIndex(index);
                    DeleteBehavior(child.objectReferenceValue);
                    RenameAllBehaviors();
                }
            };
            //abilityBehaviorsList.onReorderCallbackWithDetails = (ReorderableList list, int oldIndex, int newIndex) =>
            //{
            //    ability._behaviors[oldIndex].name = NameBehavior(ability._behaviors[oldIndex].GetType().Name, newIndex + 1);
            //    ability._behaviors[newIndex].name = NameBehavior(ability._behaviors[newIndex].GetType().Name, oldIndex + 1);
            //};


            abilityBehaviorsList.onMouseUpCallback = (ReorderableList list) =>
            {
                RenameAllBehaviors();
            };
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            abilityBehaviorsList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

        }

        void RenameAllBehaviors()
        {
            int i = 0;
            foreach (var behavior in ability._behaviors)
            {
                i++;
                behavior.name = NameBehavior(behavior.GetType().Name, i);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        }

        private string NameBehavior(string behaviorName, int index)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(index).Append("_").Append(behaviorName);
            return sb.ToString();
        }

        private void FindAbilityAttributes()
        {
            Type AbilityAttribute = typeof(AbilityBehavior);
            Assembly assembly = Assembly.GetAssembly(AbilityAttribute);
            Type[] types = assembly.GetTypes();

            AbilityBehaviorClasses = types.Where(
                t => t.IsSubclassOf(AbilityAttribute)).ToList();

            AbilityBehaviorClasses = AbilityBehaviorClasses.OrderBy(o=>o.Name).ToList();

            options = new string[AbilityBehaviorClasses.Count + 1];
            int index = 1;
            options[0] = "(Remove)";
            foreach (Type type in AbilityBehaviorClasses)
            {
                options[index] = type.Name;
                index++;
            }
        }

        void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Behaviors");
        }

        void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty element = abilityBehaviorsList.serializedProperty.GetArrayElementAtIndex(index);

            SerializedObject elementObject = new SerializedObject(element.objectReferenceValue);
            int selected = elementObject.FindProperty("SelectedID").intValue;

            elementObject.Update();


            //EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, lineHeight), "Test");
            element.isExpanded = EditorGUI.Foldout(new Rect(rect.x + 10, rect.y, rect.width, lineHeight),
                element.isExpanded, options[selected]);

            if (element.isExpanded)
            {
                int lastSelected = selected;
                selected = EditorGUI.Popup(new Rect(rect.x, rect.y + lineHeightSpace, rect.width, lineHeight), "Behavior", selected, options);

                if (lastSelected != selected)
                {
                    // delete old behavior if it exists
                    if (element != null)
                    {
                        DeleteBehavior(element.objectReferenceValue);
                        if (selected == 0)
                        {
                            RenameAllBehaviors();
                            return;
                        }
                    }
                    // create new behavior
                    CreateNewBehavior(AbilityBehaviorClasses[selected - 1], index);
                    element = abilityBehaviorsList.serializedProperty.GetArrayElementAtIndex(index);
                    elementObject = new SerializedObject(element.objectReferenceValue);
                    elementObject.Update();
                }

                SerializedProperty propertyIter = elementObject.GetIterator();

                int i = 2;
                bool showChildern = true;
                propertyIter.NextVisible(true);
                while (propertyIter.NextVisible(showChildern))
                {

                    if (propertyIter.isArray || propertyIter.hasChildren)
                    {
                        showChildern = false;
                    }
                    else showChildern = true;

                    EditorGUI.PropertyField(new Rect(rect.x, rect.y + i * lineHeightSpace, rect.width, lineHeight), propertyIter);

                    i++;
                }

                elementObject.ApplyModifiedProperties();
            }

        }

        float OnElementHeight(int index)
        {
            int i = 1;
            float height = 0;
            SerializedProperty element = abilityBehaviorsList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedObject elementObj = new SerializedObject(element.objectReferenceValue);


            if (element.isExpanded)
            {
                elementObj.Update();
                SerializedProperty propertyIterator = elementObj.GetIterator();

                bool showChildren = true;
                while (propertyIterator.NextVisible(showChildren))
                {
                    i++;
                    if (propertyIterator.isArray || propertyIterator.hasChildren)
                    {
                        showChildren = propertyIterator.isExpanded;
                    }
                }
            }

            height = lineHeightSpace * i;
            return height;
        }

        bool CreateNewBehavior(Type behaviorClass, int index)
        {
            if (AbilityBehaviorClasses.Contains(behaviorClass))
            {
                ScriptableObject instance = ScriptableObject.CreateInstance(behaviorClass);
                instance.name = NameBehavior(behaviorClass.Name, index + 1); // behavior class name + index of ability behaviors list
                AssetDatabase.AddObjectToAsset(instance, ability);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                AbilityBehavior behavior = (AbilityBehavior)instance;
                behavior.SelectedID = Array.IndexOf(options, behaviorClass.Name);
                ability._behaviors.Insert(index, behavior);
                return true;
            }
            return false;
        }

        bool DeleteBehavior(Object behaviorToDelete)
        {
            if (behaviorToDelete != null)
            {
                ability._behaviors.Remove((AbilityBehavior)behaviorToDelete);
                AssetDatabase.RemoveObjectFromAsset(behaviorToDelete);
                Object.DestroyImmediate(behaviorToDelete, true);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                return true;
            }
            return false;
        }
    }
    [CustomEditor(typeof(ChargeAbility))]
    public class ChargeAbilityEditor : AbilityEditor { }

}