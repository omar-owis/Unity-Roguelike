using System;
using UnityEditor;
using UnityEngine;

namespace DungeonMan.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class StaticBoolAttribute : PropertyAttribute
    {
        private string _booleanName;

        public string BooleanValueName { get { return _booleanName; } }
        public StaticBoolAttribute(string booleanName)
        {
            _booleanName = booleanName;
        }

    }
}
